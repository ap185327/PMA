// <copyright file="SolutionError.cs" company="Andrey Pospelov">
//     Copyright 2017-2021 Andrey Pospelov. All rights reserved.
// </copyright>

namespace PMA.Domain.Enums
{
    /// <summary>
    /// Enumerates different types of Solution errors.
    /// </summary>
    public enum SolutionError
    {
        /// <summary>
        /// No errors. This is Solution content error.
        /// </summary>
        Success = 0,

        /// <summary>
        /// No LeftEntry WordForm matches in morphological dictionary. This is Solution content error.
        /// </summary>
        NoLeftMatches,

        /// <summary>
        /// No RightEntry WordForm matches in morphological dictionary. This is Solution content error.
        /// </summary>
        NoRightMatches,

        /// <summary>
        /// No rule matches in morphological rules. This is Solution content error.
        /// </summary>
        NoRuleMatches,

        /// <summary>
        /// No rule matches in sandhi rules. This is Solution content error.
        /// </summary>
        NoSandhiMatches,

        /// <summary>
        /// No matches in morphological combinations. This is Solution content error.
        /// </summary>
        NoMorphCombinationMatches,

        /// <summary>
        /// Not found LeftEntry WordForm by parameters. This is Solution content error.
        /// </summary>
        NotFoundLeftByParameters,

        /// <summary>
        /// Not found RightEntry WordForm by parameters. This is Solution content error.
        /// </summary>
        NotFoundRightByParameters,

        /// <summary>
        /// Maximum analysis depth is exceeded. This is Solution error.
        /// </summary>
        DepthIsExceeded
    }
}
