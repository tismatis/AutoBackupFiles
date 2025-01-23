namespace AutoBackupFiles;

public class SSHConfiguration
{
    public string Name;
    public string Host = "";
    public string KeyPath = "";
    public string User = "";
    public string Password = "";
    public string Path;
    public string FileName = "%DATE%.ZIP";

    public SSHConfiguration(string name)
    {
        Name = name;
    }

    public void FixVars(string date)
    {
        FileName = FileName.Replace("%DATE%", date);
        Path = Path.Replace("%DATE%", date);
    }
}