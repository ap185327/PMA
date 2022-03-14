// <copyright file="StartupSetup.cs" company="Andrey Pospelov">
//     Copyright 2017-2021 Andrey Pospelov. All rights reserved.
// </copyright>

using Autofac;
using Autofac.Extras.DynamicProxy;
using MediatR;
using Microsoft.Toolkit.Mvvm.Messaging;
using PMA.Application.UseCases.Primary;
using PMA.Application.UseCases.Secondary;
using PMA.Application.ViewModels;
using PMA.Application.ViewModels.Controls;
using PMA.Domain.Interfaces.Interceptors;
using PMA.Domain.Interfaces.UseCases.Primary;
using PMA.Domain.Interfaces.UseCases.Secondary;
using PMA.Domain.Interfaces.ViewModels;
using PMA.Domain.Interfaces.ViewModels.Controls;
using PMA.Domain.Notifications;
using System.Reflection;

namespace PMA.Application
{
    /// <summary>
    /// The extension for startup configuration.
    /// </summary>
    public static class StartupSetup
    {
        /// <summary>
        /// Adds application services.
        /// </summary>
        /// <param name="builder">This DI container builder.</param>
        public static void AddApplicationServices(this ContainerBuilder builder)
        {
            // Messenger
            builder.RegisterType<StrongReferenceMessenger>().As<IMessenger>()
                .SingleInstance();

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
            builder.RegisterType<GetEntryIdControlViewModelsUseCase>().As<IGetEntryIdControlViewModelsUseCase>()
                .SingleInstance();
            builder.RegisterType<GetEntryIdViewModelUseCase>().As<IGetEntryIdViewModelUseCase>()
                .EnableInterfaceInterceptors()
                .InterceptedBy(typeof(ILoggerInterceptor))
                .SingleInstance();
            builder.RegisterType<GetMorphCategoryControlViewModelsUseCase>().As<IGetMorphCategoryControlViewModelsUseCase>()
                .EnableInterfaceInterceptors()
                .InterceptedBy(typeof(ILoggerInterceptor))
                .SingleInstance();
            builder.RegisterType<GetLayerForFirstNodeUseCase>().As<IGetLayerForFirstNodeUseCase>()
                .EnableInterfaceInterceptors()
                .InterceptedBy(typeof(ILoggerInterceptor))
                .SingleInstance();
            builder.RegisterType<GetMorphPropertyControlViewModelsUseCase>().As<IGetMorphPropertyControlViewModelsUseCase>()
                .EnableInterfaceInterceptors()
                .InterceptedBy(typeof(ILoggerInterceptor))
                .SingleInstance();
            builder.RegisterType<GetMorphRuleInfoItemViewModelsUseCase>().As<IGetMorphRuleInfoItemViewModelsUseCase>()
                .EnableInterfaceInterceptors()
                .InterceptedBy(typeof(ILoggerInterceptor))
                .SingleInstance();
            builder.RegisterType<GetMorphParameterByIdUseCase>().As<IGetMorphParameterByIdUseCase>()
                .EnableInterfaceInterceptors()
                .InterceptedBy(typeof(ILoggerInterceptor))
                .SingleInstance();
            builder.RegisterType<GetSandhiRuleInfoItemViewModelsUseCase>().As<IGetSandhiRuleInfoItemViewModelsUseCase>()
                .EnableInterfaceInterceptors()
                .InterceptedBy(typeof(ILoggerInterceptor))
                .SingleInstance();
            builder.RegisterType<GetSimilarMorphEntryIdsUseCase>().As<IGetSimilarMorphEntryIdsUseCase>()
                .EnableInterfaceInterceptors()
                .InterceptedBy(typeof(ILoggerInterceptor))
                .SingleInstance();
            builder.RegisterType<GetSolutionTreeNodesUseCase>().As<IGetSolutionTreeNodesUseCase>()
                .SingleInstance();
            builder.RegisterType<GetTermEntriesByIdsUseCase>().As<IGetTermEntriesByIdsUseCase>()
                .EnableInterfaceInterceptors()
                .InterceptedBy(typeof(ILoggerInterceptor))
                .SingleInstance();
            builder.RegisterType<GetWordFormTreeNodeUseCase>().As<IGetWordFormTreeNodeUseCase>()
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
            builder.RegisterType<ClearCacheUseCase>().As<IClearCacheUseCase>()
                .SingleInstance();
            builder.RegisterType<CollapseSolutionUseCase>().As<ICollapseSolutionUseCase>()
                .SingleInstance();
            builder.RegisterType<ParseMorphEntryUseCase>().As<IParseMorphEntryUseCase>()
                .SingleInstance();
            builder.RegisterType<RemoveDuplicateUseCase>().As<IRemoveDuplicateUseCase>()
                .SingleInstance();
            builder.RegisterType<RemoveSolutionWithExcessiveDepthUseCase>().As<IRemoveSolutionWithExcessiveDepthUseCase>()
                .SingleInstance();
            builder.RegisterType<RemoveUnsuitableDerivativeSolutionUseCase>().As<IRemoveUnsuitableDerivativeSolutionUseCase>()
                .SingleInstance();
            builder.RegisterType<RemoveUnsuitableSolutionUseCase>().As<IRemoveUnsuitableSolutionUseCase>()
                .SingleInstance();
            builder.RegisterType<SortSolutionUseCase>().As<ISortSolutionUseCase>()
                .SingleInstance();
            builder.RegisterType<UpdateSolutionUseCase>().As<IUpdateSolutionUseCase>()
                .SingleInstance();
            builder.RegisterType<ValidateSolutionUseCase>().As<IValidateSolutionUseCase>()
                .SingleInstance();

            // Primary & Secondary Interactors
            builder.RegisterAssemblyTypes(dataAccess).Where(x => x.Name.EndsWith("Interactor")).AsImplementedInterfaces()
                .SingleInstance();

            // ViewModels
            builder.RegisterType<GetEntryIdViewModel>().As<IGetEntryIdViewModel>()
                .EnableInterfaceInterceptors()
                .InterceptedBy(typeof(ILoggerInterceptor))
                .InstancePerDependency();
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
                .As<INotificationHandler<MorphParserNotification>>()
                .EnableInterfaceInterceptors()
                .InterceptedBy(typeof(ILoggerInterceptor))
                .SingleInstance();
            builder.RegisterType<MorphRuleInfoViewModel>().As<IMorphRuleInfoViewModel>()
                .EnableInterfaceInterceptors()
                .InterceptedBy(typeof(ILoggerInterceptor))
                .SingleInstance();
            builder.RegisterType<MorphSolutionViewModel>().As<IMorphSolutionViewModel>()
                .As<INotificationHandler<MorphParserNotification>>()
                .EnableInterfaceInterceptors()
                .InterceptedBy(typeof(ILoggerInterceptor))
                .SingleInstance();
            builder.RegisterType<OptionViewModel>().As<IOptionViewModel>()
                .SingleInstance();
            builder.RegisterType<UpdateDbViewModel>().As<IUpdateDbViewModel>()
                .As<INotificationHandler<UpdateDbNotification>>()
                .EnableInterfaceInterceptors()
                .InterceptedBy(typeof(ILoggerInterceptor))
                .SingleInstance();

            // ViewModel Controls
            builder.RegisterType<GetEntryIdControlViewModel>().As<IGetEntryIdControlViewModel>()
                .InstancePerDependency();
            builder.RegisterType<MorphCategoryControlViewModel>().As<IMorphCategoryControlViewModel>()
                .InstancePerDependency();
            builder.RegisterType<MorphPropertyControlViewModel>().As<IMorphPropertyControlViewModel>()
                .InstancePerDependency();
            builder.RegisterType<RuleInfoItemViewModel>().As<IRuleInfoItemViewModel>()
                .InstancePerDependency();
            builder.RegisterType<SolutionTreeNodeViewModel>().As<ISolutionTreeNodeViewModel>()
                .InstancePerDependency();
            builder.RegisterType<WordFormTreeNodeViewModel>().As<IWordFormTreeNodeViewModel>()
                .InstancePerDependency();
        }
    }
}
