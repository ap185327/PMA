// <copyright file="ITreeNodeInteractor.cs" company="Andrey Pospelov">
//     Copyright 2017-2021 Andrey Pospelov. All rights reserved.
// </copyright>

using PMA.Domain.DataContracts;
using PMA.Domain.Interfaces.Interactors.Base;

namespace PMA.Domain.Interfaces.Interactors.Primary
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ITreeNodeInteractor"/> interfacing class.
    /// </summary>
    public interface ITreeNodeInteractor : IInteractor
    {
        /// <summary>
        /// Gets a chronological layer for the first tree node.
        /// </summary>
        /// <returns>The operation result.</returns>
        OperationResult<uint> GetLayerForFirstNode();

        /// <summary>
        /// Extracts a morphological information from the collection of morphological parameters.
        /// </summary>
        /// <param name="parameters">The collection of morphological parameters.</param>
        /// <returns>The operation result.</returns>
        OperationResult<string> ExtractMorphInfoFromMorphParameters(byte[] parameters);
    }
}
