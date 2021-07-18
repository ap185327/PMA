// <copyright file="DbProviderBase.cs" company="Andrey Pospelov">
//     Copyright 2017-2021 Andrey Pospelov. All rights reserved.
// </copyright>

using AutoMapper;
using Microsoft.Extensions.Logging;
using PMA.Domain.Interfaces.Providers.Base;
using PMA.Infrastructure.DbContexts;

namespace PMA.Infrastructure.Providers.Base
{
    /// <summary>
    /// The base database provider class.
    /// </summary>
    /// <typeparam name="T">Type of the implementation class.</typeparam>
    public abstract class DbProviderBase<T> : IDbProvider where T : class
    {
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
        /// Initializes the new instance of <see cref="DbProviderBase{T}"/> class.
        /// </summary>
        /// <param name="context">The database context.</param>
        /// <param name="mapper">The mapper.</param>
        /// <param name="logger">The logger.</param>
        protected DbProviderBase(SqLiteDbContext context, IMapperBase mapper, ILogger<T> logger)
        {
            Context = context;
            Mapper = mapper;
            Logger = logger;
        }
    }
}
