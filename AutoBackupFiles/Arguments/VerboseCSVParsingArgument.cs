namespace AutoBackupFiles.Arguments;

public class VerboseCSVParsingArgument : GenericArgument
{
    public override string GetDescription() => "Forces the Program to parse the CSV file in verbose mode or not.";

    public override string GetArg() => "verbose-csv-parsing";

    public override void Execute(string[] split)
    {
        CSVParser.VerboseParsing = bool.Parse(split[1]);
    }
}