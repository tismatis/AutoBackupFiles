# Auto Backup Files

Auto Backup Files is a C# application designed to automate the backup of specified files and folders to a designated destination. The application reads a configuration file in CSV format to determine which files and folders to back up and where to store the backups.

## Features

- Reads configuration from a CSV file
- Supports backup of both files and folders
- Fully modular and customizable
- Able to push backups into a zip file
- Logs all operations to a `logs.txt` file
- Provides console output with detailed messages

## Usage

1. **Prepare the Configuration File**: Create a CSV file (`config.csv`) with the following structure:
    ```csv
    # Config
    config;date-format;dd-mm-yyyy HH--mm--ss
    # Destination
    output;folder;Main Output;C:/Path/To/Backup/%DATE%
    # Folders to backup
    backup;folder;FolderName;C:/Path/To/Folder
    backup;ignore-folder;FolderName;C:/Path/To/Folder/IgnoreMePlease
    backup;ignore-file;FolderName;C:/Path/To/Folder/private.txt
    backup;file;FileName;C:/Path/To/File
    ```

2. **Run the Application**: Execute the application from the command line, providing the path to the CSV configuration file as an argument:
    ```sh
    AutoBackupFiles.exe --path=config.csv
    ```

3. **Optional Arguments**: You can use the following optional arguments as the second argument:
    - `--force-special-chars=(true/false)`: Forces the use of special characters in the console output.
    - `--force-save-downloads=(true/false)`: Forces the saving in logs.txt of each line of the download status.
    - `--verbose-csv-parsing=(true/false)`: Enables verbose logging of the CSV parsing.
    - `--help`: Displays the help message.
   Example:
    ```sh
    AutoBackupFiles.exe --path=config.csv --force-special-chars=true --verbose-csv-parsing=true
    ```

## Configuration File Format

Please check each md files in the `wiki` folder for more information about the configuration file format.
* [backup](wiki/backup.md)
* [config](wiki/config.md)
* [obsolete](wiki/obsolete.md) *Note: please don't use this tag, he is just here for compatibility reasons.*
* [output](wiki/output.md)

## Example

Here is an example `config.csv` file:
```csv
# Config
config;date-format;dd-mm-yyyy HH--mm--ss
# Destination
destination;C:/Users/Username/Backup/%DATE%
# Folders to backup
folder;Documents;C:/Users/Username/Documents
file;ImportantFile;C:/Users/Username/ImportantFile.txt
```

## Logging

All operations are logged to a `logs.txt` file in the application's directory. This includes messages about the progress and any errors encountered during the backup process.

## License

This project is licensed under the GNU v3 License. See the `LICENSE` file for details.
