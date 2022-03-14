// <copyright file="IValidateSolutionUseCase.cs" company="Andrey Pospelov">
//     Copyright 2017-2021 Andrey Pospelov. All rights reserved.
// </copyright>

using PMA.Domain.InputPorts;
using PMA.Domain.Interfaces.UseCases.Base;

namespace PMA.Domain.Interfaces.UseCases.Secondary
{
    /// <summary>
    /// Initializes a new instance of the <see cref="IValidateSolutionUseCase"/> interfacing class.
    /// </summary>
    public interface IValidateSolutionUseCase : IUseCase<MorphParserInputPort>
    {
    }
}
