namespace AutoBackupFiles;

public class Configuration
{
    public string DateFormat = "dd-mm-yyyy_HH-mm-ss";
    public OutputConfiguration[] ToOutputs;
    public ElementToBackup[] ToBackups;
    public ZIPConfiguration[] ToZip;
    public FTPConfiguration[] ToFTP;
    public SSHConfiguration[] ToSSH;

    public void Finish(List<OutputConfiguration> toOutputs, List<ElementToBackup> toBackups, List<ZIPConfiguration> zipConfigurations, List<FTPConfiguration> ftpConfigurations, List<SSHConfiguration> sshConfigurations)
    {
        ToOutputs = toOutputs.ToArray();
        ToZip = zipConfigurations.ToArray();
        ToBackups = toBackups.ToArray();
        ToFTP = ftpConfigurations.ToArray();
        ToSSH = sshConfigurations.ToArray();

        string date = DateTime.Now.ToString(DateFormat);
        foreach (var output in toOutputs)
            output.FixVars(date);
        foreach (var zip in zipConfigurations)
            zip.FixVars(date);
        foreach (var ftp in ftpConfigurations)
            ftp.FixVars(date);
        foreach (var ssh in sshConfigurations)
            ssh.FixVars(date);
    }
}