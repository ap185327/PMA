// <copyright file="GetEntryIdInteractor.cs" company="Andrey Pospelov">
//     Copyright 2017-2021 Andrey Pospelov. All rights reserved.
// </copyright>

using Microsoft.Extensions.Logging;
using PMA.Application.Interactors.Base;
using PMA.Domain.DataContracts;
using PMA.Domain.Interfaces.Interactors.Primary;
using PMA.Domain.Interfaces.UseCases.Primary;
using PMA.Domain.Models;
using PMA.Domain.OutputPorts;
using PMA.Utils.Extensions;
using System.Collections.Generic;

namespace PMA.Application.Interactors.Primary
{
    /// <summary>
    /// The get entry ID interactor class.
    /// </summary>
    public sealed class GetEntryIdInteractor : InteractorBase<GetEntryIdInteractor>, IGetEntryIdInteractor
    {
        /// <summary>
        /// The try to delete morphological entries use case.
        /// </summary>
        private readonly ITryToDeleteMorphEntriesUseCase _tryToDeleteMorphEntriesUseCase;

        /// <summary>
        /// The extract morphological information from morphological parameters use case.
        /// </summary>
        private readonly IExtractMorphInfoFromMorphParametersUseCase _extractMorphInfoFromMorphParametersUseCase;

        /// <summary>
        /// The get a collection of morphological entries by the morphological entry use case.
        /// </summary>
        private readonly IGetMorphEntriesByMorphEntryUseCase _getMorphEntriesByMorphEntryUseCase;

        /// <summary>
        /// Initializes a new instance of <see cref="GetEntryIdInteractor"/> class.
        /// </summary>
        /// <param name="tryToDeleteMorphEntriesUseCase">The try to delete morphological entries use case.</param>
        /// <param name="extractMorphInfoFromMorphParametersUseCase">The extract morphological information from morphological parameters use case.</param>
        /// <param name="getMorphEntriesByMorphEntryUseCase">The get a collection of morphological entries by the morphological entry use case.</param>
        /// <param name="logger">The logger.</param>
        public GetEntryIdInteractor(ITryToDeleteMorphEntriesUseCase tryToDeleteMorphEntriesUseCase,
            IExtractMorphInfoFromMorphParametersUseCase extractMorphInfoFromMorphParametersUseCase,
            IGetMorphEntriesByMorphEntryUseCase getMorphEntriesByMorphEntryUseCase,
            ILogger<GetEntryIdInteractor> logger) : base(logger)
        {
            _tryToDeleteMorphEntriesUseCase = tryToDeleteMorphEntriesUseCase;
            _extractMorphInfoFromMorphParametersUseCase = extractMorphInfoFromMorphParametersUseCase;
            _getMorphEntriesByMorphEntryUseCase = getMorphEntriesByMorphEntryUseCase;

            Logger.LogInit();
        }

        #region Implementation of IGetEntryIdViewModelInteractor

        /// <summary>
        /// Tries to delete a collection of morphological entries from the database.
        /// </summary>
        /// <param name="ids">The collection of morphological entry IDs.</param>
        /// <returns>The operation result.</returns>
        public OperationResult<IList<DeleteMorphEntryOutputPort>> TryToDeleteMorphEntries(IList<int> ids)
        {
            var result = _tryToDeleteMorphEntriesUseCase.Execute(ids);

            if (!result.Success)
            {
                Logger.LogErrors(result.Messages);
            }

            return result;
        }

        /// <summary>
        /// Extracts a morphological information from the collection of morphological parameters.
        /// </summary>
        /// <param name="parameters">The collection of morphological parameters.</param>
        /// <returns>The operation result.</returns>
        public OperationResult<string> ExtractMorphInfoFromMorphParameters(byte[] parameters)
        {
            var result = _extractMorphInfoFromMorphParametersUseCase.Execute(parameters);

            if (!result.Success)
            {
                Logger.LogErrors(result.Messages);
            }

            return result;
        }

        /// <summary>
        /// Gets a collection of morphological entries by the morphological entry.
        /// </summary>
        /// <param name="morphEntry">The morphological entry.</param>
        /// <returns>The operation result.</returns>
        public OperationResult<IList<MorphEntry>> GetMorphEntriesByMorphEntry(MorphEntry morphEntry)
        {
            var result = _getMorphEntriesByMorphEntryUseCase.Execute(morphEntry);

            if (!result.Success)
            {
                Logger.LogErrors(result.Messages);
            }

            return result;
        }

        #endregion
    }
}
