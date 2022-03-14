// <copyright file="IGetWordFormTreeNodeUseCase.cs" company="Andrey Pospelov">
//     Copyright 2017-2021 Andrey Pospelov. All rights reserved.
// </copyright>

using PMA.Domain.InputPorts;
using PMA.Domain.Interfaces.UseCases.Base;
using PMA.Domain.Interfaces.ViewModels.Controls;

namespace PMA.Domain.Interfaces.UseCases.Primary
{
    /// <summary>
    /// Initializes a new instance of the <see cref="IGetWordFormTreeNodeUseCase"/> interfacing class.
    /// </summary>
    public interface IGetWordFormTreeNodeUseCase : IUseCaseWithResult<GetWordFormTreeNodeInputPort, IWordFormTreeNodeViewModel>
    {
    }
}
