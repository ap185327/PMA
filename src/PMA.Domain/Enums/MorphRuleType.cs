// <copyright file="MorphRuleType.cs" company="Andrey Pospelov">
//     Copyright 2017-2021 Andrey Pospelov. All rights reserved.
// </copyright>

namespace PMA.Domain.Enums
{
    /// <summary>
    /// Enumerates different types of the morphological parameter inheritance.
    /// </summary>
    public enum MorphRuleType
    {
        /// <summary>
        /// No wordform.
        /// </summary>
        None = 0,

        /// <summary>
        /// Create a new wordform.
        /// </summary>
        New = 1,

        /// <summary>
        /// Create a wordform from the parent wordform.
        /// </summary>
        Copy = 2
    }
}
