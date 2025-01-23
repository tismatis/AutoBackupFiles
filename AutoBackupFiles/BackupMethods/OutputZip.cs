using System.IO.Compression;

namespace AutoBackupFiles.BackupMethods;

public static class OutputZip
{
    public static void Backup(string tempDir, ZIPConfiguration cfg)
    {
        Console.Write($"&7Creating zip file {cfg.Name}...");

        var files = Directory.GetFiles(tempDir, "*", SearchOption.AllDirectories);
        long totalSize = files.Sum(file => new FileInfo(file).Length);
        long processedSize = 0;

        using (var zip = ZipFile.Open($"{cfg.Path}/{cfg.Name}", ZipArchiveMode.Create))
        {
            for (int i = 0; i < files.Length; i++)
            {
                var file = files[i];
                var entryName = Path.GetRelativePath(tempDir, file);
                zip.CreateEntryFromFile(file, entryName);
                processedSize += new FileInfo(file).Length;
                double progress = (double)processedSize / totalSize * 100;
                Console.SetCursorPosition(0, Console.CursorTop - 1);
                Console.Write($"&eCreating zip file... {progress:0.00}% complete! [{(processedSize / 1024.0 / 1024.0):0.00}Mo processed, {(totalSize - processedSize) / 1024.0 / 1024.0:0.00}Mo remaining]", Program.ForceSaveDownloadLog);
            }
        }
        Console.Write($"&aZip file created!");
    }
    
    public static void Backup(string tempDir, string pathAndName)
    {
        Console.Write("&aBackup done &lsuccessfully&r&a!");
        Console.Write("&7Creating zip file...");

        var files = Directory.GetFiles(tempDir, "*", SearchOption.AllDirectories);
        long totalSize = files.Sum(file => new FileInfo(file).Length);
        long processedSize = 0;

        using (var zip = ZipFile.Open(pathAndName, ZipArchiveMode.Create))
        {
            for (int i = 0; i < files.Length; i++)
            {
                var file = files[i];
                var entryName = Path.GetRelativePath(tempDir, file);
                zip.CreateEntryFromFile(file, entryName);
                processedSize += new FileInfo(file).Length;
                double progress = (double)processedSize / totalSize * 100;
                Console.SetCursorPosition(0, Console.CursorTop - 1);
                Console.Write($"&eCreating zip file... {progress:0.00}% complete! [{(processedSize / 1024.0 / 1024.0):0.00}Mo processed, {(totalSize - processedSize) / 1024.0 / 1024.0:0.00}Mo remaining]", Program.ForceSaveDownloadLog);
            }
        }
        Console.Write("&aZip file created!");
    }
}