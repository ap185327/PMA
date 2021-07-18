// <copyright file="MorphEntry.cs" company="Andrey Pospelov">
//     Copyright 2017-2021 Andrey Pospelov. All rights reserved.
// </copyright>

using PMA.Domain.Enums;

namespace PMA.Domain.Models
{
    /// <summary>
    /// Class for the morphological entry model.
    /// </summary>
    public class MorphEntry
    {
        /// <summary>
        /// Gets or sets a MorphEntry ID.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets a MorphEntry entry.
        /// </summary>
        public string Entry { get; set; }

        /// <summary>
        /// Gets or sets MorphEntry parameters.
        /// </summary>
        public byte[] Parameters { get; set; }

        /// <summary>
        /// Gets or sets a morphological base.
        /// </summary>
        public MorphBase Base { get; set; }

        /// <summary>
        /// Gets or sets a left MorphEntry.
        /// </summary>
        public MorphEntry Left { get; set; }

        /// <summary>
        /// Gets or sets a right MorphEntry.
        /// </summary>
        public MorphEntry Right { get; set; }

        /// <summary>
        /// Gets or sets whether a MorphEntry is virtual (doesn't exist in the live language) or not.
        /// </summary>
        public bool IsVirtual { get; set; }

        /// <summary>
        /// Gets or sets a MorphEntry source.
        /// </summary>
        public MorphEntrySource Source { get; set; }
    }
}
