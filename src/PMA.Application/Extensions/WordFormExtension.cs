// <copyright file="WordFormExtension.cs" company="Andrey Pospelov">
//     Copyright 2017-2021 Andrey Pospelov. All rights reserved.
// </copyright>

using PMA.Domain.Enums;
using PMA.Domain.Models;
using System;
using System.Linq;
using System.Text;

namespace PMA.Application.Extensions
{
    /// <summary>
    /// Defines method extensions for wordform.
    /// </summary>
    public static class WordFormExtension
    {
        /// <summary>
        /// Gets a maximum wordform rating.
        /// </summary>
        /// <param name="wordForm">This wordform.</param>
        /// <returns>A maximum wordform rating.</returns>
        public static double GetMaxRating(this WordForm wordForm)
        {
            return wordForm.Solutions.Select(x => x.Rating).Max();
        }

        /// <summary>
        /// Gets a minimum wordform rating.
        /// </summary>
        /// <param name="wordForm">This wordform.</param>
        /// <returns>A minimum wordform rating.</returns>
        public static double GetMinRating(this WordForm wordForm)
        {
            return wordForm.Solutions.Select(x => x.Rating).Min();
        }

        /// <summary>
        /// Gets the total wordform rating.
        /// </summary>
        /// <param name="wordForm">The wordform.</param>
        /// <returns>The total wordform rating.</returns>
        public static double GetTotalRating(this WordForm wordForm)
        {
            return wordForm.Solutions.Sum(x => x.Rating);
        }

        /// <summary>
        /// Counts all solutions of the wordform.
        /// </summary>
        /// <param name="wordForm">The wordform.</param>
        /// <returns>Total solution count.</returns>
        public static long TotalSolutionCount(this WordForm wordForm)
        {
            if (wordForm.Solutions is null)
            {
                return 0;
            }

            long count = wordForm.Solutions.Count;

            foreach (var solution in wordForm.Solutions)
            {
                if (solution.Left != null)
                {
                    count += TotalSolutionCount(solution.Left);
                }
                if (solution.Right != null)
                {
                    count += TotalSolutionCount(solution.Right);
                }
            }

            return count;
        }

        /// <summary>
        /// Gets an unique key of the wordform.
        /// </summary>
        /// <param name="wordForm">The wordform.</param>
        /// <returns>The unique key of the wordform</returns>
        public static long GetKey(this WordForm wordForm)
        {
            long key = wordForm.Entry.Length;

            if (wordForm.Solutions is null)
            {
                return key;
            }

            double totalRating = GetTotalRating(wordForm);

            foreach (var solution in wordForm.Solutions)
            {
                for (int i = 0; i < solution.Content.Parameters.Length; i++)
                {
                    key += solution.Content.Parameters[i];

                    if (solution.Rating != 0 && totalRating != 0)
                    {
                        key += (int)(Math.Round(solution.Rating / totalRating, 2) * 100);
                    }
                }

                key += Convert.ToInt32(solution.Content.IsVirtual) + (int)solution.Content.Base + solution.Content.Id + (int)solution.Content.Error;

                if (solution.Rules != null)
                {
                    key = solution.Rules.Aggregate(key, (current, rule) => current + rule.EntityId);
                }

                if (solution.Sandhi != null)
                {
                    key = solution.Sandhi.Aggregate(key, (current, match) => current + match.SandhiExpression.Length);
                }

                if (solution.Left != null)
                {
                    key += GetKey(solution.Left);
                }
                if (solution.Right != null)
                {
                    key += GetKey(solution.Right);
                }
            }

            return key;
        }

        public static string GetUniqKey(this WordForm wordForm)
        {
            var stringBuilder = new StringBuilder();

            stringBuilder.Append(wordForm.Entry);

            for (int i = 0; i < wordForm.Solutions.Count; i++)
            {
                stringBuilder.Append(wordForm.Solutions[i].Content.Id)
                    .Append(";");
            }

            return stringBuilder.ToString();
        }

        /// <summary>
        /// The solution is taken from dictionary or not.
        /// </summary>
        /// <param name="wordForm">This wordForm.</param>
        /// <param name="parent">The parent solution.</param>
        /// <returns>The solution is taken from dictionary or not.</returns>
        public static bool IsFromDict(this WordForm wordForm, Solution parent)
        {
            return parent.Original != null || wordForm.Solutions.Any(x => x.Content.Id != 0);
        }

        /// <summary>
        /// Removes all error solutions.
        /// </summary>
        /// <param name="wordForm">This wordForm.</param>
        public static void Clear(this WordForm wordForm)
        {
            wordForm.Solutions = wordForm.Solutions.Where(x => x.Content.Error == SolutionError.Success).ToList();
        }

        /// <summary>
        /// Gets a depth level of the wordForm.
        /// </summary>
        /// <param name="wordForm">This wordForm.</param>
        /// <returns>A depth level of the wordForm.</returns>
        public static MorphEntry GetMorphEntry(this WordForm wordForm)
        {
            if (wordForm.Solutions is null) return null;

            var solutionMorphEntries = wordForm.Solutions.Where(x => x.Content.Error == SolutionError.Success).Select(x => x.GetMorphEntry(wordForm.Entry)).ToList();

            if (!solutionMorphEntries.Any()) return null;

            using var enumerator = solutionMorphEntries.GetEnumerator();
            enumerator.MoveNext();

            var mainMorphEntry = enumerator.Current;

            while (enumerator.MoveNext())
            {
                var morphEntry = enumerator.Current;

                if (mainMorphEntry.Id != 0 && mainMorphEntry.Id != morphEntry.Id)
                {
                    mainMorphEntry.Id = 0;
                }

                for (int i = 0; i < mainMorphEntry.Parameters.Length; i++)
                {
                    if (mainMorphEntry.Parameters[i] != 0 && mainMorphEntry.Parameters[i] != morphEntry.Parameters[i])
                    {
                        mainMorphEntry.Parameters[i] = 0;
                    }
                }

                if (mainMorphEntry.Base != MorphBase.Unknown && mainMorphEntry.Base != morphEntry.Base)
                {
                    mainMorphEntry.Base = MorphBase.Unknown;
                }

                if (mainMorphEntry.Left != null)
                {
                    if (morphEntry.Left is null || mainMorphEntry.Left.Entry != morphEntry.Left.Entry)
                    {
                        mainMorphEntry.Left = null;
                    }
                    else if (mainMorphEntry.Left.Id != 0 && mainMorphEntry.Left.Id != morphEntry.Left.Id)
                    {
                        mainMorphEntry.Left.Id = 0;
                    }
                }

                if (mainMorphEntry.Right is null) continue;

                if (morphEntry.Right is null || mainMorphEntry.Right.Entry != morphEntry.Right.Entry)
                {
                    mainMorphEntry.Right = null;
                }
                else if (mainMorphEntry.Right.Id != 0 && mainMorphEntry.Right.Id != morphEntry.Right.Id)
                {
                    mainMorphEntry.Right.Id = 0;
                }
            }

            return mainMorphEntry;
        }
    }
}
