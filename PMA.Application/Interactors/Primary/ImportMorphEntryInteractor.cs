// <copyright file="ImportMorphEntryInteractor.cs" company="Andrey Pospelov">
//     Copyright 2017-2021 Andrey Pospelov. All rights reserved.
// </copyright>

using Microsoft.Extensions.Logging;
using PMA.Application.Interactors.Base;
using PMA.Domain.DataContracts;
using PMA.Domain.InputPorts;
using PMA.Domain.Interfaces.Interactors.Primary;
using PMA.Domain.Interfaces.UseCases.Primary;
using PMA.Utils.Extensions;

namespace PMA.Application.Interactors.Primary
{
    /// <summary>
    /// The import morphological entry interactor class.
    /// </summary>
    public sealed class ImportMorphEntryInteractor : InteractorBase<ImportMorphEntryInteractor>, IImportMorphEntryInteractor
    {
        /// <summary>
        /// The start import morphological entries use case.
        /// </summary>
        private readonly IStartImportMorphEntriesUseCase _startImportMorphEntriesUseCase;

        /// <summary>
        /// The stop import morphological entries use case.
        /// </summary>
        private readonly IStopImportMorphEntriesUseCase _stopImportMorphEntriesUseCase;

        /// <summary>
        /// Initializes a new instance of <see cref="ImportMorphEntryInteractor"/> class.
        /// </summary>
        /// <param name="startImportMorphEntriesUseCase">The start import morphological entries use case.</param>
        /// <param name="stopImportMorphEntriesUseCase">The stop import morphological entries use case.</param>
        /// <param name="logger">The logger.</param>
        public ImportMorphEntryInteractor(IStartImportMorphEntriesUseCase startImportMorphEntriesUseCase,
            IStopImportMorphEntriesUseCase stopImportMorphEntriesUseCase,
            ILogger<ImportMorphEntryInteractor> logger) : base(logger)
        {
            _startImportMorphEntriesUseCase = startImportMorphEntriesUseCase;
            _stopImportMorphEntriesUseCase = stopImportMorphEntriesUseCase;

            Logger.LogInit();
        }

        #region Implementation of IImportMorphEntryViewModelInteractor

        /// <summary>
        /// Starts the import morphological entries process.
        /// </summary>
        /// <param name="inputData">The import morphological entry input data.</param>
        public OperationResult StartImportMorphEntries(ImportMorphEntryInputPort inputData)
        {
            var result = _startImportMorphEntriesUseCase.Execute(inputData);

            if (!result.Success)
            {
                Logger.LogErrors(result.Messages);
            }

            return result;
        }

        /// <summary>
        /// Stops the import morphological entries process.
        /// </summary>
        public OperationResult StopImportMorphEntries()
        {
            var result = _stopImportMorphEntriesUseCase.Execute();

            if (!result.Success)
            {
                Logger.LogErrors(result.Messages);
            }

            return result;
        }

        #endregion
    }
}
