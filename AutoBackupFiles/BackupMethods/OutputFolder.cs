namespace AutoBackupFiles.BackupMethods;

public static class OutputFolder
{
    public static void Backup(string tempDir, OutputConfiguration cfg)
    {
        if(!Directory.Exists(tempDir))
            throw new FileNotFoundException("Could not find the backup folder");
        
        FileManagement.CopyDirectory(tempDir, $"{cfg.Path}/{cfg.FolderName}");
    }
}