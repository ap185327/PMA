// <copyright file="IImportMorphEntryInteractor.cs" company="Andrey Pospelov">
//     Copyright 2017-2021 Andrey Pospelov. All rights reserved.
// </copyright>

using PMA.Domain.DataContracts;
using PMA.Domain.InputPorts;
using PMA.Domain.Interfaces.Interactors.Base;

namespace PMA.Domain.Interfaces.Interactors.Primary
{
    /// <summary>
    /// Initializes a new instance of the <see cref="IImportMorphEntryInteractor"/> interfacing class.
    /// </summary>
    public interface IImportMorphEntryInteractor : IInteractor
    {
        /// <summary>
        /// Starts the import morphological entries process.
        /// </summary>
        /// <param name="inputData">The import morphological entry input data.</param>
        OperationResult StartImportMorphEntries(ImportMorphEntryInputPort inputData);

        /// <summary>
        /// Stops the import morphological entries process.
        /// </summary>
        OperationResult StopImportMorphEntries();
    }
}
