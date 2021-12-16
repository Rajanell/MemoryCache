using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Rajanell.MemoryCache.Model;
using Rajanell.MemoryCache.Services;
using System;
using System.IO;

namespace Rajanell.Test
{
    public static class ServicesProvider
    {
        public static IServiceProvider GetServiceProvider()
        {
            IServiceCollection services = new ServiceCollection();

            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
            IConfiguration config = builder.Build();

            services.Configure<AppSettings>(config.GetSection("AppSettings"));
            services.AddMemoryCache();
            services.AddScoped<IMemoryCacheService, MemoryCacheService>();

            var containerBuilder = new ContainerBuilder();
            containerBuilder.Populate(services);

            var container = containerBuilder.Build();
            var serviceProvider = new AutofacServiceProvider(container);

            return serviceProvider;
        }
    }
}
