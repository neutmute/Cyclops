using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using Autofac;
using Autofac.Configuration;
using Autofac.Core;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Common.Logging;

namespace Cyclops.ExtensionMethods
{
    public static class ContainerBuilderExtensions
    {
        private static readonly ILog Log = LogManager.GetLogger("ContainerBuilderExtensions");

        /// <summary>
        /// Helper to auto wire an assembly with Cyclops repositories
        /// </summary>
        /// <typeparam name="T">Base type contained in the Data assembly</typeparam>
        public static void RegisterCyclopsRepositories<T>(this ContainerBuilder builder, string serviceName)
        {
            var dataAssembly = typeof(T).Assembly;
            List<Type> repositories = GetImplementationsOf<T>(dataAssembly);
            
            builder.RegisterModule(new ConfigurationSettingsReader());

            foreach (Type repository in repositories)
            {
                if (repository.IsSubclassOf(typeof(CyclopsRepository)))
                {
                    var resolvedParameter = new ResolvedParameter(
                        (p, c) => p.ParameterType == typeof(Database)
                        , (p, c) => c.ResolveNamed<Database>(serviceName));

                    Log.Trace(m=>m("Registering {0}", repository));
                    builder
                        .RegisterType(repository)
                        .WithParameter(resolvedParameter)
                        .AsImplementedInterfaces()
                        .AsSelf();
                }
            }
        }

        private static List<Type> GetAllTypes(Assembly assembly)
        {
            // BH: This code had some funky all in one linq which failed on Assembly.GetTypes() in a solution
            // Needed to unroll and add some try catch handling to allow service to start
            List<Type> allAssemblyTypes = new List<Type>();
            try
            {
                List<Type> thisAssemblyTypes = assembly.GetTypes().ToList();
                allAssemblyTypes.AddRange(thisAssemblyTypes);
            }
            catch (ReflectionTypeLoadException ex)
            {
                StringBuilder sb = new StringBuilder();
                foreach (Exception exSub in ex.LoaderExceptions)
                {
                    sb.AppendLine(exSub.Message);
                    if (exSub is FileNotFoundException)
                    {
                        FileNotFoundException exFileNotFound = exSub as FileNotFoundException;
                        if (!string.IsNullOrEmpty(exFileNotFound.FusionLog))
                        {
                            sb.AppendLine("Fusion Log:");
                            sb.AppendLine(exFileNotFound.FusionLog);
                        }
                    }
                    sb.AppendLine();
                }
                string errorMessage = sb.ToString();
                Log.Warn(errorMessage, ex);
            }
            catch (Exception e)
            {
                Log.Warn("Failed to load types for " + assembly.GetName().Name, e);
            }

            return allAssemblyTypes;
        }

        private static List<Type> GetImplementationsOf<T>(Assembly fromAssembly)
        {
            List<Type> allTypes = GetAllTypes(fromAssembly);
            return GetImplementationsOf<T>(allTypes);
        }

        private static List<Type> GetImplementationsOf<T>(List<Type> types)
        {
            List<Type> typeList = (
                                      from t in types
                                          .Where(type => typeof(T).IsAssignableFrom(type) && !type.IsInterface && !type.IsAbstract)
                                      select t
                                  ).ToList();

            return typeList;
        }
    }
}
