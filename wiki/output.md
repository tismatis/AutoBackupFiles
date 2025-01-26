# Outputs
### Currently, we support the following output methods:
- **folder**: Backup to a folder.
- **zip**: Backup to a zip file.

### Folder Output

| Output | Type   | Name | Arg 1  | Arg 2                   |
|--------|--------|------|--------|-------------------------|
| output | folder | *    | (path) | (filename but optional) |

### Zip Output

| Output | Type | Name | Command  | Value |
|--------|------|------|----------|-------|
| output | zip  | *    | path     | *     |
| output | zip  | *    | filename | *     |

### FTP Output

| Output | Type | Name | Command    | Value |
|--------|------|------|------------|-------|
| output | ftp  | *    | host       | *     |
| output | ftp  | *    | encryption | *     |
| output | ftp  | *    | user       | *     |
| output | ftp  | *    | password   | *     |
| output | ftp  | *    | path       | *     |
| output | ftp  | *    | filename   | *     |

### SSH Output

| Output | Type | Name | Command  | Value |
|--------|------|------|----------|-------|
| output | ssh  | *    | host     | *     |
| output | ssh  | *    | keypath  | *     |
| output | ssh  | *    | user     | *     |
| output | ssh  | *    | password | *     |
| output | ssh  | *    | path     | *     |
| output | ftp  | *    | filename | *     |
