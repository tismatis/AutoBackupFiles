namespace AutoBackupFiles;

public class Configuration
{
    public string DateFormat = "dd-mm-yyyy_HH-mm-ss";
    public OutputConfiguration[] ToOutputs;
    public ElementToBackup[] ToBackups;
    public ZIPConfiguration[] ToZip;

    public void Finish(List<OutputConfiguration> toOutputs, List<ElementToBackup> toBackups, List<ZIPConfiguration> zipConfigurations)
    {
        ToOutputs = toOutputs.ToArray();
        ToZip = zipConfigurations.ToArray();
        ToBackups = toBackups.ToArray();

        foreach (var output in toOutputs)
            output.FixVars(DateTime.Now.ToString(DateFormat));
        foreach (var zip in zipConfigurations)
            zip.FixVars(DateTime.Now.ToString(DateFormat));
    }
}