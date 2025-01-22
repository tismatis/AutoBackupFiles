using AutoBackupFiles.BackupMethods;
using Renci.SshNet;

namespace AutoBackupFiles;

public static class OutputSSH
{
    public static void Backup(string tempDir, SSHConfiguration cfg)
    {
        var zip = Path.Combine(tempDir, $"SSH.{cfg.Name}.zip");
        
        OutputZip.Backup(tempDir, zip);
        
        var connectionInfo = new ConnectionInfo(cfg.Host, cfg.User, new AuthenticationMethod[]
        {
            !string.IsNullOrEmpty(cfg.KeyPath)
                ? new PrivateKeyAuthenticationMethod(cfg.User, new PrivateKeyFile(cfg.KeyPath, cfg.Password))
                : new PasswordAuthenticationMethod(cfg.User, cfg.Password)
        });
        
        using (var sftp = new SftpClient(connectionInfo))
        {
            try
            {
                sftp.Connect();
                Console.Write("Connected to SFTP server.");
                
                using (var fileStream = new FileStream(zip, FileMode.Open))
                {
                    sftp.UploadFile(fileStream, $"{cfg.Path}/{cfg.FileName}");
                    Console.Write("File uploaded successfully.");
                }

                sftp.Disconnect();
            }
            catch (Exception ex)
            {
                Console.Write($"Error: {ex.Message}");
            }
        }
    }
}