// <copyright file="IViewModel.cs" company="Andrey Pospelov">
//     Copyright 2017-2021 Andrey Pospelov. All rights reserved.
// </copyright>

using System.ComponentModel;

namespace PMA.Domain.Interfaces.ViewModels.Base
{
    /// <summary>
    /// Initializes a new instance of the <see cref="IViewModel"/> interfacing class.
    /// </summary>
    public interface IViewModel : INotifyPropertyChanged
    {
        /// <summary>
        /// Gets a value indicating whether the view model is busy.
        /// </summary>
        bool IsBusy { get; }

        /// <summary>
        /// Gets or sets a value indicating whether the current view model is currently active.
        /// </summary>
        bool IsActive { get; set; }
    }
}
