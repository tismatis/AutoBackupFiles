using System.Diagnostics;
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

        Stopwatch stopwatch = new Stopwatch();
        
        stopwatch.Start();
        using (var zip = ZipFile.Open($"{cfg.Path}/{cfg.FileName}", ZipArchiveMode.Create))
        {
            for (int i = 0; i < files.Length; i++)
            {
                var file = files[i];
                var entryName = Path.GetRelativePath(tempDir, file);
                zip.CreateEntryFromFile(file, entryName);
                processedSize += new FileInfo(file).Length;
                double progress = (double)processedSize / totalSize * 100;
                Console.SetCursorPosition(0, Console.CursorTop - 1);
                Console.Write($"&eCreating zip file... [{progress:0.00}% {Math.Round(processedSize*100f/Math.Round(stopwatch.ElapsedMilliseconds*1000f))/100}Mo/s]", Program.ForceSaveDownloadLog);
            }
        }
        stopwatch.Stop();
        Console.Write($"&aZip file created!");
    }
}