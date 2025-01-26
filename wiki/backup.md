# Backup

### Currently, we support the following backup methods:
- **folder**: Backup a folder.
- **file**: Backup a file.
- **ignore-folder**: Ignore a specific folder during backup.
- **ignore-file**: Ignore a specific file during backup.

### Backup Configuration

| Backup | Type         | Name | Path                      |
|--------|--------------|------|---------------------------|
| backup | folder       | *    | (path)                    |
| backup | file         | *    | (path)                    |
| backup | ignore-folder| *    | (path to ignore)          |
| backup | ignore-file  | *    | (path to ignore)          |

### Example Configuration

```csv
# Config
config;date-format;dd-mm-yyyy HH--mm--ss
# Destination
destination;C:/Users/Username/Backup/%DATE%
# Folders to backup
backup;folder;Documents;C:/Users/Username/Documents
backup;file;ImportantFile;C:/Users/Username/ImportantFile.txt
backup;ignore-folder;Documents;C:/Users/Username/Documents/IgnoreMe
backup;ignore-file;Documents;C:/Users/Username/Documents/private.txt