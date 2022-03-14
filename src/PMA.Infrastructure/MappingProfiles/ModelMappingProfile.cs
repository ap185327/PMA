// <copyright file="ModelMappingProfile.cs" company="Andrey Pospelov">
//     Copyright 2017-2021 Andrey Pospelov. All rights reserved.
// </copyright>

using AutoMapper;
using PMA.Domain.Models;
using PMA.Infrastructure.Entities;
using PMA.Infrastructure.Factories;
using System;
using System.Data;
using System.Text.RegularExpressions;

namespace PMA.Infrastructure.MappingProfiles
{
    /// <summary>
    /// Provides a named configuration for maps. Naming conventions become scoped per profile.
    /// </summary>
    public class ModelMappingProfile : Profile
    {
        /// <summary>
        /// Initializes the new instance of <see cref="ModelMappingProfile"/> class.
        /// </summary>
        public ModelMappingProfile()
        {
            // MorphCombinationEntity --> byte[]
            CreateMap<MorphCombinationEntity, byte[]>()
                .ConvertUsing(src => ParameterFactory.Create(src));

            // MorphEntryEntity --> MorphEntry
            CreateMap<MorphEntryEntity, MorphEntry>().ConvertUsing(src => MorphEntryFactory.Create(src));

            // MorphRuleEntity --> MorphRule
            CreateMap<MorphRuleEntity, MorphRule>().ConvertUsing(src => new MorphRule
            {
                EntityId = src.Id,
                Id = src.RuleId,
                IsCollapsed = Convert.ToBoolean(src.IsCollapsed),
                Label = src.Label,
                NeedToCheck = !src.Label.StartsWith("#"),
                SandhiGroup = src.SandhiGroupId,
                Entry = src.Entry,
                Parameters = new[]
                {
                    src.Parameter0,
                    src.Parameter1,
                    src.Parameter2,
                    src.Parameter3,
                    src.Parameter4,
                    src.Parameter5,
                    src.Parameter6,
                    src.Parameter7,
                    src.Parameter8,
                    src.Parameter9,
                    src.Parameter10,
                    src.Parameter11,
                    src.Parameter12,
                    src.Parameter13,
                    src.Parameter14,
                    src.Parameter15,
                    src.Parameter16,
                    src.Parameter17,
                    src.Parameter18,
                    src.Parameter19,
                    src.Parameter20
                },
                Base = src.Base,
                LeftType = src.LeftType,
                LeftLabel = src.LeftLabel,
                Left = src.LeftEntry,
                LeftParameters = new[]
                {
                    src.LeftParameter0,
                    src.LeftParameter1,
                    src.LeftParameter2,
                    src.LeftParameter3,
                    src.LeftParameter4,
                    src.LeftParameter5,
                    src.LeftParameter6,
                    src.LeftParameter7,
                    src.LeftParameter8,
                    src.LeftParameter9,
                    src.LeftParameter10,
                    src.LeftParameter11,
                    src.LeftParameter12,
                    src.LeftParameter13,
                    src.LeftParameter14,
                    src.LeftParameter15,
                    src.LeftParameter16,
                    src.LeftParameter17,
                    src.LeftParameter18,
                    src.LeftParameter19,
                    src.LeftParameter20
                },
                RightType = src.RightType,
                RightLabel = src.RightLabel,
                Right = src.RightEntry,
                RightParameters = new[]
                {
                    src.RightParameter0,
                    src.RightParameter1,
                    src.RightParameter2,
                    src.RightParameter3,
                    src.RightParameter4,
                    src.RightParameter5,
                    src.RightParameter6,
                    src.RightParameter7,
                    src.RightParameter8,
                    src.RightParameter9,
                    src.RightParameter10,
                    src.RightParameter11,
                    src.RightParameter12,
                    src.RightParameter13,
                    src.RightParameter14,
                    src.RightParameter15,
                    src.RightParameter16,
                    src.RightParameter17,
                    src.RightParameter18,
                    src.RightParameter19,
                    src.RightParameter20
                },
                Rating = src.Rating,
                Description = src.Description
            });

            // SandhiRuleEntity --> SandhiRule
            CreateMap<SandhiRuleEntity, SandhiRule>()
                .ForMember(dest => dest.Id, act => act.MapFrom(src => src.RuleId))
                .ForMember(dest => dest.Regex, act => act.MapFrom(src => new Regex(src.Regex)));

            // DataRow --> MorphEntry
            CreateMap<DataRow, MorphEntry>().ConvertUsing(row => MorphEntryFactory.Create(row));
        }
    }
}
