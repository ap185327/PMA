// <copyright file="IGetEntryIdViewModelUseCase.cs" company="Andrey Pospelov">
//     Copyright 2017-2021 Andrey Pospelov. All rights reserved.
// </copyright>

using PMA.Domain.Interfaces.UseCases.Base;
using PMA.Domain.Interfaces.ViewModels;
using PMA.Domain.Models;

namespace PMA.Domain.Interfaces.UseCases.Primary
{
    /// <summary>
    /// Initializes a new instance of the <see cref="IGetEntryIdViewModelUseCase"/> interfacing class.
    /// </summary>
    public interface IGetEntryIdViewModelUseCase : IUseCaseWithResult<MorphEntry, IGetEntryIdViewModel>
    {
    }
}
