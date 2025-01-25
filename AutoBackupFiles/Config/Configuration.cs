namespace AutoBackupFiles;

public class Configuration
{
    public string DateFormat = "dd-mm-yyyy_HH-mm-ss";
    public Dictionary<string, OutputConfiguration> ToOutputs = new();
    public Dictionary<string, ElementToBackup> ToBackups = new();
    public Dictionary<string, ZIPConfiguration> ToZip = new();
    public Dictionary<string, FTPConfiguration> ToFTP = new();
    public Dictionary<string, SSHConfiguration> ToSSH = new();

    public void Finish()
    {
        foreach (var output in ToOutputs)
            output.Value.FixVars(DateTime.Now.ToString(DateFormat));
        foreach (var zip in ToZip)
            zip.Value.FixVars(DateTime.Now.ToString(DateFormat));
        foreach (var ftp in ToFTP)
            ftp.Value.FixVars(DateFormat);
        foreach (var ssh in ToSSH)
            ssh.Value.FixVars(DateFormat);
    }
}