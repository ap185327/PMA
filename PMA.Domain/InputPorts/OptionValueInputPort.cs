// <copyright file="OptionValueInputPort.cs" company="Andrey Pospelov">
//     Copyright 2017-2021 Andrey Pospelov. All rights reserved.
// </copyright>

using System.Collections.Generic;

namespace PMA.Domain.InputPorts
{
    /// <summary>
    /// The option value input port class.
    /// </summary>
    public class OptionValueInputPort
    {
        /// <summary>
        /// Gets or sets a option allows you to select the way how the wordform will be analyzed: False – only successful solutions sorted by rating; True – all solutions, including unsuccessful ones.
        /// </summary>
        public bool DebugMode { get; init; }

        /// <summary>
        /// Gets or sets a collection of available terms.
        /// </summary>
        public IEnumerable<string> AvailableTerms { get; init; }

        /// <summary>
        /// Gets or sets a collection of shown terms.
        /// </summary>
        public IEnumerable<string> ShownTerms { get; init; }

        /// <summary>
        /// Gets or sets a frequency rating ratio.
        /// </summary>
        public double FreqRatingRatio { get; init; }
    }
}
