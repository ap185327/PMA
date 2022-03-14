// <copyright file="MemoryLoaderBase.cs" company="Andrey Pospelov">
//     Copyright 2017-2021 Andrey Pospelov. All rights reserved.
// </copyright>

using AutoMapper;
using Microsoft.Extensions.Logging;
using PMA.Domain.Interfaces.Loaders.Base;
using PMA.Domain.Interfaces.Locators;
using PMA.Infrastructure.DbContexts;

namespace PMA.Infrastructure.Loaders.Base
{
    /// <summary>
    /// The base memory loader class.
    /// </summary>
    /// <typeparam name="T">Type of the implementation class.</typeparam>
    /// <typeparam name="TResult">Type of the result data.</typeparam>
    public abstract class MemoryLoaderBase<T, TResult> : LoaderBase<T>, IMemoryLoader<TResult> where T : class
    {
        /// <summary>
        /// Initializes the new instance of <see cref="MemoryLoaderBase{T, TResult}"/> class.
        /// </summary>
        /// <param name="serviceLocator">The service locator.</param>
        /// <param name="context">The database context.</param>
        /// <param name="mapper">The mapper.</param>
        /// <param name="logger">The logger.</param>
        protected MemoryLoaderBase(IServiceLocator serviceLocator, SqLiteDbContext context, IMapperBase mapper, ILogger<T> logger) : base(serviceLocator, context, mapper, logger)
        {
        }

        #region Implementation of IMemoryLoader

        /// <summary>
        /// Gets mapping data.
        /// </summary>
        /// <returns>The result of operation.</returns>
        public abstract TResult GetData();

        #endregion
    }
}
