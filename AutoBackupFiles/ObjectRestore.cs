using System.Collections.Generic;

namespace AutoBackupFiles
{
    internal class ObjectRestore
    {
        public string Name;
        public List<string[]> Paths;
        public List<string[]> Banned;

        public ObjectRestore(string name)
        {
            Name = name;
            Paths = new List<string[]>();
            Banned = new List<string[]>();
        }

        public override string ToString()
        {
            string final = "";
            foreach(string[] path in Paths)
                final += $"{path[0]};{path[1]}\n";
            return final;
        }
    }
}