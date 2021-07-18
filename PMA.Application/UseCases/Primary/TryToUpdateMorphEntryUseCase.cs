// <copyright file="TryToUpdateMorphEntryUseCase.cs" company="Andrey Pospelov">
//     Copyright 2017-2021 Andrey Pospelov. All rights reserved.
// </copyright>

using MediatR;
using Microsoft.Extensions.Logging;
using PMA.Application.Extensions;
using PMA.Application.Factories;
using PMA.Application.UseCases.Base;
using PMA.Domain.Constants;
using PMA.Domain.DataContracts;
using PMA.Domain.Enums;
using PMA.Domain.Interfaces.Managers;
using PMA.Domain.Interfaces.Providers;
using PMA.Domain.Interfaces.Services;
using PMA.Domain.Interfaces.UseCases.Primary;
using PMA.Domain.Models;
using PMA.Domain.OutputPorts;
using PMA.Utils.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PMA.Application.UseCases.Primary
{
    /// <summary>
    /// The try to update morphological entry use case class.
    /// </summary>
    public sealed class TryToUpdateMorphEntryUseCase : UseCaseWithResultBase<TryToUpdateMorphEntryUseCase, MorphEntry, UpdateMorphEntryOutputPort>, ITryToUpdateMorphEntryUseCase
    {
        /// <summary>
        /// The morphological entry database provider.
        /// </summary>
        private readonly IMorphEntryDbProvider _morphEntryDbProvider;

        /// <summary>
        /// The morphological entry manager.
        /// </summary>
        private readonly IMorphEntryManager _morphEntryManager;

        /// <summary>
        /// The morphological combination manager.
        /// </summary>
        private readonly IMorphCombinationManager _morphCombinationManager;

        /// <summary>
        /// The translate service.
        /// </summary>
        private readonly ITranslateService _translateService;

        /// <summary>
        /// Initializes a new instance of <see cref="TryToUpdateMorphEntryUseCase"/> class.
        /// </summary>
        /// <param name="morphEntryDbProvider">The morphological entry database provider.</param>
        /// <param name="morphEntryManager">The morphological entry manager.</param>
        /// <param name="morphCombinationManager">The morphological combination manager.</param>
        /// <param name="translateService">The translate service.</param>
        /// <param name="mediator">The mediator.</param>
        /// <param name="parallelOptions">Options that configure the operation of methods on the <see cref="Parallel"/> class.</param>
        /// <param name="logger">The logger.</param>
        public TryToUpdateMorphEntryUseCase(IMorphEntryDbProvider morphEntryDbProvider,
            IMorphEntryManager morphEntryManager,
            IMorphCombinationManager morphCombinationManager,
            ITranslateService translateService,
            IMediator mediator,
            ParallelOptions parallelOptions,
            ILogger<TryToUpdateMorphEntryUseCase> logger) : base(mediator,
            parallelOptions,
            logger)
        {
            _morphEntryDbProvider = morphEntryDbProvider;
            _morphEntryManager = morphEntryManager;
            _morphCombinationManager = morphCombinationManager;
            _translateService = translateService;

            Logger.LogInit();
        }

        #region Overrides of UseCaseWithResultBase<TryToUpdateMorphEntryUseCase,MorphEntry,UpdateMorphEntryOutputPort>

        /// <summary>
        /// Executes an action.
        /// </summary>
        /// <param name="inputData">The input data.</param>
        /// <returns>The result of action execution.</returns>
        public override OperationResult<UpdateMorphEntryOutputPort> Execute(MorphEntry inputData)
        {
            if (inputData is null)
            {
                Logger.LogError(ErrorMessageConstants.ValueIsNull, nameof(inputData));
                return OperationResult<UpdateMorphEntryOutputPort>.FailureResult(ErrorMessageConstants.ValueIsNull, nameof(inputData));
            }

            try
            {
                string error = Validate(inputData);

                if (!string.IsNullOrEmpty(error))
                {
                    return OperationResult<UpdateMorphEntryOutputPort>.SuccessResult(new UpdateMorphEntryOutputPort
                    {
                        Error = error
                    });
                }

                if (inputData.Id > 0)
                {
                    var morphEntry = _morphEntryDbProvider.GetValues().Single(x => x.Id == inputData.Id);

                    morphEntry.Entry = inputData.Entry;
                    morphEntry.Parameters = inputData.Parameters;
                    morphEntry.Base = inputData.Base;
                    morphEntry.Left = inputData.Left;
                    morphEntry.Right = inputData.Right;
                    morphEntry.IsVirtual = inputData.IsVirtual;
                    morphEntry.Source = inputData.Source;

                    _morphEntryDbProvider.Update(morphEntry);

                    return OperationResult<UpdateMorphEntryOutputPort>.SuccessResult(new UpdateMorphEntryOutputPort
                    {
                        Ids = new List<int> { morphEntry.Id }
                    });
                }

                var values = _morphEntryManager.GetValues(inputData.Entry, inputData.Parameters, inputData.Base, inputData.IsVirtual, inputData.Left, inputData.Right);

                if (values.Count == 0)
                {
                    return OperationResult<UpdateMorphEntryOutputPort>.SuccessResult(new UpdateMorphEntryOutputPort
                    {
                        Error = _translateService.Translate(MorphEntryError.NoSimilarEntriesFound)
                    });
                }

                var newMorphEntries = new List<MorphEntry>();

                foreach (var entry in values)
                {
                    byte[] tempParameters = ParameterFactory.Clone(entry.Parameters);

                    bool isUpdated = entry.Parameters.UpdateByParameters(inputData.Parameters);

                    if (!isUpdated || _morphCombinationManager.Check(entry.Parameters))
                    {
                        entry.Base = inputData.Base;
                        entry.Source = inputData.Source;

                        _morphEntryDbProvider.Update(entry, false);

                        newMorphEntries.Add(entry);
                    }
                    else
                    {
                        entry.Parameters = tempParameters;
                    }
                }

                if (newMorphEntries.Count > 0)
                {
                    _morphEntryDbProvider.Commit();

                    return OperationResult<UpdateMorphEntryOutputPort>.SuccessResult(new UpdateMorphEntryOutputPort
                    {
                        Ids = newMorphEntries.Select(x => x.Id).ToList()
                    });
                }

                return OperationResult<UpdateMorphEntryOutputPort>.SuccessResult(new UpdateMorphEntryOutputPort
                {
                    Error = _translateService.Translate(MorphEntryError.NoSimilarEntriesFound)
                });
            }
            catch (Exception exception)
            {
                Logger.LogError(exception.Message);
                return OperationResult<UpdateMorphEntryOutputPort>.ExceptionResult(exception);
            }
        }

        #endregion

        /// <summary>
        /// Validates a morphological entry.
        /// </summary>
        /// <param name="morphEntry">A morphological entry.</param>
        /// <returns>An error.</returns>
        private string Validate(MorphEntry morphEntry)
        {
            string error = null;

            if (morphEntry.Id > 0 && _morphEntryDbProvider.GetValues().SingleOrDefault(x => x.Id == morphEntry.Id) is null)
            {
                error = _translateService.Translate(MorphEntryError.IdDoesNotExist, morphEntry.Id);
            }
            else if (string.IsNullOrEmpty(morphEntry.Entry))
            {
                error = _translateService.Translate(MorphEntryError.EntryIsEmpty);
            }
            else if (morphEntry.Parameters is null)
            {
                error = _translateService.Translate(MorphEntryError.ParametersAreEmpty);
            }
            else if (morphEntry.Left != null && morphEntry.Base == MorphBase.None)
            {
                error = _translateService.Translate(MorphEntryError.LeftDoesNotMatchBase, morphEntry.Left.Entry, morphEntry.Base);
            }
            else if (morphEntry.Right != null && morphEntry.Base == MorphBase.None)
            {
                error = _translateService.Translate(MorphEntryError.RightDoesNotMatchBase, morphEntry.Right.Entry, morphEntry.Base);
            }
            else if (!_morphCombinationManager.Check(morphEntry.Parameters))
            {
                error = _translateService.Translate(MorphEntryError.NoMorphCombinationMatches);
            }

            return error;
        }
    }
}
