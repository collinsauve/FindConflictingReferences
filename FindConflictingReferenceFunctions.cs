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
            return GetFiles(path, "*.dll", "*.exe")
                .Select(TryLoadAssembly)
                .ToList();
        }

        private static IEnumerable<FileInfo> GetFiles(string path, params string[] extensions)
        {
            return extensions.SelectMany(ext => new DirectoryInfo(path).GetFiles(ext, SearchOption.AllDirectories));
        }

        private static Assembly TryLoadAssembly(FileInfo file)
        {
            try
            {
                return Assembly.LoadFile(file.FullName);
            }
            catch (BadImageFormatException)
            {
                return null;
            }
        }
    }
}