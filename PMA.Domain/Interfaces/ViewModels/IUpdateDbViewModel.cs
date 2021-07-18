// <copyright file="IUpdateDbViewModel.cs" company="Andrey Pospelov">
//     Copyright 2017-2021 Andrey Pospelov. All rights reserved.
// </copyright>

using PMA.Domain.Interfaces.ViewModels.Base;
using System.Windows.Input;

namespace PMA.Domain.Interfaces.ViewModels
{
    /// <summary>
    /// Initializes a new instance of the <see cref="IUpdateDbViewModel"/> interfacing class.
    /// </summary>
    public interface IUpdateDbViewModel : IViewModel
    {
        /// <summary>
        /// Gets or sets whether the sandhi group database table is checked or not.
        /// </summary>
        bool IsSandhiGroupDbTableChecked { get; set; }

        /// <summary>
        /// Gets or sets whether the sandhi rule database table is checked or not.
        /// </summary>
        bool IsSandhiRuleDbTableChecked { get; set; }

        /// <summary>
        /// Gets or sets whether the morphological rule database table is checked or not.
        /// </summary>
        bool IsMorphRuleDbTableChecked { get; set; }

        /// <summary>
        /// Gets or sets whether the morphological combination database table is checked or not.
        /// </summary>
        bool IsMorphCombinationDbTableChecked { get; set; }

        /// <summary>
        /// Gets or sets a file path with PMA tables.
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
