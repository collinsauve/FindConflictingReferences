using System.Reflection;

namespace FindConflictingReferences
{
    public class Reference
    {
        public AssemblyName Assembly { get; set; }
        public AssemblyName ReferencedAssembly { get; set; }
    }
}