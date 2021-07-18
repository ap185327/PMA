// <copyright file="SolutionFactory.cs" company="Andrey Pospelov">
//     Copyright 2017-2021 Andrey Pospelov. All rights reserved.
// </copyright>

using PMA.Domain.Models;
using System.Collections.Generic;

namespace PMA.Application.Factories
{
    /// <summary>
    /// The solution factory class.
    /// </summary>
    public static class SolutionFactory
    {
        /// <summary>
        /// Create a new instance of the <see cref="Solution"/> class.
        /// </summary>
        /// <param name="content">The solution content.</param>
        /// <returns>A new instance of the <see cref="Solution"/> class.</returns>
        public static Solution Create(SolutionContent content)
        {
            return new()
            {
                Content = content
            };
        }

        /// <summary>
        /// Create a new instance of the <see cref="Solution"/> class.
        /// </summary>
        /// <param name="content">The solution content.</param>
        /// <param name="rules">The collection of morphological rules.</param>
        /// <param name="sandhi">The collection of sandhi matches.</param>
        /// <returns>A new instance of the <see cref="Solution"/> class.</returns>
        public static Solution Create(SolutionContent content, List<MorphRule> rules, List<SandhiMatch> sandhi)
        {
            return new()
            {
                Content = content,
                Rules = rules,
                Sandhi = sandhi
            };
        }

        /// <summary>
        /// Create a new instance of the <see cref="Solution"/> class.
        /// </summary>
        /// <param name="content">The solution content.</param>
        /// <param name="rule">The morphological rules.</param>
        /// <returns>A new instance of the <see cref="Solution"/> class.</returns>
        public static Solution Create(SolutionContent content, MorphRule rule)
        {
            return new()
            {
                Content = content,
                Rules = new List<MorphRule> { rule }
            };
        }

        /// <summary>
        /// Create a new instance of the <see cref="Solution"/> class.
        /// </summary>
        /// <param name="content">The solution content.</param>
        /// <param name="rule">The morphological rules.</param>
        /// <param name="sandhi">The sandhi matches.</param>
        /// <returns>A new instance of the <see cref="Solution"/> class.</returns>
        public static Solution Create(SolutionContent content, MorphRule rule, SandhiMatch sandhi)
        {
            return new()
            {
                Content = content,
                Rules = new List<MorphRule> { rule },
                Sandhi = new List<SandhiMatch> { sandhi }
            };
        }

        /// <summary>
        /// Clones the <see cref="Solution"/> class.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <returns>A new instance of the <see cref="Solution"/> class.</returns>
        public static Solution Clone(Solution source)
        {
            var solution = new Solution
            {
                Content = source.Content,
                Original = source.Original,
                CollapseRating = source.CollapseRating,
                Rating = source.Rating,
                Rules = source.Rules != null ? new List<MorphRule>(source.Rules) : null,
                Sandhi = source.Sandhi != null ? new List<SandhiMatch>(source.Sandhi) : null
            };

            if (source.Left != null)
            {
                solution.Left = WordFormFactory.Clone(source.Left);
            }

            if (source.Right != null)
            {
                solution.Right = WordFormFactory.Clone(source.Right);
            }

            return solution;
        }

        /// <summary>
        /// Clones the <see cref="Solution"/> class and replace left and right parts.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="newLeftWordForm">The new left wordform.</param>
        /// <param name="newRightWordForm">The new right wordform.</param>
        /// <returns>A new instance of the <see cref="Solution"/> class.</returns>
        public static Solution Clone(Solution source, WordForm newLeftWordForm, WordForm newRightWordForm)
        {
            return new()
            {
                Content = source.Content,
                Original = source.Original,
                CollapseRating = source.CollapseRating,
                Rating = source.Rating,
                Rules = source.Rules != null ? new List<MorphRule>(source.Rules) : null,
                Sandhi = source.Sandhi != null ? new List<SandhiMatch>(source.Sandhi) : null,
                Left = newLeftWordForm,
                Right = newRightWordForm
            };
        }
    }
}
