// <copyright file="GetMorphParameterByIdUseCase.cs" company="Andrey Pospelov">
//     Copyright 2017-2021 Andrey Pospelov. All rights reserved.
// </copyright>

using Microsoft.Extensions.Logging;
using PMA.Application.UseCases.Base;
using PMA.Domain.DataContracts;
using PMA.Domain.Interfaces.Providers;
using PMA.Domain.Interfaces.UseCases.Primary;
using PMA.Domain.Models;
using System;
using System.Linq;
using System.Threading;
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
        /// <param name="logger">The logger.</param>
        public GetMorphParameterByIdUseCase(IMorphParameterDbProvider morphParameterDbProvider, ILogger<GetMorphParameterByIdUseCase> logger) : base(logger)
        {
            _morphParameterDbProvider = morphParameterDbProvider;
        }

        #region Overrides of UseCaseWithResultBase<GetMorphParameterByIdUseCase,int,MorphParameter>

        /// <summary>
        /// Executes an action.
        /// </summary>
        /// <param name="inputData">The input data.</param>
        /// <returns>The result of action execution.</returns>
        public override OperationResult<MorphParameter> Execute(int inputData)
        {
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

        /// <summary>
        /// Executes an action.
        /// </summary>
        /// <param name="inputPort">The input data.</param>
        /// <param name="token">The cancellation token.</param>
        /// <returns>The result of action execution.</returns>
        public override Task<OperationResult<MorphParameter>> ExecuteAsync(int inputPort, CancellationToken token = default)
        {
            return Task.FromResult(Execute(inputPort));
        }

        #endregion
    }
}
