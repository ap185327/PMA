// <copyright file="IGetMorphParameterByIdUseCase.cs" company="Andrey Pospelov">
//     Copyright 2017-2021 Andrey Pospelov. All rights reserved.
// </copyright>

using PMA.Domain.Interfaces.UseCases.Base;
using PMA.Domain.Models;

namespace PMA.Domain.Interfaces.UseCases.Primary
{
    /// <summary>
    /// Initializes a new instance of the <see cref="IGetMorphParameterByIdUseCase"/> interfacing class.
    /// </summary>
    public interface IGetMorphParameterByIdUseCase : IUseCaseWithResult<int, MorphParameter>
    {
    }
}
