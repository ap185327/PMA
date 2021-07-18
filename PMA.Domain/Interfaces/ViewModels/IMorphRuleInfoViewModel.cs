// <copyright file="IMorphRuleInfoViewModel.cs" company="Andrey Pospelov">
//     Copyright 2017-2021 Andrey Pospelov. All rights reserved.
// </copyright>

using PMA.Domain.Interfaces.ViewModels.Base;
using PMA.Domain.Interfaces.ViewModels.Controls;
using System.Collections.ObjectModel;

namespace PMA.Domain.Interfaces.ViewModels
{
    /// <summary>
    /// Initializes a new instance of the <see cref="IMorphRuleInfoViewModel"/> interfacing class.
    /// </summary>
    public interface IMorphRuleInfoViewModel : IViewModel
    {
        /// <summary>
        /// Gets a collection of morphological rule info item view models.
        /// </summary>
        ObservableCollection<IRuleInfoItemViewModel> MorphRules { get; }

        /// <summary>
        /// Gets a collection of sandhi rule info item view models.
        /// </summary>
        ObservableCollection<IRuleInfoItemViewModel> SandhiRules { get; }
    }
}
