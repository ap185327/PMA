// <copyright file="DeleteMorphEntryOutputPort.cs" company="Andrey Pospelov">
//     Copyright 2017-2021 Andrey Pospelov. All rights reserved.
// </copyright>

using PMA.Domain.Models;
using System.Collections.Generic;

namespace PMA.Domain.OutputPorts
{
    /// <summary>
    /// The delete morphological entry output port class.
    /// </summary>
    public class DeleteMorphEntryOutputPort
    {
        /// <summary>
        /// Gets or sets a morphological entry ID.
        /// </summary>
        public int Id { get; init; }

        /// <summary>
        /// Gets or sets a result of the delete operation.
        /// </summary>
        public bool IsDeleted { get; init; }

        /// <summary>
        /// Gets or sets a collection of parent morphological entries.
        /// </summary>
        public IList<MorphEntry> MorphEntries { get; init; }
    }
}
