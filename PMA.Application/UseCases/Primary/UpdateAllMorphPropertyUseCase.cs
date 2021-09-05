// <copyright file="UpdateAllMorphPropertyUseCase.cs" company="Andrey Pospelov">
//     Copyright 2017-2021 Andrey Pospelov. All rights reserved.
// </copyright>

using Microsoft.Extensions.Logging;
using PMA.Application.UseCases.Base;
using PMA.Domain.Constants;
using PMA.Domain.DataContracts;
using PMA.Domain.InputPorts;
using PMA.Domain.Interfaces.Managers;
using PMA.Domain.Interfaces.UseCases.Primary;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace PMA.Application.UseCases.Primary
{
    /// <summary>
    /// The update all morphological property use case class.
    /// </summary>
    public sealed class UpdateAllMorphPropertyUseCase : UseCaseBase<UpdateAllMorphPropertyUseCase, UpdateMorphPropertyInputPort>, IUpdateAllMorphPropertyUseCase
    {
        /// <summary>
        /// The morphological combination manager.
        /// </summary>
        private readonly IMorphCombinationManager _morphCombinationManager;

        /// <summary>
        /// Initializes a new instance of <see cref="UpdateAllMorphPropertyUseCase"/> class.
        /// </summary>
        /// <param name="morphCombinationManager">The morphological combination manager.</param>
        /// <param name="logger">The logger.</param>
        public UpdateAllMorphPropertyUseCase(IMorphCombinationManager morphCombinationManager, ILogger<UpdateAllMorphPropertyUseCase> logger) : base(logger)
        {
            _morphCombinationManager = morphCombinationManager;
        }

        #region Overrides of UseCaseBase<UpdateAllMorphPropertyUseCase,UpdateMorphPropertyInputPort>

        /// <summary>
        /// Executes an action.
        /// </summary>
        /// <param name="inputPort">The input data.</param>
        /// <returns>The result of action execution.</returns>
        public override OperationResult Execute(UpdateMorphPropertyInputPort inputPort)
        {
            if (inputPort is null)
            {
                Logger.LogError(ErrorMessageConstants.ValueIsNull, nameof(inputPort));
                return OperationResult.FailureResult(ErrorMessageConstants.ValueIsNull, nameof(inputPort));
            }

            if (inputPort.Properties is null)
            {
                Logger.LogError(ErrorMessageConstants.ValueIsNull, nameof(inputPort.Properties));
                return OperationResult.FailureResult(ErrorMessageConstants.ValueIsNull, nameof(inputPort.Properties));
            }

            if (inputPort.StartIndex is >= MorphConstants.ParameterCount or < 0)
            {
                Logger.LogError(ErrorMessageConstants.MorphParameterIndexOutOfRange, inputPort.StartIndex);
                return OperationResult.FailureResult(ErrorMessageConstants.MorphParameterIndexOutOfRange, inputPort.StartIndex);
            }

            for (int i = inputPort.StartIndex; i < MorphConstants.ParameterCount; i++)
            {
                byte[] parameters = inputPort.Properties.Where(x => x.Index < i).OrderBy(x => x.Index).Select(x => x.TermIds[x.TermEntries.IndexOf(x.SelectedTerm)]).ToArray();

                var combinations = _morphCombinationManager.GetValidParameters(parameters);

                var values = combinations.Select(x => x[i]).Distinct().ToList();

                if (values.Count > 1)
                {
                    values.Add(MorphConstants.UnknownTermId);
                }

                inputPort.Properties[i].TermIds = values.OrderBy(x => x).ToList();
            }

            return OperationResult.SuccessResult();
        }

        /// <summary>
        /// Executes an action.
        /// </summary>
        /// <param name="inputPort">The input data.</param>
        /// <param name="token">The cancellation token.</param>
        /// <returns>The result of action execution.</returns>
        public override Task<OperationResult> ExecuteAsync(UpdateMorphPropertyInputPort inputPort, CancellationToken token = default)
        {
            return Task.FromResult(Execute(inputPort));
        }

        #endregion
    }
}
