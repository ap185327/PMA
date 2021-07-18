// <copyright file="IViewModel.cs" company="Andrey Pospelov">
//     Copyright 2017-2021 Andrey Pospelov. All rights reserved.
// </copyright>

using PMA.Domain.EventArguments;
using System;
using System.ComponentModel;
using System.Windows.Input;

namespace PMA.Domain.Interfaces.ViewModels.Base
{
    /// <summary>
    /// Initializes a new instance of the <see cref="IViewModel"/> interfacing class.
    /// </summary>
    public interface IViewModel : INotifyPropertyChanged
    {
        /// <summary>
        /// Event to show a modal dialog.
        /// </summary>
        event EventHandler<ModalDialogEventArgs> ShowModalDialog;

        /// <summary>
        /// Event to hide a modal dialog.
        /// </summary>
        event EventHandler HideModalDialog;

        /// <summary>
        /// Gets a command that presses a modal dialog button.
        /// </summary>
        ICommand PressModalDialogButtonCommand { get; }

        /// <summary>
        /// Gets a value indicating whether the view model is busy.
        /// </summary>
        bool IsBusy { get; }

        /// <summary>
        /// Gets a value indicating whether the view model is shown.
        /// </summary>
        bool IsShown { get; }

        /// <summary>
        /// Action when the view appears.
        /// </summary>
        void OnAppearing();

        /// <summary>
        /// Action when the view disappears.
        /// </summary>
        void OnDisappearing();
    }
}
