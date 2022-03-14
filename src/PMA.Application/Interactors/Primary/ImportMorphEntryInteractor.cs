// <copyright file="ImportMorphEntryInteractor.cs" company="Andrey Pospelov">
//     Copyright 2017-2021 Andrey Pospelov. All rights reserved.
// </copyright>

using Microsoft.Extensions.Logging;
using PMA.Application.Interactors.Base;
using PMA.Domain.DataContracts;
using PMA.Domain.InputPorts;
using PMA.Domain.Interfaces.Interactors.Primary;
using PMA.Domain.Interfaces.UseCases.Primary;
using System.Threading;
using System.Threading.Tasks;

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
        /// Initializes a new instance of <see cref="ImportMorphEntryInteractor"/> class.
        /// </summary>
        /// <param name="startImportMorphEntriesUseCase">The start import morphological entries use case.</param>
        /// <param name="logger">The logger.</param>
        public ImportMorphEntryInteractor(IStartImportMorphEntriesUseCase startImportMorphEntriesUseCase,
            ILogger<ImportMorphEntryInteractor> logger) : base(logger)
        {
            _startImportMorphEntriesUseCase = startImportMorphEntriesUseCase;
        }

        #region Implementation of IImportMorphEntryInteractor

        /// <summary>
        /// Starts the import morphological entries process.
        /// </summary>
        /// <param name="inputData">The import morphological entry input data.</param>
        /// <param name="token">The cancellation token.</param>
        /// <returns>The operation result.</returns>
        public async Task<OperationResult> StartImportMorphEntriesAsync(ImportMorphEntryInputPort inputData, CancellationToken token = default)
        {
            return await _startImportMorphEntriesUseCase.ExecuteAsync(inputData, token);
        }

        #endregion
    }
}
