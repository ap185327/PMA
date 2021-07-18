// <copyright file="StartupSetup.cs" company="Andrey Pospelov">
//     Copyright 2017-2021 Andrey Pospelov. All rights reserved.
// </copyright>

using Autofac;
using Autofac.Extras.DynamicProxy;
using AutoMapper;
using Microsoft.Extensions.Logging;
using PMA.Domain.Interfaces.Interceptors;
using PMA.Infrastructure.Configurations;
using PMA.Infrastructure.DbContexts;
using PMA.Infrastructure.Interceptors;
using System.Reflection;

namespace PMA.Infrastructure
{
    /// <summary>
    /// The extension for startup DI configuration.
    /// </summary>
    public static class StartupSetup
    {
        /// <summary>
        /// Registers infrastructure types.
        /// </summary>
        /// <param name="builder">This DI container.</param>
        /// <param name="dataSource">The data source.</param>
        public static void RegisterInfrastructureTypes(this ContainerBuilder builder, string dataSource)
        {
            // Configurations
            builder.RegisterType<LoggerInterceptor>().As<ILoggerInterceptor>().SingleInstance();
            builder.RegisterType<LoggerFactory>().As<ILoggerFactory>().SingleInstance();
            builder.RegisterGeneric(typeof(Logger<>)).As(typeof(ILogger<>)).SingleInstance();
            builder.RegisterType<SqLiteDbContext>().WithParameter("dataSource", dataSource)
                .InterceptedBy(typeof(ILoggerInterceptor))
                .SingleInstance();
            builder.RegisterType<Mapper>().As<IMapperBase>().WithParameter("configurationProvider", new MapperConfiguration(x => x.AddProfile(new MappingProfile()))).SingleInstance();

            var dataAccess = Assembly.GetExecutingAssembly();

            // Services
            builder.RegisterAssemblyTypes(dataAccess).Where(x => x.Name.EndsWith("Service")).AsImplementedInterfaces()
                .SingleInstance();

            // Providers
            builder.RegisterAssemblyTypes(dataAccess).Where(x => x.Name.EndsWith("Provider")).AsImplementedInterfaces()
                .SingleInstance();

            // Loaders
            builder.RegisterAssemblyTypes(dataAccess).Where(x => x.Name.EndsWith("Loader")).AsImplementedInterfaces()
                .EnableInterfaceInterceptors()
                .InterceptedBy(typeof(ILoggerInterceptor))
                .SingleInstance();

            // Locators
            builder.RegisterAssemblyTypes(dataAccess).Where(x => x.Name.EndsWith("Locator")).AsImplementedInterfaces()
                .SingleInstance();
        }
    }
}
