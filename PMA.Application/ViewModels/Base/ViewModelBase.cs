// <copyright file="ViewModelBase.cs" company="Andrey Pospelov">
//     Copyright 2017-2021 Andrey Pospelov. All rights reserved.
// </copyright>

using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using PMA.Domain.Enums;
using PMA.Domain.EventArguments;
using PMA.Domain.Interfaces.Locators;
using PMA.Domain.Interfaces.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Windows.Input;

namespace PMA.Application.ViewModels.Base
{
    /// <summary>
    /// The base view model class.
    /// </summary>
    /// <typeparam name="T">Type of the implementation class.</typeparam>
    public abstract class ViewModelBase<T> : ObservableRecipient, IViewModel where T : class
    {
        /// <summary>
        /// The service locator.
        /// </summary>
        protected readonly IServiceLocator ServiceLocator;

        /// <summary>
        /// The mediator.
        /// </summary>
        protected readonly IMediator Mediator;

        /// <summary>
        /// The logger.
        /// </summary>
        protected readonly ILogger<T> Logger;

        /// <summary>
        /// The current modal dialog.
        /// </summary>
        protected ModalDialogName CurrentModalDialog = ModalDialogName.None;

        /// <summary>
        /// Initializes the new instance of <see cref="ViewModelBase{T}"/> class.
        /// </summary>
        /// <param name="serviceLocator">The service locator.</param>
        /// <param name="mediator">The mediator.</param>
        /// <param name="logger">The logger.</param>
        protected ViewModelBase(IServiceLocator serviceLocator, IMediator mediator, ILogger<T> logger)
        {
            ServiceLocator = serviceLocator;
            Mediator = mediator;
            Logger = logger;

            PressModalDialogButtonCommand = new RelayCommand<int>(PressModalDialogButton);
        }

        #region Implementation of IViewModel

        /// <summary>
        /// Event to show a modal dialog.
        /// </summary>
        public event EventHandler<ModalDialogEventArgs> ShowModalDialog;

        /// <summary>
        /// Event to hide a modal dialog.
        /// </summary>
        public event EventHandler HideModalDialog;

        /// <summary>
        /// Gets a command that presses a modal dialog button.
        /// </summary>
        public ICommand PressModalDialogButtonCommand { get; }

        /// <summary>
        ///  Backing field for the IsBusy property.
        /// </summary>
        private bool _isBusy;

        /// <summary>
        /// Gets a value indicating whether the view model is busy.
        /// </summary>
        public bool IsBusy
        {
            get => _isBusy;
            protected set => SetProperty(ref _isBusy, value);
        }

        /// <summary>
        ///  Backing field for the IsShown property.
        /// </summary>
        private bool _isShown;

        /// <summary>
        /// Gets a value indicating whether the view model is shown.
        /// </summary>
        public bool IsShown
        {
            get => _isShown;
            private set => SetProperty(ref _isShown, value);
        }

        /// <summary>
        /// Action when the view appears.
        /// </summary>
        public virtual void OnAppearing()
        {
            IsShown = true;
        }

        /// <summary>
        /// Action when the view disappears.
        /// </summary>
        public virtual void OnDisappearing()
        {
            IsShown = false;
        }

        #endregion

        /// <summary>
        /// Presses a modal dialog button.
        /// </summary>
        /// <param name="modalButtonIndex">A button index.</param>
        protected virtual void PressModalDialogButton(int modalButtonIndex)
        {
            OnHideModalDialog();
        }

        /// <summary>
        /// Invokes the show a modal dialog  event to the view.
        /// </summary>
        /// <param name="title">A modal dialog title.</param>
        /// <param name="message">A modal dialog message.</param>
        /// <param name="type">A modal dialog type.</param>
        protected void OnShowModalDialog(string title, string message, ModalDialogType type = ModalDialogType.None)
        {
            ShowModalDialog?.Invoke(this, new ModalDialogEventArgs
            {
                Title = title,
                Message = message,
                Buttons = new[] { ServiceLocator.TranslateService.Translate("ModalDialog.Buttons.OK") },
                Type = type
            });
        }

        /// <summary>
        /// Invokes the show a modal dialog  event to the view.
        /// </summary>
        /// <param name="title">A modal dialog title.</param>
        /// <param name="message">A modal dialog message.</param>
        /// <param name="buttons">Modal dialog buttons.</param>
        /// <param name="type">A modal dialog type.</param>
        protected void OnShowModalDialog(string title, string message, string[] buttons, ModalDialogType type = ModalDialogType.None)
        {
            ShowModalDialog?.Invoke(this, new ModalDialogEventArgs
            {
                Title = title,
                Message = message,
                Buttons = buttons,
                Type = type
            });
        }

        /// <summary>
        /// Invokes the hide a modal dialog event to the view.
        /// </summary>
        private void OnHideModalDialog()
        {
            CurrentModalDialog = ModalDialogName.None;

            HideModalDialog?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// Shows a dialog with error messages.
        /// </summary>
        /// <param name="errors">The collection of error messages.</param>
        protected void ShowErrorModalDialog(IEnumerable<string> errors)
        {
            CurrentModalDialog = ModalDialogName.MorphEntryIsExist;

            OnShowModalDialog(ServiceLocator.TranslateService.Translate("ViewModelBase.ErrorMessageBoxTitle"),
                ServiceLocator.TranslateService.Translate("ViewModelBase.ErrorMessageBoxText", string.Join("\n", errors)),
                new[]
                {
                    ServiceLocator.TranslateService.Translate("MessageBox.Button.Ok")
                },
                ModalDialogType.Error);
        }
    }
}
