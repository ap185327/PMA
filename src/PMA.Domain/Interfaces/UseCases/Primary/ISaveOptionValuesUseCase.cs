// <copyright file="ISaveOptionValuesUseCase.cs" company="Andrey Pospelov">
//     Copyright 2017-2021 Andrey Pospelov. All rights reserved.
// </copyright>

using PMA.Domain.InputPorts;
using PMA.Domain.Interfaces.UseCases.Base;

namespace PMA.Domain.Interfaces.UseCases.Primary
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ISaveOptionValuesUseCase"/> interfacing class.
    /// </summary>
    public interface ISaveOptionValuesUseCase : IUseCase<OptionValueInputPort>
    {
    }
}
