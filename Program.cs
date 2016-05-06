using System.Collections.Generic;

namespace FindConflictingReferences
{
    // Based on https://gist.github.com/brianlow/1553265 and https://gist.github.com/WaffleSouffle/bcc3eaebaa7a7cadcab6
    public class Program
    {
        private static void FindConflicts(System.IO.TextWriter output, string path)
        {
            var groupsOfConflicts = FindConflictingReferenceFunctions.GetReferencedAssembliesWithMultipleVersions(path);

            foreach (var group in groupsOfConflicts)
            {
                output.WriteLine("Possible conflicts for {0}:", group.Key);
                foreach (var reference in group)
                {
                    output.WriteLine("{0} references {1}",
                        reference.Assembly.Name.PadRight(25),
                        reference.ReferencedAssembly.FullName);
                }
                output.WriteLine();
            }
        }

        public static void Main(string[] args)
        {
            foreach (var path in GetPathsFromArgs(args))
            {
                FindConflicts(System.Console.Out, path);
            }
        }

        private static List<string> GetPathsFromArgs(string[] args)
        {
            var paths = new List<string>();

            foreach (var arg in args)
            {
                if (arg.StartsWith("-"))
                {
                    if (arg.StartsWith("--"))
                    {
                        // Long option.
                    }
                    else
                    {
                        // Short option.
                    }
                }
                else if (arg.StartsWith("/"))
                {
                    // Windows option.
                }
                else
                {
                    paths.Add(arg);
                }
            } // Ends loop over arguments

            if (paths.Count == 0)
            {
                paths.Add(System.IO.Directory.GetCurrentDirectory());
            }

            return paths;
        }
    }
}