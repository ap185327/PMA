// <copyright file="DbProviderBase.cs" company="Andrey Pospelov">
//     Copyright 2017-2021 Andrey Pospelov. All rights reserved.
// </copyright>

using Microsoft.Extensions.Logging;
using PMA.Domain.Interfaces.Services.Base;

namespace PMA.Infrastructure.Services.Base
{
    /// <summary>
    /// The base service class.
    /// </summary>
    /// <typeparam name="T">Type of the implementation class.</typeparam>
    public abstract class ServiceBase<T> : IService where T : class
    {
        /// <summary>
        /// The logger.
        /// </summary>
        protected readonly ILogger<T> Logger;

        /// <summary>
        /// Initializes the new instance of <see cref="ServiceBase{T}"/> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        protected ServiceBase(ILogger<T> logger)
        {
            Logger = logger;
        }
    }
}
