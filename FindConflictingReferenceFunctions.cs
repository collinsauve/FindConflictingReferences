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
                .Where(asm => asm != null)
                .ToList();
        }

        private static IEnumerable<string> GetFiles(string path, params string[] extensions)
        {
            return extensions.SelectMany(ext => Directory.GetFiles(path, ext, SearchOption.AllDirectories));
        }

        private static Assembly TryLoadAssembly(string filename)
        {
            try
            {
                return Assembly.LoadFile(filename);
            }
            catch (BadImageFormatException)
            {
                return null;
            }
        }
    }
}