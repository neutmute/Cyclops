using System.Reflection;
using System.Runtime.CompilerServices;

[assembly: AssemblyTitle("Cyclops")]
[assembly: AssemblyDescription("Fluent wrappings around DAAB 6 with advanced failure logging")]

#if DEBUG
[assembly: AssemblyProduct("Cyclops (Debug)")]
[assembly: AssemblyConfiguration("Debug")]
#else
[assembly: AssemblyProduct("Cyclops (Release)")]
[assembly: AssemblyConfiguration("Release")]
#endif

[assembly: InternalsVisibleTo("Cyclops.Tests")]