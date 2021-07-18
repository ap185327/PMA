// <copyright file="IRuleInfoItemViewModel.cs" company="Andrey Pospelov">
//     Copyright 2017-2021 Andrey Pospelov. All rights reserved.
// </copyright>

using PMA.Domain.Interfaces.ViewModels.Base;

namespace PMA.Domain.Interfaces.ViewModels.Controls
{
    /// <summary>
    /// Initializes a new instance of the <see cref="IRuleInfoItemViewModel"/> interfacing class.
    /// </summary>
    public interface IRuleInfoItemViewModel : IViewModel
    {
        /// <summary>
        /// Gets a rule ID.
        /// </summary>
        int Id { get; }

        /// <summary>
        /// Gets a rule description.
        /// </summary>
        string Description { get; }
    }
}
