// <copyright file="RuleType.cs" company="Andrey Pospelov">
//     Copyright 2017-2021 Andrey Pospelov. All rights reserved.
// </copyright>

namespace PMA.Domain.Enums
{
    /// <summary>
    /// Enumerates different types of rules.
    /// </summary>
    public enum RuleType
    {
        /// <summary>
        /// Undefined type.
        /// </summary>
        Undefined = 0,

        /// <summary>
        /// The morphological rule.
        /// </summary>
        Morphological,

        /// <summary>
        /// The sandhi rule.
        /// </summary>
        Sandhi
    }
}
