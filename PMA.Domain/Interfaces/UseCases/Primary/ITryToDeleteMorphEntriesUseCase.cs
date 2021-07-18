// <copyright file="ITryToDeleteMorphEntriesUseCase.cs" company="Andrey Pospelov">
//     Copyright 2017-2021 Andrey Pospelov. All rights reserved.
// </copyright>

using PMA.Domain.Interfaces.UseCases.Base;
using PMA.Domain.OutputPorts;
using System.Collections.Generic;

namespace PMA.Domain.Interfaces.UseCases.Primary
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ITryToDeleteMorphEntriesUseCase"/> interfacing class.
    /// </summary>
    public interface ITryToDeleteMorphEntriesUseCase : IUseCaseWithResult<IList<int>, IList<DeleteMorphEntryOutputPort>>
    {
    }
}
