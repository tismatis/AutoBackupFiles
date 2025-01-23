using FluentFTP;

namespace AutoBackupFiles;

public class FTPConfiguration
{
    public string Name;
    public string Host;
    public string Encryption;
    public FtpEncryptionMode EncryptionMode = FtpEncryptionMode.Auto;
    public string User;
    public string Password;
    public string Path;
    public string FileName = "%DATE%.zip";

    public FTPConfiguration(string name)
    {
        Name = name;
    }

    public void SetEncryption(string encryption)
    {
        switch (encryption)
        {
            case "NoEncryption":
                EncryptionMode = FtpEncryptionMode.None;
                break;
            case "Implicit":
                EncryptionMode = FtpEncryptionMode.Implicit;
                break;
            case "Explicit":
                EncryptionMode = FtpEncryptionMode.Explicit;
                break;
            default:
                throw new ArgumentException("Invalid encryption type");
        }
        Encryption = encryption;
    }

    public void FixVars(string date)
    {
        FileName = FileName.Replace("%DATE%", date);
        Path = Path.Replace("%DATE%", date);
    }
}