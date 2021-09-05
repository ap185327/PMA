// <copyright file="ViewModelMappingProfile.cs" company="Andrey Pospelov">
//     Copyright 2017-2021 Andrey Pospelov. All rights reserved.
// </copyright>

using Autofac;
using AutoMapper;
using PMA.Domain.Enums;
using PMA.Domain.InputPorts;
using PMA.Domain.Interfaces.ViewModels;
using PMA.Domain.Interfaces.ViewModels.Controls;
using PMA.Domain.Models;
using System.Collections.Generic;

namespace PMA.Application.MappingProfiles
{
    /// <summary>
    /// Provides a named configuration for view model maps. Naming conventions become scoped per profile.
    /// </summary>
    public sealed class ViewModelMappingProfile : Profile
    {
        /// <summary>
        /// Initializes the new instance of <see cref="ViewModelMappingProfile"/> class.
        /// </summary>
        /// <param name="context">The context in which a service can be accessed or a component's dependencies resolved.</param>
        public ViewModelMappingProfile(IComponentContext context)
        {
            var ctx = context.Resolve<IComponentContext>();

            // MorphEntry --> IGetEntryIdViewModel
            CreateMap<MorphEntry, IGetEntryIdViewModel>()
                .ConstructUsing(x => ctx.Resolve<IGetEntryIdViewModel>(new NamedParameter("morphEntry", x)))
                .ForMember(dest => dest.IsActive, act => act.MapFrom(src => true));

            // MorphEntry --> IGetEntryIdControlViewModel
            CreateMap<MorphEntry, IGetEntryIdControlViewModel>()
                .ConstructUsing(x => ctx.Resolve<IGetEntryIdControlViewModel>(new NamedParameter("morphEntry", x)))
                .ForMember(dest => dest.IsActive, act => act.MapFrom(src => true));

            // GetWordFormTreeNodeInputPort --> IWordFormTreeNodeViewModel
            CreateMap<GetWordFormTreeNodeInputPort, IWordFormTreeNodeViewModel>()
                .ConstructUsing(x => ctx.Resolve<IWordFormTreeNodeViewModel>(new NamedParameter("parent", x.Parent), new NamedParameter("wordForm", x.WordForm)))
                .ForMember(dest => dest.IsActive, act => act.MapFrom(src => true));

            // SolutionTreeNodeViewModelGroup --> ISolutionTreeNodeViewModel
            CreateMap<SolutionTreeNodeViewModelGroup, ISolutionTreeNodeViewModel>()
                .ConstructUsing(x => ctx.Resolve<ISolutionTreeNodeViewModel>(new NamedParameter("parent", x.Parent), new NamedParameter("solution", x.Solution)))
                .ForMember(dest => dest.IsActive, act => act.MapFrom(src => true));

            // int --> IMorphPropertyControlViewModel
            CreateMap<int, IMorphPropertyControlViewModel>()
                .ConstructUsing(x => ctx.Resolve<IMorphPropertyControlViewModel>(new NamedParameter("index", x)))
                .ForMember(dest => dest.IsActive, act => act.MapFrom(src => true));

            // IList<IMorphPropertyControlViewModel> --> IMorphPropertyControlViewModel
            CreateMap<IList<IMorphPropertyControlViewModel>, IMorphCategoryControlViewModel>()
                .ConstructUsing(x => ctx.Resolve<IMorphCategoryControlViewModel>(new NamedParameter("properties", x)))
                .ForMember(dest => dest.IsActive, act => act.MapFrom(src => true));

            // MorphRule --> IRuleInfoItemViewModel
            CreateMap<MorphRule, IRuleInfoItemViewModel>()
                .ConstructUsing(x => ctx.Resolve<IRuleInfoItemViewModel>(new NamedParameter("type", RuleType.Morphological), new NamedParameter("id", x.Id), new NamedParameter("description", x.Description)))
                .ForMember(dest => dest.IsActive, act => act.MapFrom(src => true));

            // SandhiRule --> IRuleInfoItemViewModel
            CreateMap<SandhiRule, IRuleInfoItemViewModel>()
                .ConstructUsing(x => ctx.Resolve<IRuleInfoItemViewModel>(new NamedParameter("type", RuleType.Sandhi), new NamedParameter("id", x.Id), new NamedParameter("description", x.Description)))
                .ForMember(dest => dest.IsActive, act => act.MapFrom(src => true));
        }
    }
}
