using System.Text;

Console.WriteLine("Enter the path to the older input file (EN):");
string oldInputFilePath = Console.ReadLine();
Console.WriteLine("Enter the path to the new input file (EN):");
string inputFilePath = Console.ReadLine();
Console.WriteLine("Enter the path to the output file (de, es, ...):");
string langFilePath = Console.ReadLine();
CheckPaths(ref oldInputFilePath, ref inputFilePath, ref langFilePath);
await AddMissingEntries(oldInputFilePath, inputFilePath, langFilePath);
Console.WriteLine("Done!");
Console.WriteLine("Press Enter to exit...");
Console.ReadLine();

static void CheckPaths(ref string oldInputFilePath, ref string inputFilePath, ref string langFilePath)
{
    // Remove quotes from the paths
    oldInputFilePath = oldInputFilePath.Replace("\"", "");
    inputFilePath = inputFilePath.Replace("\"", "");
    langFilePath = langFilePath.Replace("\"", "");
    // Check if the files exist
    if (!File.Exists(oldInputFilePath))
    {
        Console.WriteLine("Old input file does not exist.");
        Environment.Exit(1);
    }
    if (!File.Exists(inputFilePath))
    {
        Console.WriteLine("Input file does not exist.");
        Environment.Exit(1);
    }
    if (!File.Exists(langFilePath))
    {
        Console.WriteLine("Output file does not exist.");
        Environment.Exit(1);
    }
    // Check oldInput and are the same and give a warning
    if (oldInputFilePath == inputFilePath)
    {
        Console.WriteLine("Warning: Old input file and input file are the same.");
        Console.Write("Do you want to continue? (y/n): ");
        if (Console.ReadLine().ToLower() != "y")
        {
            Environment.Exit(1);
        }
    }
    // Copy langFilePath to a backup file
    File.Copy(langFilePath, langFilePath + ".old", true);
}

static async Task AddMissingEntries(string oldInputFilePath, string inputFilePath, string outputFilePath)
{
    var readOldIn = Task.Run(() => File.ReadAllLinesAsync(oldInputFilePath));
    var readIn = Task.Run(() => File.ReadAllLinesAsync(inputFilePath));
    var readOut = Task.Run(() => File.ReadAllLinesAsync(outputFilePath));

    var OldFile1Lines = await readOldIn;
    var file1Lines = await readIn;
    var file2Lines = await readOut;

    Console.WriteLine("Checking for duplicates in file 1");
    file1Lines = await CheckDuplicates(file1Lines);

    Console.WriteLine("Checking for duplicates in file 2");
    file2Lines = await CheckDuplicates(file2Lines);

    var oldFile1Dict = OldFile1Lines.Select(line => line.Split('=', 2)).ToDictionary(split => split[0], split => split[1]);
    // get a dictionary of all keys and values from file1
    var file1Dict = file1Lines.Select(line => line.Split('=', 2)).ToDictionary(split => split[0], split => split[1]);
    // Check for changes between the old file and the new input file and use the new value
    var changes = file1Dict.Where(pair => oldFile1Dict.ContainsKey(pair.Key) && oldFile1Dict[pair.Key] != pair.Value).ToDictionary();
    Console.WriteLine($"Found {changes.Count} changes between the old input file and the new input file");

    // get a dictionary of all keys and values from file2
    var file2Dict = file2Lines.Select(line => line.Split('=', 2)).ToDictionary(split => split[0], split => split[1]);

    // update file2Dict with missing keys from file1Dict
    UpdateTranslations(file1Dict, ref file2Dict, changes);

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

static void UpdateTranslations(Dictionary<string, string> file1Dict, ref Dictionary<string, string> file2Dict, Dictionary<string, string> changes)
{
    Console.WriteLine("Updating translations");
    var keysToUpdate = new List<string>();

    foreach (var item in file1Dict)
    {
        string key = item.Key;
        string keyVariant = key.EndsWith(",P") ? key.Remove(key.Length - 2) : key + ",P";

        if (file2Dict.ContainsKey(key))
        {
            if (changes.ContainsKey(key))
            {
                keysToUpdate.Add(key);
            }
            else if(file2Dict.ContainsKey(keyVariant))
            {
                keysToUpdate.Add(keyVariant);
            }
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
        if (file2Dict.ContainsKey(oldKey))
        {
            string value = file2Dict[oldKey];
            file2Dict.Remove(oldKey);
            file2Dict[newKey] = value;
        }
        // Overwrite the value if it was changed
        if (changes.ContainsKey(oldKey))
        {
            file2Dict[oldKey] = changes[oldKey];
        }
    }
}