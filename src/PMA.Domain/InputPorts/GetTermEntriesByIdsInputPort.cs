// <copyright file="GetTermEntriesByIdsInputPort.cs" company="Andrey Pospelov">
//     Copyright 2017-2021 Andrey Pospelov. All rights reserved.
// </copyright>

using System.Collections.Generic;

namespace PMA.Domain.InputPorts
{
    /// <summary>
    /// The get term entries by IDs input port class.
    /// </summary>
    public class GetTermEntriesByIdsInputPort
    {
        /// <summary>
        /// Gets or sets a collection of term IDs.
        /// </summary>
        public IList<byte> TermIds { get; init; }

        /// <summary>
        /// Gets or sets whether to use alternative property entry.
        /// </summary>
        public bool UseAltPropertyEntry { get; init; }
    }
}
