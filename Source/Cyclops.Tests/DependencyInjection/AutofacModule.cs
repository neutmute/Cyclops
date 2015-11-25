using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using Cyclops.ExtensionMethods;
using Common.Logging;
using PetStore.Infrastructure;

namespace Cyclops.Tests.DependencyInjection
{
    public class AutofacModule : Module
    {
        private static readonly ILog Log = LogManager.GetLogger< AutofacModule>();

        /// <summary>
        /// Override to add registrations to the container.
        /// </summary>
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);

            Log.Trace("Registering Cyclops.DependencyInjection module");
            builder.RegisterCyclopsRepositories<PetstoreRepository>("PetstoreDatabase");
        }
    }
}
