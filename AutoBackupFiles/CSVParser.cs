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
                if(line.StartsWith("#") || line == "" || line.StartsWith(" "))
                    continue;
                
                parsedLines.Add(line.Split(';'));
            }

            return parsedLines.ToArray();
        }
    }
}