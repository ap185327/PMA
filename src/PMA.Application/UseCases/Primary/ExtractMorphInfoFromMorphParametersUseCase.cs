// <copyright file="ExtractMorphInfoFromMorphParametersUseCase.cs" company="Andrey Pospelov">
//     Copyright 2017-2021 Andrey Pospelov. All rights reserved.
// </copyright>

using Microsoft.Extensions.Logging;
using PMA.Application.UseCases.Base;
using PMA.Domain.DataContracts;
using PMA.Domain.InputPorts;
using PMA.Domain.Interfaces.Providers;
using PMA.Domain.Interfaces.UseCases.Primary;
using System;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PMA.Application.UseCases.Primary
{
    /// <summary>
    /// The extract morphological information from morphological parameters use case class.
    /// </summary>
    public sealed class ExtractMorphInfoFromMorphParametersUseCase : UseCaseWithResultBase<ExtractMorphInfoFromMorphParametersUseCase, ExtractMorphInfoInputPort, string>, IExtractMorphInfoFromMorphParametersUseCase
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
        /// <param name="logger">The logger.</param>
        public ExtractMorphInfoFromMorphParametersUseCase(ITermDbProvider termDbProvider,
            IMorphParameterDbProvider morphParameterDbProvider,
            ILogger<ExtractMorphInfoFromMorphParametersUseCase> logger) : base(logger)
        {
            _termDbProvider = termDbProvider;
            _morphParameterDbProvider = morphParameterDbProvider;
        }

        #region Overrides of UseCaseWithResultBase<ExtractMorphInfoFromMorphParametersUseCase,ExtractMorphInfoInputPort,string>

        /// <summary>
        /// Executes an action.
        /// </summary>
        /// <param name="inputPort">The input data.</param>
        /// <returns>The result of action execution.</returns>
        public override OperationResult<string> Execute(ExtractMorphInfoInputPort inputPort)
        {
            try
            {
                var stringBuilder = new StringBuilder();
                for (int i = 0; i < inputPort.Parameters.Length; i++)
                {
                    string value = _termDbProvider.GetValues().Single(x => x.Id == inputPort.Parameters[i]).AltEntry;
                    if (!string.IsNullOrEmpty(value) && (!inputPort.UseVisibility || _morphParameterDbProvider.GetValues().Single(x => x.Id == i).IsVisible))
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

        /// <summary>
        /// Executes an action.
        /// </summary>
        /// <param name="inputPort">The input data.</param>
        /// <param name="token">The cancellation token.</param>
        /// <returns>The result of action execution.</returns>
        public override Task<OperationResult<string>> ExecuteAsync(ExtractMorphInfoInputPort inputPort, CancellationToken token = default)
        {
            return Task.FromResult(Execute(inputPort));
        }

        #endregion
    }
}
