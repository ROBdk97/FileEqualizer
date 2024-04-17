# FileEqualizer

## Overview
FileEqualizer is a C# console application designed to synchronize localization files efficiently. It handles discrepancies between different versions of localization files by adding missing entries and managing duplicates. Developed primarily for the [StarCitizen-Localization](https://github.com/Dymerz/StarCitizen-Localization) Repository, this tool can be adapted for other similar use cases.

## Features
- **Duplicate Resolution**: Automatically identifies and allows users to resolve duplicate entries in localization files.
- **Missing Entries Addition**: Adds missing entries from a source file to a target file, ensuring completeness.
- **Interactive Console Interface**: Users are guided through the process via interactive console prompts, ensuring clear and straightforward navigation through tasks.
- **Intelligent Text Reuse**: Attempts to intelligently reuse existing translations by recognizing and adjusting entries with the ',P' suffix.
- **Backup Management**: Automatically creates backup files before making changes, allowing for easy recovery if needed.

### Pre-built Executable
For quick setup, download the pre-built executable:
- [Download the latest release here.](https://github.com/ROBdk97/FIleEqualizer/releases/latest/download/FIleEqualizer.exe)

Simply download and run the `FileEqualizer.exe` file. No additional installation is required.
## Usage
1. **Start the Application**: Launch `FileEqualizer.exe`. Follow the on-screen prompts to input the paths for the source and target files:
   ```plaintext
   Enter the path to the older input file (EN):
   Enter the path to the new input file (EN):
   Enter the path to the output file (de, es, ...):
   ```

2. **File Verification**: The application verifies the existence of the specified files and warns if any file paths overlap.

3. **Processing**: Engages in processing the files by checking for duplicates, adding missing entries, and updating the target file accordingly.

4. **Completion**: Upon successful processing, a "Done!" message is displayed. Press Enter to exit the application.

## Requirements
- Input and output files should be formatted as key=value pairs in UTF-8 encoding.

## Contributing
Your contributions are welcome! Please refer to the project's issues page on GitHub for submitting bug reports, feature requests, or code contributions.

## License
Distributed under the MIT License. See `LICENSE` file for more information.
