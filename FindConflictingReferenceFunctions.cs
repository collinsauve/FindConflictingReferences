using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace FindConflictingReferences
{
    public class FindConflictingReferenceFunctions
    {
        public static IEnumerable<IGrouping<string, Reference>> FindReferencesWithTheSameShortNameButDiffererntFullNames(IEnumerable<Reference> references)
        {
            return from reference in references
                group reference by reference.ReferencedAssembly.Name
                into referenceGroup
                where referenceGroup.ToList().Select(reference => reference.ReferencedAssembly.FullName).Distinct().Count() > 1
                select referenceGroup;
        }

        public static IEnumerable<Reference> GetReferencesFromAllAssemblies(IEnumerable<Assembly> assemblies)
        {
            return assemblies.SelectMany(GetReferencedAssemblies);
        }

        private static IEnumerable<Reference> GetReferencedAssemblies(Assembly asm)
        {
            return asm.GetReferencedAssemblies().Select(refAsm => new Reference
            {
                Assembly = asm.GetName(),
                ReferencedAssembly = refAsm
            });
        }

        public static IEnumerable<Assembly> GetAllAssemblies(string path)
        {
            return GetFiles(path, "*.dll", "*.exe")
                .Select(TryLoadAssembly)
                .Where(asm => asm != null);
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