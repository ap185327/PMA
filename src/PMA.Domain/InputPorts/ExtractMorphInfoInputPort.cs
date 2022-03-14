// <copyright file="ExtractMorphInfoInputPort.cs" company="Andrey Pospelov">
//     Copyright 2017-2021 Andrey Pospelov. All rights reserved.
// </copyright>

namespace PMA.Domain.InputPorts
{
    /// <summary>
    /// The extract morphological information input port class.
    /// </summary>
    public class ExtractMorphInfoInputPort
    {
        /// <summary>
        /// Gets or initializes whether to use the visibility of morphological parameters.
        /// </summary>
        public bool UseVisibility { get; init; }

        /// <summary>
        /// Gets or initializes a collection of morphological parameters.
        /// </summary>
        public byte[] Parameters { get; init; }
    }
}
