// <copyright file="MorphEntryError.cs" company="Andrey Pospelov">
//     Copyright 2017-2021 Andrey Pospelov. All rights reserved.
// </copyright>

namespace PMA.Domain.Enums
{
    /// <summary>
    /// Enumerates different types of MorphEntry errors.
    /// </summary>
    public enum MorphEntryError
    {
        /// <summary>
        /// No errors.
        /// </summary>
        Success = 0,

        /// <summary>
        /// No similar entries found to update.
        /// </summary>
        NoSimilarEntriesFound,

        /// <summary>
        /// The ID doesn't exist in the collection.
        /// </summary>
        IdDoesNotExist,

        /// <summary>
        /// The left ID doesn't exist in the collection.
        /// </summary>
        LeftIdDoesNotExist,

        /// <summary>
        /// The right ID doesn't exist in the collection.
        /// </summary>
        RightIdDoesNotExist,

        /// <summary>
        /// Found more than one left in the collection.
        /// </summary>
        FoundMoreThanOneLeft,

        /// <summary>
        /// Found more than one right in the collection.
        /// </summary>
        FoundMoreThanOneRight,

        /// <summary>
        /// The left entry doesn't exist in the collection.
        /// </summary>
        LeftEntryDoesNotExist,

        /// <summary>
        /// The right entry doesn't exist in the collection.
        /// </summary>
        RightEntryDoesNotExist,

        /// <summary>
        /// The entry is empty.
        /// </summary>
        EntryIsEmpty,

        /// <summary>
        /// The morph. parameters is empty.
        /// </summary>
        ParametersAreEmpty,

        /// <summary>
        /// The left morph. entry doesn't match morph base.
        /// </summary>
        LeftDoesNotMatchBase,

        /// <summary>
        /// The right morph. entry doesn't match morph base.
        /// </summary>
        RightDoesNotMatchBase,

        /// <summary>
        /// There are no morphological combination matches.
        /// </summary>
        NoMorphCombinationMatches,

        /// <summary>
        /// The entry already exists in the collection.
        /// </summary>
        EntryAlreadyExists
    }
}
