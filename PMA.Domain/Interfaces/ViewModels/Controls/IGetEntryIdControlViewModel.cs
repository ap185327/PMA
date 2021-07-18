// <copyright file="IGetEntryIdControlViewModel.cs" company="Andrey Pospelov">
//     Copyright 2017-2021 Andrey Pospelov. All rights reserved.
// </copyright>

using PMA.Domain.Interfaces.ViewModels.Base;
using System.Windows.Input;

namespace PMA.Domain.Interfaces.ViewModels.Controls
{
    /// <summary>
    /// Initializes a new instance of the <see cref="IGetEntryIdControlViewModel"/> interfacing class.
    /// </summary>
    public interface IGetEntryIdControlViewModel : IViewModel
    {
        /// <summary>
        /// Gets a morphological entry ID.
        /// </summary>
        int Id { get; }

        /// <summary>
        /// Gets a morphological entry.
        /// </summary>
        string Entry { get; }

        /// <summary>
        /// Gets a morphological parameters.
        /// </summary>
        string Parameters { get; }

        /// <summary>
        /// Gets a morphological base.
        /// </summary>
        string Base { get; }

        /// <summary>
        /// Gets a left morphological entry.
        /// </summary>
        string Left { get; }

        /// <summary>
        /// Gets a right morphological entry.
        /// </summary>
        string Right { get; }

        /// <summary>
        /// Gets whether the morphological entry is virtual (doesn't exist in the live language) or not.
        /// </summary>
        bool IsVirtual { get; }

        /// <summary>
        /// Gets or sets whether the morphological entry is selected or not.
        /// </summary>
        bool IsSelected { get; set; }

        /// <summary>
        /// Gets a command to select the morphological entry.
        /// </summary>
        ICommand SelectCommand { get; }
    }
}
