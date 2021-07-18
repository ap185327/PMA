// <copyright file="SolutionContentFactory.cs" company="Andrey Pospelov">
//     Copyright 2017-2021 Andrey Pospelov. All rights reserved.
// </copyright>

using PMA.Domain.Enums;
using PMA.Domain.Models;

namespace PMA.Application.Factories
{
    public static class SolutionContentFactory
    {
        /// <summary>
        /// Create a new instance of the <see cref="SolutionContent"/> class.
        /// </summary>
        /// <param name="id">The ID.</param>
        /// <param name="parameters">Morphological parameters.</param>
        /// <param name="morphBase">The morphological base.</param>
        /// <returns>A new instance of the <see cref="SolutionContent"/> class.</returns>
        public static SolutionContent Create(int id, byte[] parameters, MorphBase morphBase)
        {
            return new()
            {
                Id = id,
                Parameters = ParameterFactory.Clone(parameters),
                Base = morphBase
            };
        }

        /// <summary>
        /// Create a new instance of the <see cref="SolutionContent"/> class.
        /// </summary>
        /// <param name="parameters">Morphological parameters.</param>
        /// <param name="morphBase">The morphological base.</param>
        /// <param name="isVirtual">The solution is virtual (doesn't exist in the live language) or not.</param>
        /// <returns>A new instance of the <see cref="SolutionContent"/> class.</returns>
        public static SolutionContent Create(byte[] parameters, MorphBase morphBase, bool isVirtual)
        {
            return new()
            {
                Parameters = ParameterFactory.Clone(parameters),
                Base = morphBase,
                IsVirtual = isVirtual
            };
        }

        /// <summary>
        /// Create a new instance of the <see cref="SolutionContent"/> class.
        /// </summary>
        /// <param name="id">The ID.</param>
        /// <param name="parameters">Morphological parameters.</param>
        /// <param name="morphBase">The morphological base.</param>
        /// <param name="isVirtual">The solution is virtual (doesn't exist in the live language) or not.</param>
        /// <returns>A new instance of the <see cref="SolutionContent"/> class.</returns>
        public static SolutionContent Create(int id, byte[] parameters, MorphBase morphBase, bool isVirtual)
        {
            return new()
            {
                Id = id,
                Parameters = ParameterFactory.Clone(parameters),
                Base = morphBase,
                IsVirtual = isVirtual
            };
        }

        /// <summary>
        /// Create a new instance of the <see cref="SolutionContent"/> class.
        /// </summary>
        /// <param name="id">The ID.</param>
        /// <param name="parameters">Morphological parameters.</param>
        /// <param name="morphBase">The morphological base.</param>
        /// <param name="isVirtual">The solution is virtual (doesn't exist in the live language) or not.</param>
        /// <param name="error">The solution error.</param>
        /// <returns>A new instance of the <see cref="SolutionContent"/> class.</returns>
        public static SolutionContent Create(int id, byte[] parameters, MorphBase morphBase, bool isVirtual, SolutionError error)
        {
            return new()
            {
                Id = id,
                Parameters = ParameterFactory.Clone(parameters),
                Base = morphBase,
                IsVirtual = isVirtual,
                Error = error
            };
        }

        /// <summary>
        /// Clones the <see cref="SolutionContent"/> class.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <returns>A new instance of the <see cref="SolutionContent"/> class.</returns>
        public static SolutionContent Clone(SolutionContent source)
        {
            return new()
            {
                Id = source.Id,
                Parameters = ParameterFactory.Clone(source.Parameters),
                Base = source.Base,
                IsVirtual = source.IsVirtual,
                Error = source.Error
            };
        }

        /// <summary>
        /// Clones the <see cref="SolutionContent"/> class and replace the solution error.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="error">The solution error.</param>
        /// <returns>A new instance of the <see cref="SolutionContent"/> class.</returns>
        public static SolutionContent Clone(SolutionContent source, SolutionError error)
        {
            return new()
            {
                Id = source.Id,
                Parameters = ParameterFactory.Clone(source.Parameters),
                Base = source.Base,
                IsVirtual = source.IsVirtual,
                Error = error
            };
        }

        /// <summary>
        /// Clones the <see cref="SolutionContent"/> class and replace morphological parameters.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="parameters">Morphological parameters.</param>
        /// <returns>A new instance of the <see cref="SolutionContent"/> class.</returns>
        public static SolutionContent Clone(SolutionContent source, byte[] parameters)
        {
            return new()
            {
                Id = source.Id,
                Parameters = parameters,
                Base = source.Base,
                IsVirtual = source.IsVirtual,
                Error = source.Error
            };
        }
    }
}
