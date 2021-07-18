// <copyright file="ExcelTableStructureConstants.cs" company="Andrey Pospelov">
//     Copyright 2017-2021 Andrey Pospelov. All rights reserved.
// </copyright>

using PMA.Infrastructure.Models;
using System;

namespace PMA.Infrastructure.Constants
{
    /// <summary>
    /// The excel table structure constant class.
    /// </summary>
    internal static class ExcelTableStructureConstants
    {
        /// <summary>
        /// The structure for the MorphCombinations excel table.
        /// </summary>
        public static readonly ExcelTableStructure MorphCombinations = new()
        {
            AllColumns = new[] { "Id", "Language", "Part", "PoS1", "PoS2", "PoS3", "Tense", "Voice", "Gender", "Mode", "Person", "Number", "Case", "IsIrregular", "IsName", "IsNegative", "WithAugment", "Formation", "Parent", "Type1", "Type2", "Type3" },
            UniqueColumns = new[] { "Id" },
            NonEmptyColumns = new[] { "Id", "Language", "Part", "PoS1", "PoS2", "PoS3", "Tense", "Voice", "Gender", "Mode", "Person", "Number", "Case", "IsIrregular", "IsName", "IsNegative", "WithAugment", "Formation", "Parent", "Type1", "Type2", "Type3" },
            IntegerColumns = new[] { "Id" },
            DoubleColumns = Array.Empty<string>(),
            BooleanColumns = Array.Empty<string>(),
            RegExpColumns = Array.Empty<string>()
        };

        /// <summary>
        /// The structure for the SandhiGroups excel table.
        /// </summary>
        public static readonly ExcelTableStructure SandhiGroups = new()
        {
            AllColumns = new[] { "Id", "Entry" },
            UniqueColumns = new[] { "Id", "Entry" },
            NonEmptyColumns = new[] { "Id", "Entry" },
            IntegerColumns = new[] { "Id" },
            DoubleColumns = Array.Empty<string>(),
            BooleanColumns = Array.Empty<string>(),
            RegExpColumns = Array.Empty<string>()
        };

        /// <summary>
        /// The structure for the SandhiRules excel table.
        /// </summary>
        public static readonly ExcelTableStructure SandhiRules = new()
        {
            AllColumns = new[] { "Id", "Direction", "Group", "RegexLeft", "RegexBody", "RegexRight", "RegexResult", "Description", "IsActive" },
            UniqueColumns = new[] { "Id" },
            NonEmptyColumns = new[] { "Id", "Direction", "Group", "Description", "IsActive" },
            IntegerColumns = new[] { "Id" },
            DoubleColumns = Array.Empty<string>(),
            BooleanColumns = new[] { "IsActive" },
            RegExpColumns = Array.Empty<string>()
        };

        /// <summary>
        /// The structure for the SandhiVars excel table.
        /// </summary>
        public static readonly ExcelTableStructure SandhiVars = new()
        {
            AllColumns = new[] { "Id", "Name", "Regex", "RegexResult", "Description" },
            UniqueColumns = new[] { "Id", "Name" },
            NonEmptyColumns = new[] { "Id", "Name", "Regex" },
            IntegerColumns = new[] { "Id" },
            DoubleColumns = Array.Empty<string>(),
            BooleanColumns = Array.Empty<string>(),
            RegExpColumns = new[] { "Regex" }
        };

        /// <summary>
        /// The structure for the MorphRules excel table.
        /// </summary>
        public static readonly ExcelTableStructure MorphRules = new()
        {
            AllColumns = new[] { "Id", "Group", "IsCollapsed", "Label", "Sandhi", "Entry", "Info", "Base", "LeftType", "LeftLabel", "LeftEntry", "LeftInfo", "RightType", "RightLabel", "RightEntry", "RightInfo", "Description", "IsActive" },
            UniqueColumns = new[] { "Id" },
            NonEmptyColumns = new[] { "Id", "IsCollapsed", "Sandhi", "Base", "LeftType", "RightType", "Description", "IsActive" },
            IntegerColumns = new[] { "Id" },
            DoubleColumns = Array.Empty<string>(),
            BooleanColumns = new[] { "IsCollapsed", "IsActive" },
            RegExpColumns = new[] { "Entry" }
        };

        /// <summary>
        /// The structure for the MorphEntries excel table.
        /// </summary>
        public static readonly ExcelTableStructure MorphEntries = new ExcelTableStructure
        {
            AllColumns = new[] { "Entry", "Language", "Part", "PoS1", "PoS2", "PoS3", "Tense", "Voice", "Gender", "Mode", "Person", "Number", "Case", "IsIrregular", "IsName", "IsNegative", "WithAugment", "Formation", "Parent", "Type1", "Type2", "Type3", "Base", "LeftEntry", "RightEntry", "IsVirtual" },
            UniqueColumns = Array.Empty<string>(),
            NonEmptyColumns = new[] { "Entry", "Language", "Part", "PoS1", "PoS2", "PoS3", "Tense", "Voice", "Gender", "Mode", "Person", "Number", "Case", "IsIrregular", "IsName", "IsNegative", "WithAugment", "Formation", "Parent", "Type1", "Type2", "Type3", "Base", "IsVirtual" },
            IntegerColumns = Array.Empty<string>(),
            DoubleColumns = Array.Empty<string>(),
            BooleanColumns = new[] { "IsVirtual" },
            RegExpColumns = Array.Empty<string>()
        };
    }
}
