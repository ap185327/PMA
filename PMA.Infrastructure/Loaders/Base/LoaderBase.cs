// <copyright file="LoaderBase.cs" company="Andrey Pospelov">
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
    /// The base loader class.
    /// </summary>
    /// <typeparam name="T">Type of the implementation class.</typeparam>
    public abstract class LoaderBase<T> : ILoader where T : class
    {
        /// <summary>
        /// The service locator.
        /// </summary>
        protected readonly IServiceLocator ServiceLocator;

        /// <summary>
        /// The database context.
        /// </summary>
        protected readonly SqLiteDbContext Context;

        /// <summary>
        /// The mapper.
        /// </summary>
        protected readonly IMapperBase Mapper;

        /// <summary>
        /// The logger.
        /// </summary>
        protected readonly ILogger<T> Logger;

        /// <summary>
        /// Initializes the new instance of <see cref="LoaderBase{T}"/> class.
        /// </summary>
        /// <param name="serviceLocator">The service locator.</param>
        /// <param name="context">The database context.</param>
        /// <param name="mapper">The mapper.</param>
        /// <param name="logger">The logger.</param>
        protected LoaderBase(IServiceLocator serviceLocator,
            SqLiteDbContext context,
            IMapperBase mapper,
            ILogger<T> logger)
        {
            ServiceLocator = serviceLocator;
            Context = context;
            Mapper = mapper;
            Logger = logger;
        }

        #region Implementation of ILoader

        /// <summary>
        /// Reads raw data.
        /// </summary>
        /// <returns>The result of operation.</returns>
        public abstract bool ReadRawData();

        /// <summary>
        /// Validates and formats raw data.
        /// </summary>
        /// <returns>The result of operation.</returns>
        public abstract bool ValidateAndFormatRawData();

        /// <summary>
        /// Clears temp data.
        /// </summary>
        /// <returns>The result of operation.</returns>
        public abstract void Clear();

        #endregion
    }
}
