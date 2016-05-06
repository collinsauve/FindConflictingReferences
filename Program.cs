using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace FindConflictingReferences
{
    // Based on https://gist.github.com/brianlow/1553265 and https://gist.github.com/WaffleSouffle/bcc3eaebaa7a7cadcab6
    public class Program
    {
        private static void OutputConflicts(TextWriter output, IEnumerable<IGrouping<string, Reference>> references)
        {
            if (!references.Any())
                return;

            var maxNameLength = references.SelectMany(t => t).Max(t => t.Assembly.Name.Length);

            foreach (var group in references)
            {
                output.WriteLine("Possible conflicts for {0}:", group.Key);
                foreach (var reference in group)
                {
                    output.WriteLine("    {0} references {1}",
                        reference.Assembly.Name.PadRight(maxNameLength),
                        reference.ReferencedAssembly.FullName);
                }
                output.WriteLine();
            }
        }

        public static void Main(string[] args)
        {
            foreach (var path in GetPathsFromArgs(args))
            {
                var references = AssemblyReference.GetReferencedAssembliesWithMultipleVersions(path);
                OutputConflicts(Console.Out, references);
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
                paths.Add(Directory.GetCurrentDirectory());
            }

            return paths;
        }
    }
}