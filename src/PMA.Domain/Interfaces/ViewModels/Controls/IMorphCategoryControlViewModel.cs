// <copyright file="IMorphCategoryControlViewModel.cs" company="Andrey Pospelov">
//     Copyright 2017-2021 Andrey Pospelov. All rights reserved.
// </copyright>

using PMA.Domain.Interfaces.ViewModels.Base;
using System.Collections.Generic;

namespace PMA.Domain.Interfaces.ViewModels.Controls
{
    /// <summary>
    /// Initializes a new instance of the <see cref="IMorphCategoryControlViewModel"/> interfacing class.
    /// </summary>
    public interface IMorphCategoryControlViewModel : IViewModel
    {
        /// <summary>
        /// Gets a category name.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Gets a selected property terms for the category.
        /// </summary>
        string Terms { get; }

        /// <summary>
        /// Gets the collection of property view models.
        /// </summary>
        IEnumerable<IMorphPropertyControlViewModel> Properties { get; }
    }
}
