// <copyright file="IRemoveSolutionWithExcessiveDepthUseCase.cs" company="Andrey Pospelov">
//     Copyright 2017-2021 Andrey Pospelov. All rights reserved.
// </copyright>

using PMA.Domain.InputPorts;
using PMA.Domain.Interfaces.UseCases.Base;

namespace PMA.Domain.Interfaces.UseCases.Secondary
{
    /// <summary>
    /// Initializes a new instance of the <see cref="IRemoveSolutionWithExcessiveDepthUseCase"/> interfacing class.
    /// </summary>
    public interface IRemoveSolutionWithExcessiveDepthUseCase : IUseCase<MorphParserInputPort>
    {
    }
}
