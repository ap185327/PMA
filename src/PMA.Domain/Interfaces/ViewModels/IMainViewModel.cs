// <copyright file="IMainViewModel.cs" company="Andrey Pospelov">
//     Copyright 2017-2021 Andrey Pospelov. All rights reserved.
// </copyright>

using PMA.Domain.Interfaces.ViewModels.Base;
using System.Windows.Input;

namespace PMA.Domain.Interfaces.ViewModels
{
    /// <summary>
    /// Initializes a new instance of the <see cref="IMainViewModel"/> interfacing class.
    /// </summary>
    public interface IMainViewModel : IViewModel
    {
        /// <summary>
        /// Gets or sets an input text for analysis.
        /// </summary>
        string InputText { get; set; }

        /// <summary>
        /// Gets an input watermark.
        /// </summary>
        string InputWatermark { get; }

        /// <summary>
        /// Gets a status label text.
        /// </summary>
        string StatusLabel { get; }

        /// <summary>
        /// Gets or sets a chronological layer for calculating the rating of solutions.
        /// </summary>
        int Layer { get; set; }

        /// <summary>
        /// Gets or sets a maximum depth of wordform analysis according to morphological rules.
        /// </summary>
        int MaxDepthLevel { get; set; }

        /// <summary>
        /// Gets or sets auto symbol replace option: aa -> ā, ii -> ī, etc.
        /// </summary>
        bool AutoSymbolReplace { get; set; }

        /// <summary>
        /// Gets whether the execute command is disabled.
        /// </summary>
        bool ExecuteCommandDisabled { get; }

        /// <summary>
        /// Gets the command to start or stop analysis.
        /// </summary>
        ICommand ExecuteCommand { get; }
    }
}
