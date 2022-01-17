using System.Collections.Generic;

namespace ParseFile
{
    public class ArgParser
    {
        public List<string> PositionalArguments { get; } = new List<string>();

        public List<string> Flags { get; } = new List<string>();

        public ArgParser(string[] arguments)
        {
            foreach (var arg in arguments)
            {
                if (arg.StartsWith("-"))
                {
                    Flags.Add(arg);
                }
                else
                {
                    PositionalArguments.Add(arg);
                }
            }
        }
    }
}
