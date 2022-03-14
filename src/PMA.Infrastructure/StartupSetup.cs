// <copyright file="StartupSetup.cs" company="Andrey Pospelov">
//     Copyright 2017-2021 Andrey Pospelov. All rights reserved.
// </copyright>

using Autofac;
using Autofac.Extras.DynamicProxy;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using PMA.Application.MappingProfiles;
using PMA.Domain.Interfaces.Interceptors;
using PMA.Infrastructure.DbContexts;
using PMA.Infrastructure.Interceptors;
using PMA.Infrastructure.MappingProfiles;
using System.Reflection;

namespace PMA.Infrastructure
{
    /// <summary>
    /// The extension for startup DI configuration.
    /// </summary>
    public static class StartupSetup
    {
        /// <summary>
        /// Adds infrastructure services.
        /// </summary>
        /// <param name="builder">This DI container builder.</param>
        /// <param name="dataSource">The data source.</param>
        public static void AddInfrastructureServices(this ContainerBuilder builder, string dataSource)
        {
            // Interceptors
            builder.RegisterType<LoggerInterceptor>().As<ILoggerInterceptor>().SingleInstance();

            // Mapper
            builder.Register(c =>
                    new Mapper(new MapperConfiguration(x =>
                    {
                        x.AddProfile(new EntityMappingProfile());
                        x.AddProfile(new ModelMappingProfile());
                        x.AddProfile(new ViewModelMappingProfile(c));
                    })))
                .As<IMapperBase>()
                .SingleInstance();

            // Logger
            builder.RegisterType<LoggerFactory>().As<ILoggerFactory>().SingleInstance();
            builder.RegisterGeneric(typeof(Logger<>)).As(typeof(ILogger<>)).SingleInstance();

            // Mediator
            builder.RegisterType<Mediator>().As<IMediator>().SingleInstance();
            builder.Register<ServiceFactory>(context =>
            {
                var c = context.Resolve<IComponentContext>();
                return t => c.Resolve(t);
            });

            // DbContexts
            builder.RegisterType<SqLiteDbContext>().WithParameter("dataSource", dataSource)
                .InterceptedBy(typeof(ILoggerInterceptor))
                .SingleInstance();

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
