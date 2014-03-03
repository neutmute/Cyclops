using System.Reflection;
using System.Runtime.CompilerServices;


[assembly: AssemblyTitle("Cyclops.DependencyInjection")]
[assembly: AssemblyDescription("Autofac container builder extensions for Cyclops repositories")]

#if DEBUG
[assembly: AssemblyProduct("Cyclops.DependencyInjection (Debug)")]
[assembly: AssemblyConfiguration("Debug")]
#else
[assembly: AssemblyProduct("Cyclops.DependencyInjection (Release)")]
[assembly: AssemblyConfiguration("Release")]
#endif

// [AS] remove this once we have a DependencyInjection test project 
[assembly: InternalsVisibleTo("PetStore.IntegrationTest")]