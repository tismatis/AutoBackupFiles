using System.Collections.Generic;
using System.IO;

namespace AutoBackupFiles
{
    public class CSVParser
    {
        public static string[][] Parse(string path)
        {
            string[] lines = File.ReadAllLines(path);
            List<string[]> parsedLines = new List<string[]>();
            foreach (var line in lines)
            {
                parsedLines.Add(line.Split(';'));
            }

            return parsedLines.ToArray();
        }
    }
}