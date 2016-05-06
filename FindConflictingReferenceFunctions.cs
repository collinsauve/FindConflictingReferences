using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace FindConflictingReferences
{
    public class FindConflictingReferenceFunctions
    {
        public static IEnumerable<IGrouping<string, Reference>> FindReferencesWithTheSameShortNameButDiffererntFullNames(List<Reference> references)
        {
            return from reference in references
                group reference by reference.ReferencedAssembly.Name
                into referenceGroup
                where referenceGroup.ToList().Select(reference => reference.ReferencedAssembly.FullName).Distinct().Count() > 1
                select referenceGroup;
        }

        public static List<Reference> GetReferencesFromAllAssemblies(List<Assembly> assemblies)
        {
            var references = new List<Reference>();
            foreach (var assembly in assemblies)
            {
                if (assembly == null)
                    continue;
                foreach (var referencedAssembly in assembly.GetReferencedAssemblies())
                {
                    references.Add(new Reference
                    {
                        Assembly = assembly.GetName(),
                        ReferencedAssembly = referencedAssembly
                    });
                }
            }
            return references;
        }

        public static List<Assembly> GetAllAssemblies(string path)
        {
            var files = new List<FileInfo>();
            var directoryToSearch = new DirectoryInfo(path);
            files.AddRange(directoryToSearch.GetFiles("*.dll", SearchOption.AllDirectories));
            files.AddRange(directoryToSearch.GetFiles("*.exe", SearchOption.AllDirectories));
            return files.ConvertAll(file =>
            {
                try
                {
                    Assembly asm = Assembly.LoadFile(file.FullName);
                    return asm;
                }
                catch (BadImageFormatException)
                {
                    return null;
                }

            });
        }
    }
}