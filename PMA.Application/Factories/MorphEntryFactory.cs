// <copyright file="MorphEntryFactory.cs" company="Andrey Pospelov">
//     Copyright 2017-2021 Andrey Pospelov. All rights reserved.
// </copyright>

using PMA.Domain.Enums;
using PMA.Domain.Models;

namespace PMA.Application.Factories
{
    /// <summary>
    /// The morphological entry factory class.
    /// </summary>
    public static class MorphEntryFactory
    {
        /// <summary>
        /// Create a new instance of the <see cref="MorphEntry"/> class.
        /// </summary>
        /// <param name="id">The ID.</param>
        /// <returns>A new instance of the <see cref="MorphEntry"/> class.</returns>
        public static MorphEntry Create(int id)
        {
            return new()
            {
                Id = id,
                Parameters = ParameterFactory.Create()
            };
        }

        /// <summary>
        /// Create a new instance of the <see cref="MorphEntry"/> class.
        /// </summary>
        /// <param name="entry">The entry.</param>
        /// <returns>A new instance of the <see cref="MorphEntry"/> class.</returns>
        public static MorphEntry Create(string entry)
        {
            return new()
            {
                Entry = entry,
                Parameters = ParameterFactory.Create()
            };
        }

        /// <summary>
        /// Create a new instance of the <see cref="MorphEntry"/> class.
        /// </summary>
        /// <param name="id">The ID.</param>
        /// <param name="entry">The entry.</param>
        /// <returns>A new instance of the <see cref="MorphEntry"/> class.</returns>
        public static MorphEntry Create(int id, string entry)
        {
            return new()
            {
                Id = id,
                Entry = entry,
                Parameters = ParameterFactory.Create()
            };
        }

        /// <summary>
        /// Create a new instance of the <see cref="MorphEntry"/> class.
        /// </summary>
        /// <param name="id">The ID.</param>
        /// <param name="entry">The entry.</param>
        /// <param name="parameters">Morphological parameters.</param>
        /// <param name="morphBase">The morphological base.</param>
        /// <param name="isVirtual">The solution is virtual (doesn't exist in the live language) or not.</param>
        /// <returns>A new instance of the <see cref="MorphEntry"/> class.</returns>
        public static MorphEntry Create(int id, string entry, byte[] parameters, MorphBase morphBase, bool isVirtual)
        {
            return new()
            {
                Id = id,
                Entry = entry,
                Parameters = parameters,
                Base = morphBase,
                IsVirtual = isVirtual
            };
        }

        /// <summary>
        /// Create a new instance of the <see cref="MorphEntry"/> class.
        /// </summary>
        /// <param name="id">The ID.</param>
        /// <param name="entry">The entry.</param>
        /// <param name="parameters">Morphological parameters.</param>
        /// <param name="morphBase">The morphological base.</param>
        /// <param name="isVirtual">The solution is virtual (doesn't exist in the live language) or not.</param>
        /// <param name="source">The morphological entry source.</param>
        /// <returns>A new instance of the <see cref="MorphEntry"/> class.</returns>
        public static MorphEntry Create(int id, string entry, byte[] parameters, MorphBase morphBase, bool isVirtual, MorphEntrySource source)
        {
            return new()
            {
                Id = id,
                Entry = entry,
                Parameters = parameters,
                Base = morphBase,
                IsVirtual = isVirtual,
                Source = source
            };
        }
    }
}
