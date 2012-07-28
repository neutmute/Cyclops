using System.Reflection;
using System.Runtime.CompilerServices;


[assembly: AssemblyTitle("Cyclops")]
[assembly: AssemblyDescription("Cyclops Core")]
[assembly: AssemblyConfiguration("")]

#if DEBUG
[assembly: AssemblyProduct("Cyclops (Debug)")]
#else
[assembly: AssemblyProduct("Cyclops (Release)")]
#endif


// [AS] remove this once we have a core test project 
[assembly: InternalsVisibleTo("PetStore.IntegrationTest")]