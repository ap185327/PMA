// <copyright file="IModalDialogService.cs" company="Andrey Pospelov">
//     Copyright 2017-2021 Andrey Pospelov. All rights reserved.
// </copyright>

using PMA.Domain.Enums;
using PMA.Domain.EventArguments;
using PMA.Domain.Interfaces.Services.Base;
using System;

namespace PMA.Domain.Interfaces.Services
{
    /// <summary>
    /// Initializes a new instance of the <see cref="IModalDialogService"/> interfacing class.
    /// </summary>
    public interface IModalDialogService : IService
    {
        /// <summary>
        ///  The event that the modal dialog was shown.
        /// </summary>
        event EventHandler<ModalDialogEventArgs> ModalDialogShown;

        /// <summary>
        /// Presses a modal dialog button.
        /// </summary>
        /// <param name="button">A pressed button type.</param>
        void PressButton(ModalButtonType button);

        /// <summary>
        /// Shows an information modal dialog.
        /// </summary>
        /// <param name="title">The modal dialog title.</param>
        /// <param name="message">The modal dialog message.</param>
        void ShowInformationModalDialog(string title, string message);

        /// <summary>
        /// Shows an error modal dialog.
        /// </summary>
        /// <param name="title">The modal dialog title.</param>
        /// <param name="message">The modal dialog message.</param>
        void ShowErrorModalDialog(string title, string message);

        /// <summary>
        /// Shows a modal dialog.
        /// </summary>
        /// <param name="title">The modal dialog title.</param>
        /// <param name="message">The modal dialog message.</param>
        /// <param name="type">The modal dialog type.</param>
        /// <param name="buttons">The collection of modal buttons.</param>
        /// <param name="afterHideCallback">The action after hide modal dialog.</param>
        void ShowModalDialog(string title, string message, ModalDialogType type, ModalButtonType[] buttons, Action<ModalButtonType> afterHideCallback = null);
    }
}
