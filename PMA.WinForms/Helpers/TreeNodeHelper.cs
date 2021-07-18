// <copyright file="TreeNodeHelper.cs" company="Andrey Pospelov">
//     Copyright 2017-2021 Andrey Pospelov. All rights reserved.
// </copyright>

using PMA.Domain.Enums;
using PMA.Domain.Interfaces.ViewModels.Controls;
using System.Drawing;
using System.Windows.Forms;

namespace PMA.WinForms.Helpers
{
    /// <summary>
    /// The tree node helper.
    /// </summary>
    internal static class TreeNodeHelper
    {
        /// <summary>
        /// Gets a tree node.
        /// </summary>
        /// <param name="solutionTreeNodeViewModel">The solution tree node view model.</param>
        /// <param name="font">The tree node view font.</param>
        /// <returns>A tree node.</returns>
        public static TreeNode GetTreeNode(ISolutionTreeNodeViewModel solutionTreeNodeViewModel, Font font)
        {
            var solutionNode = new TreeNode
            {
                Text = solutionTreeNodeViewModel.Text,
                ToolTipText = solutionTreeNodeViewModel.ToolTipText,
                Tag = solutionTreeNodeViewModel,
                ForeColor = GetColor(solutionTreeNodeViewModel.RelativeRating, solutionTreeNodeViewModel.IsVirtual, solutionTreeNodeViewModel.Error)
            };

            if (solutionTreeNodeViewModel.IsFromDict)
            {
                solutionNode.NodeFont = new Font(font, FontStyle.Bold);
            }

            return solutionNode;
        }

        /// <summary>
        /// Gets a tree node.
        /// </summary>
        /// <param name="wordFormTreeNodeViewModel">The wordform tree node view model.</param>
        /// <returns>A tree node.</returns>
        public static TreeNode GetTreeNode(IWordFormTreeNodeViewModel wordFormTreeNodeViewModel)
        {
            return new TreeNode
            {
                Text = wordFormTreeNodeViewModel.Text,
                ToolTipText = wordFormTreeNodeViewModel.ToolTipText,
                Tag = wordFormTreeNodeViewModel
            };
        }

        /// <summary>
        /// Gets a color.
        /// </summary>
        /// <param name="rating">The rating value.</param>
        /// <param name="isVirtual">The value indicating whether the morphological entry is virtual.</param>
        /// <param name="error">The solution error.</param>
        /// <returns>A color.</returns>
        private static Color GetColor(double rating, bool isVirtual, SolutionError error)
        {
            if (error != SolutionError.Success)
            {
                return Color.DarkGray;
            }

            return rating switch
            {
                0 => isVirtual ? Color.FromArgb(255, 127, 127) : Color.FromArgb(255, 0, 0),
                1 => isVirtual ? Color.FromArgb(127, 191, 127) : Color.FromArgb(0, 127, 0),
                _ => isVirtual
                    ? Color.FromArgb((int)((1 - rating) * 127) + 127, (int)(rating * 63) + 127, 127)
                    : Color.FromArgb((int)((1 - rating) * 255), (int)(rating * 127), 0)
            };
        }
    }
}
