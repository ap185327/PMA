// <copyright file="UpdateMorphEntryOutputPort.cs" company="Andrey Pospelov">
//     Copyright 2017-2021 Andrey Pospelov. All rights reserved.
// </copyright>

using System.Collections.Generic;

namespace PMA.Domain.OutputPorts
{
    /// <summary>
    /// The update morphological entry output port class.
    /// </summary>
    public class UpdateMorphEntryOutputPort
    {
        /// <summary>
        /// Gets or sets a result of the update operation.
        /// </summary>
        public string Error { get; init; }

        /// <summary>
        /// Gets or sets a collection of updated morphological entry IDs.
        /// </summary>
        public IList<int> Ids { get; init; }
    }
}
