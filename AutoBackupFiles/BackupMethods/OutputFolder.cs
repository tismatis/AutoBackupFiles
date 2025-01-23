namespace AutoBackupFiles.BackupMethods;

public static class OutputFolder
{
    public static void Backup(string tempDir, OutputConfiguration cfg)
    {
        Console.Write($"&7Backup into Folder {cfg.Name}...");
        if(!Directory.Exists(tempDir))
            throw new FileNotFoundException("Could not find the backup folder");
        if (!Directory.Exists($"{cfg.Path}/{cfg.FolderName}"))
            Directory.CreateDirectory($"{cfg.Path}/{cfg.FolderName}");
        FileManagement.CopyDirectory(tempDir, $"{cfg.Path}/{cfg.FolderName}");
        Console.Write("&aBackup folder created!");
    }
}