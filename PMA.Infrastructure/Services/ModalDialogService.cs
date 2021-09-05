// <copyright file="ModalDialogService.cs" company="Andrey Pospelov">
//     Copyright 2017-2021 Andrey Pospelov. All rights reserved.
// </copyright>

using Microsoft.Extensions.Logging;
using PMA.Domain.Enums;
using PMA.Domain.EventArguments;
using PMA.Domain.Interfaces.Services;
using PMA.Infrastructure.Services.Base;
using PMA.Utils.Exceptions;
using System;
using System.Linq;

namespace PMA.Infrastructure.Services
{
    /// <summary>
    /// The modal dialog service class.
    /// </summary>
    public sealed class ModalDialogService : ServiceBase<ModalDialogService>, IModalDialogService
    {
        /// <summary>
        /// Initializes the new instance of <see cref="ModalDialogService"/> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        public ModalDialogService(ILogger<ModalDialogService> logger) : base(logger)
        {
        }

        #region Implementation of IModalDialogService

        /// <summary>
        ///  The event that the modal dialog was shown.
        /// </summary>
        public event EventHandler<ModalDialogEventArgs> ModalDialogShown;

        /// <summary>
        /// Presses a modal dialog button.
        /// </summary>
        /// <param name="button">A pressed button type.</param>
        public void PressButton(ModalButtonType button)
        {
            if (_afterHideCallback is null) return;

            _afterHideCallback.Invoke(button);
            _afterHideCallback = null;
        }

        /// <summary>
        /// Shows an information modal dialog.
        /// </summary>
        /// <param name="title">The modal dialog title.</param>
        /// <param name="message">The modal dialog message.</param>
        public void ShowInformationModalDialog(string title, string message) => ShowModalDialog(title, message, ModalDialogType.Information, new[] { ModalButtonType.Ok });

        /// <summary>
        /// Shows an error modal dialog.
        /// </summary>
        /// <param name="title">The modal dialog title.</param>
        /// <param name="message">The modal dialog message.</param>
        public void ShowErrorModalDialog(string title, string message) => ShowModalDialog(title, message, ModalDialogType.Error, new[] { ModalButtonType.Ok });

        /// <summary>
        /// Shows a modal dialog.
        /// </summary>
        /// <param name="title">The modal dialog title.</param>
        /// <param name="message">The modal dialog message.</param>
        /// <param name="type">The modal dialog type.</param>
        /// <param name="buttons">The collection of modal buttons.</param>
        /// <param name="afterHideCallback">The action after hide modal dialog.</param>
        public void ShowModalDialog(string title, string message, ModalDialogType type, ModalButtonType[] buttons, Action<ModalButtonType> afterHideCallback = null)
        {
            if (buttons is null || buttons.Length == 0)
            {
                throw new CustomException("There must be at least one button");
            }

            if (buttons.Distinct().Count() != buttons.Length)
            {
                throw new CustomException($"All buttons must be different: {string.Join(",", buttons)}");
            }

            _afterHideCallback = afterHideCallback;

            ModalDialogShown?.Invoke(this, new ModalDialogEventArgs
            {
                Title = title,
                Message = message,
                Type = type,
                Buttons = buttons
            });
        }

        #endregion

        #region Private Fields

        /// <summary>
        /// The current action
        /// </summary>
        private Action<ModalButtonType> _afterHideCallback;

        #endregion
    }
}
