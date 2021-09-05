// <copyright file="MorphEntryMessage.cs" company="Andrey Pospelov">
//     Copyright 2017-2021 Andrey Pospelov. All rights reserved.
// </copyright>

using PMA.Domain.Models;

namespace PMA.Domain.Messages
{
    /// <summary>
    /// The morphological entry message class.
    /// </summary>
    public class MorphEntryMessage
    {
        /// <summary>
        /// Gets or initializes a morphological entry.
        /// </summary>
        public MorphEntry MorphEntry { get; init; }
    }
}
