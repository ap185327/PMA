// <copyright file="IMorphPropertyViewModel.cs" company="Andrey Pospelov">
//     Copyright 2017-2021 Andrey Pospelov. All rights reserved.
// </copyright>

using PMA.Domain.Enums;
using PMA.Domain.Interfaces.ViewModels.Base;
using PMA.Domain.Interfaces.ViewModels.Controls;
using System.Collections.Generic;
using System.Windows.Input;

namespace PMA.Domain.Interfaces.ViewModels
{
    /// <summary>
    /// Initializes a new instance of the <see cref="IMorphPropertyViewModel"/> interfacing class.
    /// </summary>
    public interface IMorphPropertyViewModel : IViewModel
    {
        /// <summary>
        /// Gets or sets the morphological entry text.
        /// </summary>
        string Entry { get; set; }

        /// <summary>
        /// Gets or sets the left part text of the morphological entry.
        /// </summary>
        string LeftEntry { get; set; }

        /// <summary>
        /// Gets or sets the right part text of the morphological entry.
        /// </summary>
        string RightEntry { get; set; }

        /// <summary>
        /// Gets the input watermark.
        /// </summary>
        string InputWatermark { get; }

        /// <summary>
        /// Gets auto symbol replace option: aa -> ā, ii -> ī, etc.
        /// </summary>
        bool AutoSymbolReplace { get; }

        /// <summary>
        /// Gets the morphological entry ID.
        /// </summary>
        int EntryId { get; }

        /// <summary>
        /// Gets the ID for the left part of the morphological entry.
        /// </summary>
        int LeftId { get; }

        /// <summary>
        /// Gets the ID for the right part of the morphological entry.
        /// </summary>
        int RightId { get; }

        /// <summary>
        /// Gets or sets the morphological base.
        /// </summary>
        MorphBase Base { get; set; }

        /// <summary>
        /// Gets or sets whether the input morphological entry is virtual (doesn't exist in the live language) or not.
        /// </summary>
        bool IsVirtual { get; set; }

        /// <summary>
        /// Gets or sets whether the left part of the morphological entry is used or not.
        /// </summary>
        bool IsLeftChecked { get; set; }

        /// <summary>
        /// Gets or sets whether the right part of the morphological entry is used or not.
        /// </summary>
        bool IsRightChecked { get; set; }

        /// <summary>
        /// Gets or sets whether the entry ID is locked or not.
        /// </summary>
        bool IsLockEntryIdChecked { get; set; }

        /// <summary>
        /// Gets the command to start analysis.
        /// </summary>
        ICommand StartCommand { get; }

        /// <summary>
        /// Gets the command to stop analysis.
        /// </summary>
        ICommand StopCommand { get; }

        /// <summary>
        /// Gets the command to save the morphological entry.
        /// </summary>
        ICommand SaveCommand { get; }

        /// <summary>
        /// Gets the command to delete the morphological entry.
        /// </summary>
        ICommand DeleteCommand { get; }

        /// <summary>
        /// Gets the command to reset values.
        /// </summary>
        ICommand ResetCommand { get; }

        /// <summary>
        /// Gets the command to get the morphological entry ID.
        /// </summary>
        ICommand GetEntryIdCommand { get; }

        /// <summary>
        /// Gets the command to get the left part ID of morphological entry.
        /// </summary>
        ICommand GetLeftIdCommand { get; }

        /// <summary>
        /// Gets the command to get the right part ID of morphological entry.
        /// </summary>
        ICommand GetRightIdCommand { get; }

        /// <summary>
        /// Gets the collection of property view models.
        /// </summary>
        IList<IMorphPropertyControlViewModel> Properties { get; }
    }
}
