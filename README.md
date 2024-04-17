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
- [Download the latest release here.](https://github.com/ROBdk97/FileEqualizer/releases/latest/download/FIleEqualizer.exe)

Simply download and run the `FileEqualizer.exe` file. No additional installation is required.
## Usage

FileEqualizer simplifies the synchronization of localization files through an interactive console application. Follow these steps to use the tool effectively:

### Running the Application
1. **Launch the Tool**: Execute `FileEqualizer.exe`. The application will prompt you to enter the paths for the source and target localization files.

### Inputting File Paths
2. **Provide File Paths**:
   - **Old Input File Path**: Enter the full path to the older version of the input file (typically in English).
     ```
     Enter the path to the older input file (EN):
     ```
   - **New Input File Path**: Enter the full path to the newer version of the input file (also typically in English).
     ```
     Enter the path to the new input file (EN):
     ```
   - **Output File Path**: Specify the path where the updated localization file will be saved (other languages).
     ```
     Enter the path to the output file (de, es, ...):
     ```

### Processing Files
3. **File Validation**: The application verifies that all specified files exist and are accessible. It will also check for path overlaps and confirm actions with the user if needed.

4. **Handling Duplicates and Adding Entries**: FileEqualizer will automatically detect duplicates within the files and allow you to choose which entries to keep. It will then add missing entries from the new input file to the output file and update it accordingly.

### Completing the Process
5. **Final Steps**: After processing the files, the application will display a "Done!" message. Press Enter to close the application.

### Notes
- Ensure that all paths are correctly entered to avoid errors.
- The application supports files in UTF-8 format to maintain consistency across different systems and languages.

## Troubleshooting
If you encounter issues during the usage of FileEqualizer, consider the following:
- **File Paths**: Verify that all file paths are correct and that the files exist.
- **Permissions**: Ensure the application has the necessary permissions to read and write the files.
- **File Encoding**: Confirm that the files are encoded in UTF-8, as other encodings might cause unexpected behavior.

## Requirements
- Input and output files should be formatted as key=value pairs in UTF-8 encoding.

## Contributing
Your contributions are welcome! Please refer to the project's issues page on GitHub for submitting bug reports, feature requests, or code contributions.

## License
Distributed under the MIT License. See `LICENSE` file for more information.
