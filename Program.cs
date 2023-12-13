using System.Text;

Console.WriteLine("Enter the path to the input file (EN):");
string inputFilePath = Console.ReadLine();
inputFilePath = inputFilePath.Replace("\"", "");
Console.WriteLine("Enter the path to the output file (de, es, ...):");
string langFilePath = Console.ReadLine();
langFilePath = langFilePath.Replace("\"", "");
await AddMissingEntries(inputFilePath, langFilePath);
Console.WriteLine("Done!");
Console.WriteLine("Press Enter to exit...");
Console.ReadLine();

static async Task AddMissingEntries(string inputFilePath, string outputFilePath)
{
    var readIn = Task.Run(() => File.ReadAllLinesAsync(inputFilePath));
    var readOut = Task.Run(() => File.ReadAllLinesAsync(outputFilePath));

    var file1Lines = await readIn;
    var file2Lines = await readOut;

    Console.WriteLine("Checking for duplicates in file 1");
    file1Lines = await CheckDuplicates(file1Lines);

    Console.WriteLine("Checking for duplicates in file 2");
    file2Lines = await CheckDuplicates(file2Lines);

    // get a dictionary of all keys and values from file1
    var file1Dict = file1Lines.Select(line => line.Split('=', 2)).ToDictionary(split => split[0], split => split[1]);

    // get a dictionary of all keys and values from file2
    var file2Dict = file2Lines.Select(line => line.Split('=', 2)).ToDictionary(split => split[0], split => split[1]);

    // update file2Dict with missing keys from file1Dict
    UpdateTranslations(file1Dict, ref file2Dict);

    // sort file2Dict to have the same order as file1Dict
    var orderedList = file1Dict.Keys
    .Where(file2Dict.ContainsKey) // Ensure the key exists in dict2
    .Select(key => new KeyValuePair<string, string>(key, file2Dict[key]))
    .ToList();

    // Create a string builder to build the output file
    var outputBuilder = new StringBuilder();
    // append key value pairs from file2Dict it a "=" as sepeartor
    foreach (var item in orderedList)
    {
        outputBuilder.AppendLine($"{item.Key}={item.Value}");
    }
    // write the output file
    await File.WriteAllTextAsync(outputFilePath, outputBuilder.ToString(), new UTF8Encoding(true));
}

// Check for duplicates and ask the user to choose one
static Task<string[]> CheckDuplicates(string[] lines)
{
    var keyValuePairs = new Dictionary<string, List<string>>();

    // First pass to group values by keys
    foreach (var line in lines)
    {
        var parts = line.Split('=', 2);
        var key = parts[0];
        var value = parts.Length > 1 ? parts[1] : "";

        if (!keyValuePairs.ContainsKey(key))
        {
            keyValuePairs[key] = new List<string>();
        }
        keyValuePairs[key].Add(value);
    }

    // User interaction for duplicates
    foreach (var pair in keyValuePairs)
    {
        if (pair.Value.Count > 1)
        {
            // dont ask if they are the same
            if (pair.Value.Distinct().Count() == 1)
            {
                keyValuePairs[pair.Key] = new List<string> { pair.Value[0] };
                continue;
            }
            Console.WriteLine($"Multiple values found for key '{pair.Key}'. Please choose one:");
            for (int i = 0; i < pair.Value.Count; i++)
            {
                Console.WriteLine($"{i + 1}: {pair.Value[i]}");
            }

            int choice;
            do
            {
                Console.Write("Enter the number of the value you want to keep: ");
            } while (!int.TryParse(Console.ReadLine(), out choice) || choice < 1 || choice > pair.Value.Count);

            // Keep only the chosen value
            keyValuePairs[pair.Key] = new List<string> { pair.Value[choice - 1] };
        }
    }
    // Construct the final list of lines
    var finalLines = keyValuePairs.SelectMany(pair => pair.Value.Select(value => $"{pair.Key}={value}")).ToList();

    // Return the final list
    return Task.FromResult(finalLines.ToArray());
}

static void UpdateTranslations(Dictionary<string, string> file1Dict, ref Dictionary<string, string> file2Dict)
{
    Console.WriteLine("Updating translations");
    var keysToUpdate = new List<string>();

    foreach (var item in file1Dict)
    {
        string key = item.Key;
        string keyVariant = key.EndsWith(",P") ? key.Remove(key.Length - 2) : key + ",P";

        if (file2Dict.ContainsKey(key))
        {
            // If the exact key exists, no need to update
            continue;
        }
        else if (file2Dict.ContainsKey(keyVariant))
        {
            // Add to a list to update after iterating (to avoid modifying the collection while iterating)
            keysToUpdate.Add(keyVariant);
            Console.WriteLine($"Using existing translation for {keyVariant}");
        }
        else
        {
            // add the key and value to file2Dict
            file2Dict[key] = item.Value;
            Console.WriteLine($"Adding new entry: \"{item.Key}={item.Value}\"");
        }
    }

    // Update keys in file2Dict
    foreach (var oldKey in keysToUpdate)
    {
        string newKey = oldKey.EndsWith(",P") ? oldKey.Remove(oldKey.Length - 2) : oldKey + ",P";
        string value = file2Dict[oldKey];
        file2Dict.Remove(oldKey);
        file2Dict[newKey] = value;
    }
}