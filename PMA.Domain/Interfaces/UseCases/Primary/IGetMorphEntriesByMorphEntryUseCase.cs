// <copyright file="IGetMorphEntriesByMorphEntryUseCase.cs" company="Andrey Pospelov">
//     Copyright 2017-2021 Andrey Pospelov. All rights reserved.
// </copyright>

using PMA.Domain.Interfaces.UseCases.Base;
using PMA.Domain.Models;
using System.Collections.Generic;

namespace PMA.Domain.Interfaces.UseCases.Primary
{
    /// <summary>
    /// Initializes a new instance of the <see cref="IGetMorphEntriesByMorphEntryUseCase"/> interfacing class.
    /// </summary>
    public interface IGetMorphEntriesByMorphEntryUseCase : IUseCaseWithResult<MorphEntry, IList<MorphEntry>>
    {
    }
}
