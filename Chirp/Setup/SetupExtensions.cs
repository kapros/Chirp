using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Chirp.Setup
{
    public static class SetupExtensions
    {
        public static void RunAllInstallers(this IServiceCollection services, IConfiguration configuration)
        {
            var installers = typeof(Startup).Assembly
                .ExportedTypes
                .Where(x => typeof(IInstaller).IsAssignableFrom(x) && !x.IsInterface && !x.IsAbstract)
                .Select(Activator.CreateInstance)
                .Cast<IInstaller>()
                .ToList();
            installers.ForEach(x => x.InstallServices(services, configuration));
        }
    }
}
