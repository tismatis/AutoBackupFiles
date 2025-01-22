using System.Net;
using AutoBackupFiles.BackupMethods;
using FluentFTP;

namespace AutoBackupFiles;

public static class OutputFTP
{
    public static void Backup(string tempDir, FTPConfiguration cfg)
    {
        var zip = Path.Combine(tempDir, $"FTP.{cfg.Name}.zip");
        
        OutputZip.Backup(tempDir, zip);
        
        using (var ftpClient = new FtpClient(cfg.Host, cfg.User, cfg.Password))
        {
            ftpClient.Config.EncryptionMode = cfg.EncryptionMode;
            ftpClient.Config.ValidateAnyCertificate = true;
            ftpClient.Connect();

            Console.Write("Starting upload...");
            ftpClient.UploadFile(zip, $"{cfg.Path}/{cfg.FileName}", FtpRemoteExists.Overwrite);
            Console.Write("Upload completed successfully!");

            ftpClient.Disconnect();
        }
    }
}