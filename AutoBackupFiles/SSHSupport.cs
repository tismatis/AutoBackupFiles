using Renci.SshNet;

namespace AutoBackupFiles;

public static class SSHSupport
{
    public static void UploadFileToSFTP(string host, string localFilePath, string remoteFilePath, string username, string password, string privateKeyPath = "")
    {
        var connectionInfo = new ConnectionInfo(host, username, new AuthenticationMethod[]
        {
            !string.IsNullOrEmpty(privateKeyPath)
                ? new PrivateKeyAuthenticationMethod(username, new PrivateKeyFile(privateKeyPath, password))
                : new PasswordAuthenticationMethod(username, password)
        });
        
        using (var sftp = new SftpClient(connectionInfo))
        {
            try
            {
                sftp.Connect();
                Console.Write("Connected to SFTP server.");
                
                using (var fileStream = new FileStream(localFilePath, FileMode.Open))
                {
                    sftp.UploadFile(fileStream, remoteFilePath);
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