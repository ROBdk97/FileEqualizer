# Localization File Synchronization Tool

## Overview
This C# console application is designed to synchronize and update localization files. It helps maintain consistency between different language files by adding missing entries and resolving duplicate values.
This tool was originally developed to synchronize localization files for the [StarCitizen-Localization](https://github.com/Dymerz/StarCitizen-Localization) Repository.

## Features
- **Duplicate Resolution**: Identifies and resolves duplicate entries in localization files.
- **Missing Entry Addition**: Automatically adds missing entries from a source file to a target file.
- **Console-based User Interaction**: Provides interactive prompts for user inputs and choices.

## Requirements
- .NET Core or .NET Framework compatible with C#.
- Input localization files in a specific format (key=value pairs).

## Usage
1. **Start the Program**: Run the application. It will prompt for the paths of the input and output files.
   
   ```
   Enter the path to the input file (EN):
   Enter the path to the output file (de, es, ...):
   ```

2. **Provide File Paths**: Enter the full paths for both the source (English, by default) and target (other languages) localization files. 

3. **Processing**: The application checks for duplicates in both files, resolves them (with user input if necessary), adds missing entries from the source to the target file, and then sorts and updates the target file.

4. **Completion**: Once processing is done, a "Done!" message appears. Press Enter to exit the application.

## Input File Format
- The expected format for both input and output files is a series of key-value pairs, each on a new line, formatted as `key=value`.

## Notes
- The application expects files to be in a UTF-8 encoding.
- It handles Placeholder keys by checking for keys ending with `,P` or without a `,P` at the end.
- User is required to resolve ambiguities in case of duplicate values for the same key.

## Contributing
Contributions, bug reports, and feature requests are welcome. Please refer to the project's issue tracker to contribute.