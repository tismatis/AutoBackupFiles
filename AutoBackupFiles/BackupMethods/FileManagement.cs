namespace AutoBackupFiles.BackupMethods;

public static class FileManagement
{
    public static void CopyDirectory(string sourceDir, string destDir)
    {
        if (!Directory.Exists(destDir))
        {
            Directory.CreateDirectory(destDir);
        }

        foreach (var file in Directory.GetFiles(sourceDir))
        {
            var destFile = Path.Combine(destDir, Path.GetFileName(file));
            File.Copy(file, destFile, true);
        }

        foreach (var directory in Directory.GetDirectories(sourceDir))
        {
            var destDirectory = Path.Combine(destDir, Path.GetFileName(directory));
            CopyDirectory(directory, destDirectory);
        }
    }
    
    public static void CopyDirectory(string sourceDir, string destDir, List<string[]> list)
    {
        if (!Directory.Exists(destDir))
        {
            Directory.CreateDirectory(destDir);
        }

        foreach (var file in Directory.GetFiles(sourceDir))
        {
            if(list.Any(l => l[0] == "file" && l[1] == file) && list.Any(l => l[0] == "folder" && file.StartsWith(l[1])))
                continue;
            
            var destFile = Path.Combine(destDir, Path.GetFileName(file));
            File.Copy(file, destFile, true);
        }

        foreach (var directory in Directory.GetDirectories(sourceDir))
        {
            if(list.Any(l => l[0] == "folder" && directory.StartsWith(l[1])))
                continue;

            var destDirectory = Path.Combine(destDir, Path.GetFileName(directory));
            CopyDirectory(directory, destDirectory, list);
        }
    }

    public static void CopyFile(string sourceFile, string destFile, List<string[]> list)
    {
        if(list.Any(l => l[0] == "file" && l[1] == sourceFile) && list.Any(l => l[0] == "folder" && sourceFile.StartsWith(l[1])))
            return;
        
        CopyFile(sourceFile, destFile);
    }
    
    public static void CopyFile(string sourceFile, string destFile)
    {
        var destDir = Path.GetDirectoryName(destFile);
        
        if (!Directory.Exists(destDir))
        {
            if (destDir != null) Directory.CreateDirectory(destDir);
        }
        
        File.Copy(sourceFile, destFile, true);
    }
}