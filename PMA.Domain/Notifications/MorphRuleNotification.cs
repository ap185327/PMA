// <copyright file="MorphRuleNotification.cs" company="Andrey Pospelov">
//     Copyright 2017-2021 Andrey Pospelov. All rights reserved.
// </copyright>

using MediatR;
using PMA.Domain.Models;
using System.Collections.Generic;

namespace PMA.Domain.Notifications
{
    /// <summary>
    /// The morphological rule notification class.
    /// </summary>
    public class MorphRuleNotification : INotification
    {
        /// <summary>
        /// Gets or sets a collection of morphological rules.
        /// </summary>
        public IList<MorphRule> MorphRules { get; init; }

        /// <summary>
        /// Gets or sets a collection of sandhi matches.
        /// </summary>
        public IList<SandhiMatch> SandhiMatches { get; init; }
    }
}
