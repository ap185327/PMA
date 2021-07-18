// <copyright file="GetMorphParameterByIdUseCase.cs" company="Andrey Pospelov">
//     Copyright 2017-2021 Andrey Pospelov. All rights reserved.
// </copyright>

using MediatR;
using Microsoft.Extensions.Logging;
using PMA.Application.UseCases.Base;
using PMA.Domain.Constants;
using PMA.Domain.DataContracts;
using PMA.Domain.Interfaces.Providers;
using PMA.Domain.Interfaces.UseCases.Primary;
using PMA.Domain.Models;
using PMA.Utils.Extensions;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace PMA.Application.UseCases.Primary
{
    /// <summary>
    /// The get morphological parameter by ID use case class.
    /// </summary>
    public sealed class GetMorphParameterByIdUseCase : UseCaseWithResultBase<GetMorphParameterByIdUseCase, int, MorphParameter>, IGetMorphParameterByIdUseCase
    {
        /// <summary>
        /// The morphological parameter database provider.
        /// </summary>
        private readonly IMorphParameterDbProvider _morphParameterDbProvider;

        /// <summary>
        /// Initializes a new instance of <see cref="GetMorphParameterByIdUseCase"/> class.
        /// </summary>
        /// <param name="morphParameterDbProvider">The morphological parameter database provider.</param>
        /// <param name="mediator">The mediator.</param>
        /// <param name="parallelOptions">Options that configure the operation of methods on the <see cref="Parallel"/> class.</param>
        /// <param name="logger">The logger.</param>
        public GetMorphParameterByIdUseCase(IMorphParameterDbProvider morphParameterDbProvider, IMediator mediator, ParallelOptions parallelOptions, ILogger<GetMorphParameterByIdUseCase> logger) : base(mediator, parallelOptions, logger)
        {
            _morphParameterDbProvider = morphParameterDbProvider;

            Logger.LogInit();
        }

        #region Overrides of UseCaseWithResultBase<GetMorphParameterByIdUseCase,int,MorphParameter>

        /// <summary>
        /// Executes an action.
        /// </summary>
        /// <param name="inputData">The input data.</param>
        /// <returns>The result of action execution.</returns>
        public override OperationResult<MorphParameter> Execute(int inputData)
        {
            if (inputData is >= MorphConstants.ParameterCount or < 0)
            {
                Logger.LogError(ErrorMessageConstants.MorphParameterIndexOutOfRange, inputData);
                return OperationResult<MorphParameter>.FailureResult(ErrorMessageConstants.MorphParameterIndexOutOfRange, inputData);
            }

            try
            {
                var morphParameter = _morphParameterDbProvider.GetValues().Single(x => x.Id == inputData);
                return OperationResult<MorphParameter>.SuccessResult(morphParameter);
            }
            catch (Exception exception)
            {
                Logger.LogError(exception.Message);
                return OperationResult<MorphParameter>.ExceptionResult(exception);
            }
        }

        #endregion
    }
}
