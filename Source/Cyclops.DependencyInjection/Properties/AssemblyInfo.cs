using System.Reflection;
using System.Runtime.CompilerServices;


[assembly: AssemblyTitle("Cyclops.DependencyInjection")]
[assembly: AssemblyDescription("Cyclops DependencyInjection")]
[assembly: AssemblyConfiguration("")]

#if DEBUG
[assembly: AssemblyProduct("Cyclops.DependencyInjection (Debug)")]
#else
[assembly: AssemblyProduct("Cyclops.DependencyInjection (Release)")]
#endif

// [AS] remove this once we have a DependencyInjection test project 
[assembly: InternalsVisibleTo("PetStore.IntegrationTest")]