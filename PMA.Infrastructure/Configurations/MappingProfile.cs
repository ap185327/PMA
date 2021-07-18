// <copyright file="MappingProfile.cs" company="Andrey Pospelov">
//     Copyright 2017-2021 Andrey Pospelov. All rights reserved.
// </copyright>

using AutoMapper;
using PMA.Domain.Enums;
using PMA.Domain.Models;
using PMA.Infrastructure.Entities;
using PMA.Infrastructure.Factories;
using PMA.Utils.Extensions;
using System;
using System.Data;
using System.Text.RegularExpressions;

namespace PMA.Infrastructure.Configurations
{
    /// <summary>
    /// Provides a named configuration for maps. Naming conventions become scoped per profile.
    /// </summary>
    public class MappingProfile : Profile
    {
        /// <summary>
        /// Initializes the new instance of <see cref="MappingProfile"/> class.
        /// </summary>
        public MappingProfile()
        {
            // MorphCombinationEntity --> byte[]
            CreateMap<MorphCombinationEntity, byte[]>()
                .ConvertUsing(src => ParameterFactory.Create(src));

            // MorphEntryEntity --> MorphEntry
            CreateMap<MorphEntryEntity, MorphEntry>().ConvertUsing(src => MorphEntryFactory.Create(src));

            // MorphEntry --> MorphEntryEntity
            CreateMap<MorphEntry, MorphEntryEntity>()
                .ForMember(dest => dest.Parameter0, act => act.MapFrom(src => src.Parameters[0]))
                .ForMember(dest => dest.Parameter1, act => act.MapFrom(src => src.Parameters[1]))
                .ForMember(dest => dest.Parameter2, act => act.MapFrom(src => src.Parameters[2]))
                .ForMember(dest => dest.Parameter3, act => act.MapFrom(src => src.Parameters[3]))
                .ForMember(dest => dest.Parameter4, act => act.MapFrom(src => src.Parameters[4]))
                .ForMember(dest => dest.Parameter5, act => act.MapFrom(src => src.Parameters[5]))
                .ForMember(dest => dest.Parameter6, act => act.MapFrom(src => src.Parameters[6]))
                .ForMember(dest => dest.Parameter7, act => act.MapFrom(src => src.Parameters[7]))
                .ForMember(dest => dest.Parameter8, act => act.MapFrom(src => src.Parameters[8]))
                .ForMember(dest => dest.Parameter9, act => act.MapFrom(src => src.Parameters[9]))
                .ForMember(dest => dest.Parameter10, act => act.MapFrom(src => src.Parameters[10]))
                .ForMember(dest => dest.Parameter11, act => act.MapFrom(src => src.Parameters[11]))
                .ForMember(dest => dest.Parameter12, act => act.MapFrom(src => src.Parameters[12]))
                .ForMember(dest => dest.Parameter13, act => act.MapFrom(src => src.Parameters[13]))
                .ForMember(dest => dest.Parameter14, act => act.MapFrom(src => src.Parameters[14]))
                .ForMember(dest => dest.Parameter15, act => act.MapFrom(src => src.Parameters[15]))
                .ForMember(dest => dest.Parameter16, act => act.MapFrom(src => src.Parameters[16]))
                .ForMember(dest => dest.Parameter17, act => act.MapFrom(src => src.Parameters[17]))
                .ForMember(dest => dest.Parameter18, act => act.MapFrom(src => src.Parameters[18]))
                .ForMember(dest => dest.Parameter19, act => act.MapFrom(src => src.Parameters[19]))
                .ForMember(dest => dest.Parameter20, act => act.MapFrom(src => src.Parameters[20]))
                .ForMember(dest => dest.LeftEntryId, act => act.MapFrom(src => src.Left != null ? src.Left.Id : 0))
                .ForMember(dest => dest.RightEntryId, act => act.MapFrom(src => src.Right != null ? src.Right.Id : 0));

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

            // DataRow --> MorphRuleEntity
            CreateMap<DataRow, MorphRuleEntity>().ConvertUsing(row => new MorphRuleEntity
            {
                RuleId = Convert.ToInt32(row["Id"]),
                IsCollapsed = Convert.ToInt32(Convert.ToBoolean(row["IsCollapsed"])),
                Label = (string)row["Label"],
                SandhiGroupId = Convert.ToInt32(row["Sandhi"]),
                Entry = (string)row["Entry"],
                Parameter0 = Convert.ToByte(row["0"]),
                Parameter1 = Convert.ToByte(row["1"]),
                Parameter2 = Convert.ToByte(row["2"]),
                Parameter3 = Convert.ToByte(row["3"]),
                Parameter4 = Convert.ToByte(row["4"]),
                Parameter5 = Convert.ToByte(row["5"]),
                Parameter6 = Convert.ToByte(row["6"]),
                Parameter7 = Convert.ToByte(row["7"]),
                Parameter8 = Convert.ToByte(row["8"]),
                Parameter9 = Convert.ToByte(row["9"]),
                Parameter10 = Convert.ToByte(row["10"]),
                Parameter11 = Convert.ToByte(row["11"]),
                Parameter12 = Convert.ToByte(row["12"]),
                Parameter13 = Convert.ToByte(row["13"]),
                Parameter14 = Convert.ToByte(row["14"]),
                Parameter15 = Convert.ToByte(row["15"]),
                Parameter16 = Convert.ToByte(row["16"]),
                Parameter17 = Convert.ToByte(row["17"]),
                Parameter18 = Convert.ToByte(row["18"]),
                Parameter19 = Convert.ToByte(row["19"]),
                Parameter20 = Convert.ToByte(row["20"]),
                Base = (MorphBase)Enum.Parse(typeof(MorphBase), ((string)row["Base"]).FirstCharToUpper()),
                LeftType = (MorphRuleType)Enum.Parse(typeof(MorphRuleType), ((string)row["LeftType"]).FirstCharToUpper()),
                LeftLabel = (string)row["LeftLabel"],
                LeftEntry = (string)row["LeftEntry"],
                LeftParameter0 = Convert.ToByte(row["Left0"]),
                LeftParameter1 = Convert.ToByte(row["Left1"]),
                LeftParameter2 = Convert.ToByte(row["Left2"]),
                LeftParameter3 = Convert.ToByte(row["Left3"]),
                LeftParameter4 = Convert.ToByte(row["Left4"]),
                LeftParameter5 = Convert.ToByte(row["Left5"]),
                LeftParameter6 = Convert.ToByte(row["Left6"]),
                LeftParameter7 = Convert.ToByte(row["Left7"]),
                LeftParameter8 = Convert.ToByte(row["Left8"]),
                LeftParameter9 = Convert.ToByte(row["Left9"]),
                LeftParameter10 = Convert.ToByte(row["Left10"]),
                LeftParameter11 = Convert.ToByte(row["Left11"]),
                LeftParameter12 = Convert.ToByte(row["Left12"]),
                LeftParameter13 = Convert.ToByte(row["Left13"]),
                LeftParameter14 = Convert.ToByte(row["Left14"]),
                LeftParameter15 = Convert.ToByte(row["Left15"]),
                LeftParameter16 = Convert.ToByte(row["Left16"]),
                LeftParameter17 = Convert.ToByte(row["Left17"]),
                LeftParameter18 = Convert.ToByte(row["Left18"]),
                LeftParameter19 = Convert.ToByte(row["Left19"]),
                LeftParameter20 = Convert.ToByte(row["Left20"]),
                RightType = (MorphRuleType)Enum.Parse(typeof(MorphRuleType), ((string)row["RightType"]).FirstCharToUpper()),
                RightLabel = (string)row["RightLabel"],
                RightEntry = (string)row["RightEntry"],
                RightParameter0 = Convert.ToByte(row["Right0"]),
                RightParameter1 = Convert.ToByte(row["Right1"]),
                RightParameter2 = Convert.ToByte(row["Right2"]),
                RightParameter3 = Convert.ToByte(row["Right3"]),
                RightParameter4 = Convert.ToByte(row["Right4"]),
                RightParameter5 = Convert.ToByte(row["Right5"]),
                RightParameter6 = Convert.ToByte(row["Right6"]),
                RightParameter7 = Convert.ToByte(row["Right7"]),
                RightParameter8 = Convert.ToByte(row["Right8"]),
                RightParameter9 = Convert.ToByte(row["Right9"]),
                RightParameter10 = Convert.ToByte(row["Right10"]),
                RightParameter11 = Convert.ToByte(row["Right11"]),
                RightParameter12 = Convert.ToByte(row["Right12"]),
                RightParameter13 = Convert.ToByte(row["Right13"]),
                RightParameter14 = Convert.ToByte(row["Right14"]),
                RightParameter15 = Convert.ToByte(row["Right15"]),
                RightParameter16 = Convert.ToByte(row["Right16"]),
                RightParameter17 = Convert.ToByte(row["Right17"]),
                RightParameter18 = Convert.ToByte(row["Right18"]),
                RightParameter19 = Convert.ToByte(row["Right19"]),
                RightParameter20 = Convert.ToByte(row["Right20"]),
                Rating = 0.99,
                Description = (string)row["Description"]
            });

            // DataRow --> SandhiGroupEntity
            CreateMap<DataRow, SandhiGroupEntity>().ConvertUsing(row => new SandhiGroupEntity
            {
                Id = Convert.ToInt32(row["Id"]),
                Entry = (string)row["Entry"]
            });

            // DataRow --> SandhiRuleEntity
            CreateMap<DataRow, SandhiRuleEntity>().ConvertUsing(row => new SandhiRuleEntity
            {
                RuleId = Convert.ToInt32(row["Id"]),
                GroupId = Convert.ToInt32(row["Group"]),
                Regex = (string)row["Regex"],
                Description = (string)row["Description"]
            });

            // DataRow --> SandhiResultEntity
            CreateMap<DataRow, SandhiResultEntity>().ConvertUsing(row => new SandhiResultEntity
            {
                RuleId = Convert.ToInt32(row["Id"]),
                RegexResult = (string)row["RegexResult"]
            });

            // DataRow --> MorphCombinationEntity
            CreateMap<DataRow, MorphCombinationEntity>().ConvertUsing(row => new MorphCombinationEntity
            {
                Parameter0 = Convert.ToByte(row["Language"]),
                Parameter1 = Convert.ToByte(row["Part"]),
                Parameter2 = Convert.ToByte(row["PoS1"]),
                Parameter3 = Convert.ToByte(row["PoS2"]),
                Parameter4 = Convert.ToByte(row["PoS3"]),
                Parameter5 = Convert.ToByte(row["Tense"]),
                Parameter6 = Convert.ToByte(row["Voice"]),
                Parameter7 = Convert.ToByte(row["Gender"]),
                Parameter8 = Convert.ToByte(row["Mode"]),
                Parameter9 = Convert.ToByte(row["Person"]),
                Parameter10 = Convert.ToByte(row["Number"]),
                Parameter11 = Convert.ToByte(row["Case"]),
                Parameter12 = Convert.ToByte(row["IsIrregular"]),
                Parameter13 = Convert.ToByte(row["IsName"]),
                Parameter14 = Convert.ToByte(row["IsNegative"]),
                Parameter15 = Convert.ToByte(row["WithAugment"]),
                Parameter16 = Convert.ToByte(row["Formation"]),
                Parameter17 = Convert.ToByte(row["Parent"]),
                Parameter18 = Convert.ToByte(row["Type1"]),
                Parameter19 = Convert.ToByte(row["Type2"]),
                Parameter20 = Convert.ToByte(row["Type3"])
            });

            // DataRow --> MorphEntry
            CreateMap<DataRow, MorphEntry>().ConvertUsing(row => MorphEntryFactory.Create(row));
        }
    }
}
