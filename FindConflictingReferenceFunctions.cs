using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace FindConflictingReferences
{
    public class FindConflictingReferenceFunctions
    {
        public static IEnumerable<IGrouping<string, Reference>> GetReferencedAssembliesWithMultipleVersions(string path)
        {
            var assemblies = GetAllAssemblies(path);
            var references = GetReferencesFromAllAssemblies(assemblies);

            return references
                .GroupBy(r => r.ReferencedAssembly.Name)
                .Where(r => r.Select(t => t.ReferencedAssembly.FullName).Distinct().Count() > 1);
        }

        private static IEnumerable<Reference> GetReferencesFromAllAssemblies(IEnumerable<Assembly> assemblies)
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

        private static IEnumerable<Assembly> GetAllAssemblies(string path)
        {
            return GetFileNames(path, "*.dll", "*.exe")
                .Select(TryLoadAssembly)
                .Where(asm => asm != null);
        }

        private static IEnumerable<string> GetFileNames(string path, params string[] extensions)
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