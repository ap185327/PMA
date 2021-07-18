// <copyright file="ITryToUpdateMorphEntryUseCase.cs" company="Andrey Pospelov">
//     Copyright 2017-2021 Andrey Pospelov. All rights reserved.
// </copyright>

using PMA.Domain.Interfaces.UseCases.Base;
using PMA.Domain.Models;
using PMA.Domain.OutputPorts;

namespace PMA.Domain.Interfaces.UseCases.Primary
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ITryToUpdateMorphEntryUseCase"/> interfacing class.
    /// </summary>
    public interface ITryToUpdateMorphEntryUseCase : IUseCaseWithResult<MorphEntry, UpdateMorphEntryOutputPort>
    {
    }
}
