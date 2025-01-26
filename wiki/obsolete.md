# Obsolete

### Currently, we support the following obsolete settings:
- **folder**: Obsolete a folder.
- **file**: Obsolete a file.
- **ignore-folder**: Ignore a specific folder during obsolescence.
- **ignore-file**: Ignore a specific file during obsolescence.

*The 'obsolete' key is optional for this one for compatibility reason, that mean `obsolete;folder;MyFile;C:/Users/` equals `folder;MyFile;C:/Users/`*

### Obsolete Configuration

| Obsolete | Type         | Name | Path                      |
|----------|--------------|------|---------------------------|
| obsolete | folder       | *    | (path)                    |
| obsolete | file         | *    | (path)                    |
| obsolete | ignore-folder| *    | (path to ignore)          |
| obsolete | ignore-file  | *    | (path to ignore)          |

### Example Configuration

```csv
# Config
config;date-format;dd-mm-yyyy HH--mm--ss
# Destination
destination;C:/Users/Username/Backup/%DATE%
# Folders to obsolete
obsolete;folder;Documents;C:/Users/Username/Documents
obsolete;file;ImportantFile;C:/Users/Username/ImportantFile.txt
obsolete;ignore-folder;Documents;C:/Users/Username/Documents/IgnoreMe
obsolete;ignore-file;Documents;C:/Users/Username/Documents/private.txt
```