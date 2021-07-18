// <copyright file="IGetEntryIdViewModel.cs" company="Andrey Pospelov">
//     Copyright 2017-2021 Andrey Pospelov. All rights reserved.
// </copyright>

using PMA.Domain.Interfaces.ViewModels.Base;
using PMA.Domain.Interfaces.ViewModels.Controls;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace PMA.Domain.Interfaces.ViewModels
{
    /// <summary>
    /// Initializes a new instance of the <see cref="IGetEntryIdViewModel"/> interfacing class.
    /// </summary>
    public interface IGetEntryIdViewModel : IViewModel
    {
        /// <summary>
        /// Gets a morphological entry.
        /// </summary>
        string Entry { get; }

        /// <summary>
        /// Gets a collection of morphological entries.
        /// </summary>
        ObservableCollection<IGetEntryIdControlViewModel> MorphEntries { get; }

        /// <summary>
        /// Gets a command to delete the selected morphological entry.
        /// </summary>
        ICommand DeleteCommand { get; }
    }
}
