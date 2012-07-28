using System.Reflection;
using System.Runtime.CompilerServices;


[assembly: AssemblyTitle("Sprocker.DependencyInjection")]
[assembly: AssemblyDescription("Sprocker DependencyInjection")]
[assembly: AssemblyConfiguration("")]

#if DEBUG
[assembly: AssemblyProduct("Sprocker.DependencyInjection (Debug)")]
#else
[assembly: AssemblyProduct("Sprocker.DependencyInjection (Release)")]
#endif

// [AS] remove this once we have a DependencyInjection test project 
[assembly: InternalsVisibleTo("PetStore.IntegrationTest")]