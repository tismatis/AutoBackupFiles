using System.Net;
using FluentFTP;

namespace AutoBackupFiles;

public static class FTPSupport
{
    public static void UploadFileToFtp(FtpEncryptionMode mode, string ftpUrl, string localFilePath, string remoteFilePath, string ftpUser, string ftpPassword)
    {
        using (var ftpClient = new FtpClient(ftpUrl, ftpUser, ftpPassword))
        {
            ftpClient.Config.EncryptionMode = mode;
            ftpClient.Config.ValidateAnyCertificate = true;
            ftpClient.Connect();

            Console.Write("Starting upload...");
            ftpClient.UploadFile(localFilePath, remoteFilePath, FtpRemoteExists.Overwrite);
            Console.Write("Upload completed successfully!");

            ftpClient.Disconnect();
        }
    }
}