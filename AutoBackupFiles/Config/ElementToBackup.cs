namespace AutoBackupFiles;

public class ElementToBackup
{
    public string Name;
    public List<string[]> ToInclude;
    public List<string[]> ToExclude;

    public ElementToBackup(string name)
    {
        Name = name;
        ToInclude = new List<string[]>();
        ToExclude = new List<string[]>();
    }

    public override string ToString()
    {
        string output = "";
        ToInclude.ForEach(x => output += $"include;{x[0]};{x[1]}\n");
        ToExclude.ForEach(x => output += $"exclude;{x[0]};{x[1]}\n");
        return output;
    }
}