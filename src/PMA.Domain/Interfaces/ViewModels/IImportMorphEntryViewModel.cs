// <copyright file="IImportMorphEntryViewModel.cs" company="Andrey Pospelov">
//     Copyright 2017-2021 Andrey Pospelov. All rights reserved.
// </copyright>

using PMA.Domain.Interfaces.ViewModels.Base;
using System.Windows.Input;

namespace PMA.Domain.Interfaces.ViewModels
{
    /// <summary>
    /// Initializes a new instance of the <see cref="IImportMorphEntryViewModel"/> interfacing class.
    /// </summary>
    public interface IImportMorphEntryViewModel : IViewModel
    {
        /// <summary>
        /// Gets or sets whether the analyze before import is checked or not.
        /// </summary>
        bool IsAnalyzeBeforeImportChecked { get; set; }

        /// <summary>
        /// Gets an analyze progress bar value.
        /// </summary>
        int AnalyzeProgressBarValue { get; }

        /// <summary>
        /// Gets or sets a file path with morphological entries.
        /// </summary>
        string DataFilePath { get; set; }

        /// <summary>
        /// Gets a command to start importing.
        /// </summary>
        ICommand StartCommand { get; }

        /// <summary>
        /// Gets a command to stop importing.
        /// </summary>
        ICommand StopCommand { get; }

        /// <summary>
        /// Gets a command to reset view model values.
        /// </summary>
        ICommand ResetCommand { get; }
    }
}
