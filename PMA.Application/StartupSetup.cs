// <copyright file="StartupSetup.cs" company="Andrey Pospelov">
//     Copyright 2017-2021 Andrey Pospelov. All rights reserved.
// </copyright>

using Autofac;
using Autofac.Extras.DynamicProxy;
using MediatR;
using PMA.Application.UseCases.Primary;
using PMA.Application.UseCases.Secondary;
using PMA.Application.ViewModels;
using PMA.Domain.Interfaces.Interceptors;
using PMA.Domain.Interfaces.UseCases.Primary;
using PMA.Domain.Interfaces.UseCases.Secondary;
using PMA.Domain.Interfaces.ViewModels;
using PMA.Domain.Models;
using PMA.Domain.Notifications;
using PMA.Domain.Requests;
using System;
using System.Reflection;
using System.Threading.Tasks;

namespace PMA.Application
{
    /// <summary>
    /// The extension for startup DI configuration.
    /// </summary>
    public static class StartupSetup
    {
        /// <summary>
        /// Registers application types.
        /// </summary>
        /// <param name="builder">This DI container.</param>
        public static void RegisterApplicationTypes(this ContainerBuilder builder)
        {
            // Configurations
            builder.RegisterType<ParallelOptions>().SingleInstance();
            builder.RegisterType<Mediator>().As<IMediator>().SingleInstance();
            builder.Register<ServiceFactory>(context =>
            {
                var c = context.Resolve<IComponentContext>();
                return t => c.Resolve(t);
            });

            var dataAccess = Assembly.GetExecutingAssembly();

            // Managers
            builder.RegisterAssemblyTypes(dataAccess).Where(x => x.Name.EndsWith("Manager")).AsImplementedInterfaces()
                .SingleInstance();

            // Primary UseCases
            builder.RegisterType<ExtractMorphInfoFromMorphParametersUseCase>().As<IExtractMorphInfoFromMorphParametersUseCase>()
                .SingleInstance();
            builder.RegisterType<GetCurrentOptionValuesUseCase>().As<IGetCurrentOptionValuesUseCase>()
                .EnableInterfaceInterceptors()
                .InterceptedBy(typeof(ILoggerInterceptor))
                .SingleInstance();
            builder.RegisterType<GetLayerForFirstNodeUseCase>().As<IGetLayerForFirstNodeUseCase>()
                .EnableInterfaceInterceptors()
                .InterceptedBy(typeof(ILoggerInterceptor))
                .SingleInstance();
            builder.RegisterType<GetMorphEntriesByMorphEntryUseCase>().As<IGetMorphEntriesByMorphEntryUseCase>()
                .EnableInterfaceInterceptors()
                .InterceptedBy(typeof(ILoggerInterceptor))
                .SingleInstance();
            builder.RegisterType<GetMorphParameterByIdUseCase>().As<IGetMorphParameterByIdUseCase>()
                .EnableInterfaceInterceptors()
                .InterceptedBy(typeof(ILoggerInterceptor))
                .SingleInstance();
            builder.RegisterType<GetSimilarMorphEntryIdsUseCase>().As<IGetSimilarMorphEntryIdsUseCase>()
                .EnableInterfaceInterceptors()
                .InterceptedBy(typeof(ILoggerInterceptor))
                .SingleInstance();
            builder.RegisterType<GetTermEntriesByIdsUseCase>().As<IGetTermEntriesByIdsUseCase>()
                .EnableInterfaceInterceptors()
                .InterceptedBy(typeof(ILoggerInterceptor))
                .SingleInstance();
            builder.RegisterType<SaveOptionValuesUseCase>().As<ISaveOptionValuesUseCase>()
                .EnableInterfaceInterceptors()
                .InterceptedBy(typeof(ILoggerInterceptor))
                .SingleInstance();
            builder.RegisterType<StartDbUpdatingUseCase>().As<IStartDbUpdatingUseCase>()
                .EnableInterfaceInterceptors()
                .InterceptedBy(typeof(ILoggerInterceptor))
                .SingleInstance();
            builder.RegisterType<StartImportMorphEntriesUseCase>().As<IStartImportMorphEntriesUseCase>()
                .EnableInterfaceInterceptors()
                .InterceptedBy(typeof(ILoggerInterceptor))
                .SingleInstance();
            builder.RegisterType<StartMorphAnalysisUseCase>().As<IStartMorphAnalysisUseCase>()
                .EnableInterfaceInterceptors()
                .InterceptedBy(typeof(ILoggerInterceptor))
                .SingleInstance();
            builder.RegisterType<StopDbUpdatingUseCase>().As<IStopDbUpdatingUseCase>()
                .As<INotificationHandler<CancellationTokenResourceNotification>>()
                .EnableInterfaceInterceptors()
                .InterceptedBy(typeof(ILoggerInterceptor))
                .SingleInstance();
            builder.RegisterType<StopImportMorphEntriesUseCase>().As<IStopImportMorphEntriesUseCase>()
                .As<INotificationHandler<CancellationTokenResourceNotification>>()
                .EnableInterfaceInterceptors()
                .InterceptedBy(typeof(ILoggerInterceptor))
                .SingleInstance();
            builder.RegisterType<StopMorphAnalysisUseCase>().As<IStopMorphAnalysisUseCase>()
                .As<INotificationHandler<CancellationTokenResourceNotification>>()
                .EnableInterfaceInterceptors()
                .InterceptedBy(typeof(ILoggerInterceptor))
                .SingleInstance();
            builder.RegisterType<TryToAddMorphEntryUseCase>().As<ITryToAddMorphEntryUseCase>()
                .EnableInterfaceInterceptors()
                .InterceptedBy(typeof(ILoggerInterceptor))
                .SingleInstance();
            builder.RegisterType<TryToDeleteMorphEntriesUseCase>().As<ITryToDeleteMorphEntriesUseCase>()
                .EnableInterfaceInterceptors()
                .InterceptedBy(typeof(ILoggerInterceptor))
                .SingleInstance();
            builder.RegisterType<TryToDeleteMorphEntryUseCase>().As<ITryToDeleteMorphEntryUseCase>()
                .EnableInterfaceInterceptors()
                .InterceptedBy(typeof(ILoggerInterceptor))
                .SingleInstance();
            builder.RegisterType<TryToUpdateMorphEntryUseCase>().As<ITryToUpdateMorphEntryUseCase>()
                .EnableInterfaceInterceptors()
                .InterceptedBy(typeof(ILoggerInterceptor))
                .SingleInstance();
            builder.RegisterType<UpdateAllMorphPropertyUseCase>().As<IUpdateAllMorphPropertyUseCase>()
                .EnableInterfaceInterceptors()
                .InterceptedBy(typeof(ILoggerInterceptor))
                .SingleInstance();

            // Secondary UseCases
            builder.RegisterType<ClearCacheUseCase>().As<IClearCacheUseCase>().SingleInstance();
            builder.RegisterType<CollapseSolutionUseCase>().As<ICollapseSolutionUseCase>().SingleInstance();
            builder.RegisterType<ParseMorphEntryUseCase>().As<IParseMorphEntryUseCase>().SingleInstance();
            builder.RegisterType<RemoveDuplicateUseCase>().As<IRemoveDuplicateUseCase>().SingleInstance();
            builder.RegisterType<RemoveSolutionWithExcessiveDepthUseCase>().As<IRemoveSolutionWithExcessiveDepthUseCase>().SingleInstance();
            builder.RegisterType<RemoveUnsuitableDerivativeSolutionUseCase>().As<IRemoveUnsuitableDerivativeSolutionUseCase>().SingleInstance();
            builder.RegisterType<RemoveUnsuitableSolutionUseCase>().As<IRemoveUnsuitableSolutionUseCase>().SingleInstance();
            builder.RegisterType<SortSolutionUseCase>().As<ISortSolutionUseCase>().SingleInstance();
            builder.RegisterType<UpdateSolutionUseCase>().As<IUpdateSolutionUseCase>().SingleInstance();
            builder.RegisterType<ValidateSolutionUseCase>().As<IValidateSolutionUseCase>().SingleInstance();

            // Primary & Secondary Interactors
            builder.RegisterAssemblyTypes(dataAccess).Where(x => x.Name.EndsWith("Interactor")).AsImplementedInterfaces()
                .SingleInstance();

            // ViewModels
            builder.RegisterType<GetEntryIdViewModel>().As<IGetEntryIdViewModel>()
                .EnableInterfaceInterceptors()
                .InterceptedBy(typeof(ILoggerInterceptor))
                .SingleInstance();
            builder.RegisterType<ImportMorphEntryViewModel>().As<IImportMorphEntryViewModel>()
                .As<INotificationHandler<ImportMorphEntryNotification>>()
                .EnableInterfaceInterceptors()
                .InterceptedBy(typeof(ILoggerInterceptor))
                .SingleInstance();
            builder.RegisterType<MainViewModel>().As<IMainViewModel>()
                .As<INotificationHandler<MorphParserNotification>>()
                .As<INotificationHandler<DepthLevelNotification>>()
                .EnableInterfaceInterceptors()
                .InterceptedBy(typeof(ILoggerInterceptor))
                .SingleInstance();
            builder.RegisterType<MorphPropertyViewModel>().As<IMorphPropertyViewModel>()
                .As<INotificationHandler<SettingChangeNotification>>()
                .As<INotificationHandler<MorphParserNotification>>()
                .As<INotificationHandler<MorphEntryNotification>>()
                .As<IRequestHandler<GetMorphEntryRequest, MorphEntry>>()
                .EnableInterfaceInterceptors()
                .InterceptedBy(typeof(ILoggerInterceptor))
                .SingleInstance();
            builder.RegisterType<MorphRuleInfoViewModel>().As<IMorphRuleInfoViewModel>()
                .As<INotificationHandler<MorphRuleNotification>>()
                .EnableInterfaceInterceptors()
                .InterceptedBy(typeof(ILoggerInterceptor))
                .SingleInstance();
            builder.RegisterType<MorphSolutionViewModel>().As<IMorphSolutionViewModel>()
                .As<INotificationHandler<MorphParserNotification>>()
                .EnableInterfaceInterceptors()
                .InterceptedBy(typeof(ILoggerInterceptor))
                .SingleInstance();
            builder.RegisterType<OptionViewModel>().As<IOptionViewModel>().SingleInstance();
            builder.RegisterType<UpdateDbViewModel>().As<IUpdateDbViewModel>()
                .As<INotificationHandler<UpdateDbNotification>>()
                .EnableInterfaceInterceptors()
                .InterceptedBy(typeof(ILoggerInterceptor))
                .SingleInstance();
        }
    }
}
