// <copyright file="ViewModelBase.cs" company="Andrey Pospelov">
//     Copyright 2017-2021 Andrey Pospelov. All rights reserved.
// </copyright>

using Microsoft.Extensions.Logging;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Messaging;
using PMA.Domain.Constants;
using PMA.Domain.DataContracts;
using PMA.Domain.Interfaces.Locators;
using PMA.Domain.Interfaces.ViewModels.Base;

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
        /// The logger.
        /// </summary>
        protected readonly ILogger<T> Logger;

        /// <summary>
        /// Initializes the new instance of <see cref="ViewModelBase{T}"/> class.
        /// </summary>
        /// <param name="serviceLocator">The service locator.</param>
        /// <param name="logger">The logger.</param>
        /// <param name="messenger">The messenger.</param>
        protected ViewModelBase(IServiceLocator serviceLocator, ILogger<T> logger, IMessenger messenger) : base(messenger)
        {
            ServiceLocator = serviceLocator;
            Logger = logger;
        }

        #region Implementation of IViewModel

        /// <summary>
        /// Gets a value indicating whether the view model is busy.
        /// </summary>
        public bool IsBusy
        {
            get => _isBusy;
            protected set => SetProperty(ref _isBusy, value);
        }

        #endregion

        #region Private Fields

        /// <summary>
        ///  Backing field for the IsBusy property.
        /// </summary>
        private bool _isBusy;

        #endregion

        #region Private Methods

        /// <summary>
        /// Gets whether the operation result is success.
        /// </summary>
        /// <typeparam name="TResult">Type of the result.</typeparam>
        /// <param name="result">The operation result.</param>
        /// <returns>True if the operation result is success; otherwise False.</returns>
        protected bool IsOperationResultSuccess<TResult>(OperationResult<TResult> result)
        {
            if (result.Success) return true;

            foreach (string logMessage in result.Messages)
            {
                Logger.LogError(logMessage);
            }

            string title = ServiceLocator.TranslateService.Translate(ModalDialogTitleConstants.Error);
            string message = ServiceLocator.TranslateService.Translate(ModalDialogMessageConstants.Error, string.Join("; ", result.Messages));

            ServiceLocator.ModalDialogService.ShowErrorModalDialog(title, message);

            return false;
        }

        /// <summary>
        /// Gets whether the operation result is success.
        /// </summary>
        /// <param name="result">The operation result.</param>
        /// <returns>True if the operation result is success; otherwise False.</returns>
        protected bool IsOperationResultSuccess(OperationResult result)
        {
            if (result.Success) return true;

            foreach (string logMessage in result.Messages)
            {
                Logger.LogError(logMessage);
            }

            string title = ServiceLocator.TranslateService.Translate(ModalDialogTitleConstants.Error);
            string message = ServiceLocator.TranslateService.Translate(ModalDialogMessageConstants.Error, string.Join("; ", result.Messages));

            ServiceLocator.ModalDialogService.ShowErrorModalDialog(title, message);

            return false;
        }

        #endregion
    }
}
