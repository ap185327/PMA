// <copyright file="IOptionViewModel.cs" company="Andrey Pospelov">
//     Copyright 2017-2021 Andrey Pospelov. All rights reserved.
// </copyright>

using PMA.Domain.Interfaces.ViewModels.Base;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace PMA.Domain.Interfaces.ViewModels
{
    /// <summary>
    /// Initializes a new instance of the <see cref="IOptionViewModel"/> interfacing class.
    /// </summary>
    public interface IOptionViewModel : IViewModel
    {
        /// <summary>
        /// Gets or sets a option allows you to select the way how the wordform will be analyzed: False – only successful solutions sorted by rating; True – all solutions, including unsuccessful ones.
        /// </summary>
        bool DebugMode { get; set; }

        /// <summary>
        /// Gets or sets s selected root term index.
        /// </summary>
        int SelectedRootTermIndex { get; set; }

        /// <summary>
        /// Gets a collection of root terms.
        /// </summary>
        IList<string> RootTerms { get; }

        /// <summary>
        /// Gets a collection of available terms.
        /// </summary>
        ObservableCollection<string> AvailableTerms { get; }

        /// <summary>
        /// Gets a collection of shown terms.
        /// </summary>
        ObservableCollection<string> ShownTerms { get; }

        /// <summary>
        /// Gets a frequency rating ratio.
        /// </summary>
        double FreqRatingRatio { get; set; }

        /// <summary>
        /// Gets a command to save settings.
        /// </summary>
        ICommand SaveCommand { get; }

        /// <summary>
        /// Gets a command to add a term to collection of shown terms.
        /// </summary>
        ICommand AddTermCommand { get; }

        /// <summary>
        /// Gets a command to remove a term from collection of shown terms.
        /// </summary>
        ICommand RemoveTermCommand { get; }
    }
}
