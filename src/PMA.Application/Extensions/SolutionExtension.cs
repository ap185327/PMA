// <copyright file="SolutionExtension.cs" company="Andrey Pospelov">
//     Copyright 2017-2021 Andrey Pospelov. All rights reserved.
// </copyright>

using PMA.Application.Factories;
using PMA.Domain.Enums;
using PMA.Domain.Models;
using System;
using System.Linq;
using System.Text;

namespace PMA.Application.Extensions
{
    /// <summary>
    /// Defines method extensions for solution.
    /// </summary>
    public static class SolutionExtension
    {
        /// <summary>
        /// Gets the unique key of the solution with parameters.
        /// </summary>
        /// <param name="solution">This solution.</param>
        /// <returns>The unique key of the solution with parameters.</returns>
        public static string GetKey(this Solution solution)
        {
            return solution.Content.Id + ";" + string.Join(";", solution.Content.Parameters) + ";" + (int)solution.Content.Base + ";" + solution.Left?.Entry + ";" + solution.Right?.Entry + ";" + solution.Content.IsVirtual;
        }

        /// <summary>
        /// Gets the unique key of the solution with parameters.
        /// </summary>
        /// <param name="solution">This solution.</param>
        /// <param name="left">The left part of solution.</param>
        /// <param name="right">The right part of solution.</param>
        /// <param name="sandhiMatch">The sandhi match.</param>
        /// <returns>The unique key of the solution with parameters.</returns>
        public static string GetUniqKey(this Solution solution, string left, string right, SandhiMatch sandhiMatch)
        {
            var stringBuilder = new StringBuilder();

            stringBuilder.Append(solution.Rules[0].EntityId)
                .Append(sandhiMatch.SandhiExpression)
                .Append(";")
                .Append(solution.Content.Id)
                .Append(";")
                .Append(string.Join(";", solution.Content.Parameters))
                .Append(";")
                .Append((int)solution.Content.Base)
                .Append(";")
                .Append(left)
                .Append(";")
                .Append(right);

            return stringBuilder.ToString();
        }

        /// <summary>
        /// Gets the unique key of the solution without parameters.
        /// </summary>
        /// <param name="solution">This solution.</param>
        /// <returns>The unique key of the solution without parameters.</returns>
        public static string GetSimpleKey(this Solution solution)
        {
            return solution.Content.Id + ";" + (int)solution.Content.Base + ";" + solution.Left?.Entry + ";" + solution.Right?.Entry + ";" + solution.Content.IsVirtual;
        }

        public static string GetUniqKey(this Solution solution)
        {
            var stringBuilder = new StringBuilder();

            stringBuilder.Append(solution.Content.Id)
                .Append(";")
                .Append(string.Join(";", solution.Content.Parameters))
                .Append(";")
                .Append((int)solution.Content.Base)
                .Append(";")
                .Append(solution.Content.IsVirtual)
                .Append(";")
                .Append((int)solution.Content.Error)
                .Append(";")
                .Append(solution.Original?.GetHashCode())
                .Append(";")
                .Append(solution.CollapseRating)
                .Append(";")
                .Append(solution.Rating)
                .Append(";")
                .Append(solution.Left?.GetUniqKey())
                .Append(";")
                .Append(solution.Right?.GetUniqKey())
                .Append(";");

            if (solution.Rules != null)
            {
                stringBuilder.Append(string.Join(";", solution.Rules?.Select(x => x.EntityId)));
            }

            stringBuilder.Append(";");

            if (solution.Sandhi != null)
            {
                stringBuilder.Append(string.Join(";", solution.Sandhi?.Select(x => x.SandhiExpression)));
            }

            return stringBuilder.ToString();
        }

        /// <summary>
        /// Determines if the solution is complete or not.
        /// </summary>
        /// <param name="solution">This solution.</param>
        /// <returns>The solution is complete or not</returns>
        public static bool IsCompleted(this Solution solution)
        {
            switch (solution.Content.Base)
            {
                case MorphBase.None:
                    {
                        return true;
                    }
                case MorphBase.Left:
                    {
                        return solution.Left != null;
                    }
                case MorphBase.Right:
                case MorphBase.Both:
                    {
                        return solution.Left != null && solution.Right != null;
                    }
                case MorphBase.Unknown:
                    {
                        return false;
                    }
                default:
                    {
                        throw new ArgumentOutOfRangeException();
                    }
            }
        }

        /// <summary>
        /// Gets maximum depth level.
        /// </summary>
        /// <param name="solution">This solution.</param>
        /// <param name="maxDepth">Input maximum depth level.</param>
        /// <returns></returns>
        public static int GetMaxDepthLevel(this Solution solution, int maxDepth = 0)
        {
            maxDepth += 1;

            int leftMaxDepth = maxDepth;
            int rightMaxDepth = maxDepth;

            if (solution.Left?.Solutions != null && solution.Left.Solutions.Count > 0)
            {
                leftMaxDepth = solution.Left.Solutions.Select(child => child.GetMaxDepthLevel(maxDepth)).Max();
            }

            if (solution.Right?.Solutions != null && solution.Right.Solutions.Count > 0)
            {
                rightMaxDepth = solution.Right.Solutions.Select(child => child.GetMaxDepthLevel(maxDepth)).Max();
            }

            return leftMaxDepth > rightMaxDepth ? leftMaxDepth : rightMaxDepth;
        }

        /// <summary>
        /// Gets a morphological entry.
        /// </summary>
        /// <param name="solution">This solution.</param>
        /// <param name="entry">The wordform entry.</param>
        /// <returns>A morphological entry.</returns>
        public static MorphEntry GetMorphEntry(this Solution solution, string entry)
        {
            byte[] parameters = solution.Content.Error == SolutionError.Success
                ? ParameterFactory.Clone(solution.Content.Parameters)
                : ParameterFactory.Create();

            var morphEntry = MorphEntryFactory.Create(solution.Content.Id, entry, parameters, solution.Content.Base, solution.Content.IsVirtual);

            if (solution.Left != null)
            {
                var leftIds = solution.Left.Solutions.Where(x => x.Content.Error == SolutionError.Success)
                    .Select(x => x.Content.Id).Distinct().ToList();

                int leftId = leftIds.Count == 1 ? leftIds.First() : 0;

                morphEntry.Left = MorphEntryFactory.Create(leftId, solution.Left.Entry);
            }

            if (solution.Right is null) return morphEntry;

            var rightIds = solution.Right.Solutions.Where(x => x.Content.Error == SolutionError.Success)
                .Select(x => x.Content.Id).Distinct().ToList();

            int rightId = rightIds.Count == 1 ? rightIds.First() : 0;

            morphEntry.Right = MorphEntryFactory.Create(rightId, solution.Right.Entry);

            return morphEntry;
        }
    }
}
