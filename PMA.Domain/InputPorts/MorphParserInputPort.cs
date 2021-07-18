// <copyright file="MorphParserInputPort.cs" company="Andrey Pospelov">
//     Copyright 2017-2021 Andrey Pospelov. All rights reserved.
// </copyright>

using PMA.Domain.Enums;
using PMA.Domain.Models;

namespace PMA.Domain.InputPorts
{
    /// <summary>
    /// The morphological parser input port class.
    /// </summary>
    public class MorphParserInputPort
    {
        /// <summary>
        /// Gets or sets a morphological entry.
        /// </summary>
        public MorphEntry MorphEntry { get; init; }

        /// <summary>
        /// Gets or sets a wordform.
        /// </summary>
        public WordForm WordForm { get; set; }

        /// <summary>
        /// Gets or sets a morphological parsing type.
        /// </summary>
        public MorphParsingType ParsingType { get; init; }

        /// <summary>
        /// Gets or sets a maximum depth of wordform analysis according to morphological rules.
        /// </summary>
        public int MaxDepthLevel { get; init; }
    }
}
