// <copyright file="ParameterFactory.cs" company="Andrey Pospelov">
//     Copyright 2017-2021 Andrey Pospelov. All rights reserved.
// </copyright>

using PMA.Infrastructure.Entities;

namespace PMA.Infrastructure.Factories
{
    /// <summary>
    /// The parameter factory class.
    /// </summary>
    public static class ParameterFactory
    {
        /// <summary>
        /// Create a new instance of the <see cref="byte"/>[] class.
        /// </summary>
        /// <param name="entity">The morphological entry entity.</param>
        /// <returns>A new instance of the <see cref="byte"/>[] class.</returns>
        public static byte[] Create(MorphEntryEntity entity)
        {
            return new[]
            {
                entity.Parameter0,
                entity.Parameter1,
                entity.Parameter2,
                entity.Parameter3,
                entity.Parameter4,
                entity.Parameter5,
                entity.Parameter6,
                entity.Parameter7,
                entity.Parameter8,
                entity.Parameter9,
                entity.Parameter10,
                entity.Parameter11,
                entity.Parameter12,
                entity.Parameter13,
                entity.Parameter14,
                entity.Parameter15,
                entity.Parameter16,
                entity.Parameter17,
                entity.Parameter18,
                entity.Parameter19,
                entity.Parameter20
            };
        }

        /// <summary>
        /// Create a new instance of the <see cref="byte"/>[] class.
        /// </summary>
        /// <param name="entity">The morphological combination entity.</param>
        /// <returns>A new instance of the <see cref="byte"/>[] class.</returns>
        public static byte[] Create(MorphCombinationEntity entity)
        {
            return new[]
            {
                entity.Parameter0,
                entity.Parameter1,
                entity.Parameter2,
                entity.Parameter3,
                entity.Parameter4,
                entity.Parameter5,
                entity.Parameter6,
                entity.Parameter7,
                entity.Parameter8,
                entity.Parameter9,
                entity.Parameter10,
                entity.Parameter11,
                entity.Parameter12,
                entity.Parameter13,
                entity.Parameter14,
                entity.Parameter15,
                entity.Parameter16,
                entity.Parameter17,
                entity.Parameter18,
                entity.Parameter19,
                entity.Parameter20
            };
        }
    }
}
