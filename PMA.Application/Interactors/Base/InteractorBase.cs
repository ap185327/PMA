// <copyright file="InteractorBase.cs" company="Andrey Pospelov">
//     Copyright 2017-2021 Andrey Pospelov. All rights reserved.
// </copyright>

using Microsoft.Extensions.Logging;
using PMA.Domain.Interfaces.Interactors.Base;

namespace PMA.Application.Interactors.Base
{
    /// <summary>
    /// The base interactor class.
    /// </summary>
    /// <typeparam name="T">Type of the implementation class.</typeparam>
    public class InteractorBase<T> : IInteractor where T : IInteractor
    {
        /// <summary>
        /// The logger.
        /// </summary>
        protected readonly ILogger<T> Logger;

        /// <summary>
        /// Initializes a new instance of <see cref="InteractorBase{T}"/> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        protected InteractorBase(ILogger<T> logger)
        {
            Logger = logger;
        }
    }
}
