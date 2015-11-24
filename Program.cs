using System.Collections.Generic;

namespace FindConflictingReferences
{
    // Based on https://gist.github.com/brianlow/1553265 and https://gist.github.com/WaffleSouffle/bcc3eaebaa7a7cadcab6
    public class Program
    {
        private static void FindConflicts(System.IO.TextWriter output, string path)
        {
            var assemblies = FindConflictingReferenceFunctions.GetAllAssemblies(path);

            var references = FindConflictingReferenceFunctions.GetReferencesFromAllAssemblies(assemblies);

            var groupsOfConflicts = FindConflictingReferenceFunctions.FindReferencesWithTheSameShortNameButDiffererntFullNames(references);

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
            var paths = new List<string>();
            for (var argIter = 0; argIter < args.Length; ++argIter)
            {
                var arg = args[argIter];
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

            foreach (var path in paths)
            {
                FindConflicts(System.Console.Out, path);
            }
        }
    }
}