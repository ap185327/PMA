// <copyright file="IMorphPropertyControlViewModel.cs" company="Andrey Pospelov">
//     Copyright 2017-2021 Andrey Pospelov. All rights reserved.
// </copyright>

using PMA.Domain.Interfaces.ViewModels.Base;
using System.Collections.Generic;

namespace PMA.Domain.Interfaces.ViewModels.Controls
{
    /// <summary>
    /// Initializes a new instance of the <see cref="IMorphPropertyControlViewModel"/> interfacing class.
    /// </summary>
    public interface IMorphPropertyControlViewModel : IViewModel
    {
        /// <summary>
        /// Gets the property index.
        /// </summary>
        int Index { get; }

        /// <summary>
        /// Gets the property category.
        /// </summary>
        string Category { get; }

        /// <summary>
        /// Gets the property name.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Gets the property description.
        /// </summary>
        string Description { get; }

        /// <summary>
        /// Gets or sets the selected term.
        /// </summary>
        string SelectedTerm { get; set; }

        /// <summary>
        /// Gets the collection of alternative property term entries.
        /// </summary>
        IList<string> TermEntries { get; }

        /// <summary>
        /// Gets or sets the collection of property term IDs.
        /// </summary>
        IList<byte> TermIds { get; set; }
    }
}
