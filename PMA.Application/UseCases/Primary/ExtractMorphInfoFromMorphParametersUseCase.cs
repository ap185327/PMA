// <copyright file="ExtractMorphInfoFromMorphParametersUseCase.cs" company="Andrey Pospelov">
//     Copyright 2017-2021 Andrey Pospelov. All rights reserved.
// </copyright>

using MediatR;
using Microsoft.Extensions.Logging;
using PMA.Application.UseCases.Base;
using PMA.Domain.Constants;
using PMA.Domain.DataContracts;
using PMA.Domain.Interfaces.Providers;
using PMA.Domain.Interfaces.UseCases.Primary;
using PMA.Utils.Extensions;
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PMA.Application.UseCases.Primary
{
    /// <summary>
    /// The extract morphological information from morphological parameters use case class.
    /// </summary>
    public sealed class ExtractMorphInfoFromMorphParametersUseCase : UseCaseWithResultBase<ExtractMorphInfoFromMorphParametersUseCase, byte[], string>, IExtractMorphInfoFromMorphParametersUseCase
    {
        /// <summary>
        /// The term database provider.
        /// </summary>
        private readonly ITermDbProvider _termDbProvider;

        /// <summary>
        /// The morphological parameter database provider.
        /// </summary>
        private readonly IMorphParameterDbProvider _morphParameterDbProvider;

        /// <summary>
        /// Initializes a new instance of <see cref="ExtractMorphInfoFromMorphParametersUseCase"/> class.
        /// </summary>
        /// <param name="termDbProvider">The term database provider.</param>
        /// <param name="morphParameterDbProvider">The morphological parameter database provider.</param>
        /// <param name="mediator">The mediator.</param>
        /// <param name="parallelOptions">Options that configure the operation of methods on the <see cref="Parallel"/> class.</param>
        /// <param name="logger">The logger.</param>
        public ExtractMorphInfoFromMorphParametersUseCase(ITermDbProvider termDbProvider,
            IMorphParameterDbProvider morphParameterDbProvider,
            IMediator mediator,
            ParallelOptions parallelOptions,
            ILogger<ExtractMorphInfoFromMorphParametersUseCase> logger) : base(mediator,
            parallelOptions,
            logger)
        {
            _termDbProvider = termDbProvider;
            _morphParameterDbProvider = morphParameterDbProvider;

            Logger.LogInit();
        }

        #region Overrides of UseCaseWithResultBase<ExtractMorphInfoFromMorphParametersUseCase,byte[],string>

        /// <summary>
        /// Executes an action.
        /// </summary>
        /// <param name="inputData">The input data.</param>
        /// <returns>The result of action execution.</returns>
        public override OperationResult<string> Execute(byte[] inputData)
        {
            if (inputData is null)
            {
                Logger.LogError(ErrorMessageConstants.ValueIsNull, nameof(inputData));
                return OperationResult<string>.FailureResult(ErrorMessageConstants.ValueIsNull, nameof(inputData));
            }

            try
            {
                var stringBuilder = new StringBuilder();
                for (int i = 0; i < inputData.Length; i++)
                {
                    string value = _termDbProvider.GetValues().Single(x => x.Id == inputData[i]).AltEntry;
                    if (!string.IsNullOrEmpty(value) && _morphParameterDbProvider.GetValues().Single(x => x.Id == i).IsVisible)
                    {
                        stringBuilder.Append(value + " ");
                    }
                }

                return OperationResult<string>.SuccessResult(stringBuilder.ToString());
            }
            catch (Exception exception)
            {
                Logger.LogError(exception.Message);
                return OperationResult<string>.ExceptionResult(exception);
            }
        }

        #endregion
    }
}
