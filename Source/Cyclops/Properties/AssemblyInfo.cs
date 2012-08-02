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

[assembly: InternalsVisibleTo("PetStore.IntegrationTest")]