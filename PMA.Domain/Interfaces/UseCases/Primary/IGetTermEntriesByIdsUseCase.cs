// <copyright file="IGetTermEntriesByIdsUseCase.cs" company="Andrey Pospelov">
//     Copyright 2017-2021 Andrey Pospelov. All rights reserved.
// </copyright>

using PMA.Domain.InputPorts;
using PMA.Domain.Interfaces.UseCases.Base;
using System.Collections.Generic;

namespace PMA.Domain.Interfaces.UseCases.Primary
{
    /// <summary>
    /// Initializes a new instance of the <see cref="IGetTermEntriesByIdsUseCase"/> interfacing class.
    /// </summary>
    public interface IGetTermEntriesByIdsUseCase : IUseCaseWithResult<GetTermEntriesByIdsInputPort, IList<string>>
    {
    }
}
