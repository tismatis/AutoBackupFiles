namespace AutoBackupFiles.Keys;

public abstract class GenericParserKey
{
    public abstract string GetName();

    public abstract void OnParseCSV(ref Configuration configuration, string[] line);
}