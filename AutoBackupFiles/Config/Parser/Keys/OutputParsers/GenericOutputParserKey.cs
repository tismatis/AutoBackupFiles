namespace AutoBackupFiles.Config.Parser.Keys.OutputParsers;

public abstract class GenericOutputParserKey
{
    public abstract string GetName();

    public abstract void OnParseCSV(ref Configuration configuration, string[] line);
}