// <copyright file="DbLoaderBase.cs" company="Andrey Pospelov">
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
    /// The base database loader class.
    /// </summary>
    /// <typeparam name="T">Type of the implementation class.</typeparam>
    public abstract class DbLoaderBase<T> : LoaderBase<T>, IDbLoader where T : class
    {
        /// <summary>
        /// Initializes the new instance of <see cref="DbLoaderBase{T}"/> class.
        /// </summary>
        /// <param name="serviceLocator">The service locator.</param>
        /// <param name="context">The database context.</param>
        /// <param name="mapper">The mapper.</param>
        /// <param name="logger">The logger.</param>
        protected DbLoaderBase(IServiceLocator serviceLocator, SqLiteDbContext context, IMapperBase mapper, ILogger<T> logger) : base(serviceLocator, context, mapper, logger)
        {
        }

        #region Implementation of IDbLoader

        /// <summary>
        /// Loads mapping data to database.
        /// </summary>
        /// <returns>True if the raw data has been loaded; otherwise - False.</returns>
        public abstract bool LoadData();

        #endregion
    }
}
