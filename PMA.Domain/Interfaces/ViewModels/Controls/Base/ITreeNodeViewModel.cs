// <copyright file="ITreeNodeViewModel.cs" company="Andrey Pospelov">
//     Copyright 2017-2021 Andrey Pospelov. All rights reserved.
// </copyright>

using PMA.Domain.Interfaces.ViewModels.Base;
using System.Windows.Input;

namespace PMA.Domain.Interfaces.ViewModels.Controls.Base
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ITreeNodeViewModel"/> interfacing class.
    /// </summary>
    public interface ITreeNodeViewModel : IViewModel
    {
        /// <summary>
        /// Gets a node text.
        /// </summary>
        string Text { get; }

        /// <summary>
        /// Gets a node tooltip text.
        /// </summary>
        string ToolTipText { get; }

        /// <summary>
        /// Gets a sandhi text.
        /// </summary>
        string SandhiText { get; }

        /// <summary>
        /// Gets a sandhi line.
        /// </summary>
        string SandhiLine { get; }

        /// <summary>
        /// Gets a tag object.
        /// </summary>
        object Tag { get; }

        /// <summary>
        /// Gets whether tree node has child nodes.
        /// </summary>
        bool HasChildren { get; }

        /// <summary>
        /// Gets a parent tree node view model.
        /// </summary>
        ITreeNodeViewModel Parent { get; }

        /// <summary>
        /// Gets a command to select node.
        /// </summary>
        ICommand SelectCommand { get; }
    }
}
