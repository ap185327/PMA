// <copyright file="IGetMorphCategoryControlViewModelsUseCase.cs" company="Andrey Pospelov">
//     Copyright 2017-2021 Andrey Pospelov. All rights reserved.
// </copyright>

using PMA.Domain.Interfaces.UseCases.Base;
using PMA.Domain.Interfaces.ViewModels.Controls;
using System.Collections.Generic;

namespace PMA.Domain.Interfaces.UseCases.Primary
{
    /// <summary>
    /// Initializes a new instance of the <see cref="IGetMorphCategoryControlViewModelsUseCase"/> interfacing class.
    /// </summary>
    public interface IGetMorphCategoryControlViewModelsUseCase : IUseCaseWithResult<IList<IMorphPropertyControlViewModel>, IList<IMorphCategoryControlViewModel>>
    {
    }
}
