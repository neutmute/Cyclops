using System.Reflection;
using System.Runtime.CompilerServices;


[assembly: AssemblyTitle("Sprocker.Core")]
[assembly: AssemblyDescription("Sprocker Core")]
[assembly: AssemblyConfiguration("")]

#if DEBUG
[assembly: AssemblyProduct("Sprocker.Core (Debug)")]
#else
[assembly: AssemblyProduct("Sprocker.Core (Release)")]
#endif


// [AS] remove this once we have a core test project 
[assembly: InternalsVisibleTo("PetStore.IntegrationTest")]