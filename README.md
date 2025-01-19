# Auto Backup Files

Auto Backup Files is a C# application designed to automate the backup of specified files and folders to a designated destination. The application reads a configuration file in CSV format to determine which files and folders to back up and where to store the backups.

## Features

- Reads configuration from a CSV file
- Supports backup of both files and folders
- Customizable date format for destination paths
- Logs all operations to a `logs.txt` file
- Provides console output with color-coded messages

## Usage

1. **Prepare the Configuration File**: Create a CSV file (`config.csv`) with the following structure:
    ```csv
    # Config
    config;date-format;dd-mm-yyyy HH--mm--ss
    # Destination
    destination;C:/Path/To/Backup/%DATE%
    # Folders to backup
    folder;FolderName;C:/Path/To/Folder
    file;FileName;C:/Path/To/File
    ```

2. **Run the Application**: Execute the application from the command line, providing the path to the CSV configuration file as an argument:
    ```sh
    AutoBackupFiles.exe config.csv
    ```

## Configuration File Format

- **Config Section**: Define global settings such as date format.
    ```csv
    config;date-format;dd-mm-yyyy HH--mm--ss
    ```

- **Destination Section**: Specify the backup destination path. The `%DATE%` placeholder will be replaced with the current date and time.
    ```csv
    destination;C:/Path/To/Backup/%DATE%
    ```

- **Folders and Files to Backup**: List the folders and files to be backed up.
    ```csv
    folder;FolderName;C:/Path/To/Folder
    file;FileName;C:/Path/To/File
    ```

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

This project is licensed under the MIT License. See the `LICENSE` file for details.