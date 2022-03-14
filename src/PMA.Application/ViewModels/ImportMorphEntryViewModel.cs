// <copyright file="ImportMorphEntryViewModel.cs" company="Andrey Pospelov">
//     Copyright 2017-2021 Andrey Pospelov. All rights reserved.
// </copyright>

using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.Toolkit.Mvvm.Input;
using Microsoft.Toolkit.Mvvm.Messaging;
using PMA.Application.ViewModels.Base;
using PMA.Domain.Enums;
using PMA.Domain.InputPorts;
using PMA.Domain.Interfaces.Interactors.Primary;
using PMA.Domain.Interfaces.Locators;
using PMA.Domain.Interfaces.ViewModels;
using PMA.Domain.Notifications;
using PMA.Utils.Extensions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;

namespace PMA.Application.ViewModels
{
    /// <summary>
    /// The import morphological entry view model class.
    /// </summary>
    public sealed class ImportMorphEntryViewModel : ViewModelBase<ImportMorphEntryViewModel>, IImportMorphEntryViewModel, INotificationHandler<ImportMorphEntryNotification>
    {
        /// <summary>
        /// The import morphological entry view model interactor.
        /// </summary>
        private readonly IImportMorphEntryInteractor _interactor;

        /// <summary>
        /// Initializes the new instance of <see cref="ImportMorphEntryViewModel"/> class.
        /// </summary>
        /// <param name="interactor"></param>
        /// <param name="serviceLocator">The service locator.</param>
        /// <param name="logger">The logger.</param>
        /// <param name="messenger">The messenger.</param>
        public ImportMorphEntryViewModel(IImportMorphEntryInteractor interactor, IServiceLocator serviceLocator, ILogger<ImportMorphEntryViewModel> logger, IMessenger messenger) : base(serviceLocator, logger, messenger)
        {
            _interactor = interactor;

            Logger.LogInit();

            StartCommand = new AsyncRelayCommand(StartAsync);
            StopCommand = new RelayCommand(Stop);
            ResetCommand = new RelayCommand(Reset);
        }

        #region Overrides of ObservableRecipient

        /// <summary>
        /// Raised whenever the <see cref="P:Microsoft.Toolkit.Mvvm.ComponentModel.ObservableRecipient.IsActive" /> property is set to <see langword="true" />.
        /// Use this method to register to messages and do other initialization for this instance.
        /// </summary>
        /// <remarks>
        /// The base implementation registers all messages for this recipients that have been declared
        /// explicitly through the <see cref="T:Microsoft.Toolkit.Mvvm.Messaging.IRecipient`1" /> interface, using the default channel.
        /// For more details on how this works, see the <see cref="M:Microsoft.Toolkit.Mvvm.Messaging.IMessengerExtensions.RegisterAll(Microsoft.Toolkit.Mvvm.Messaging.IMessenger,System.Object)" /> method.
        /// If you need more fine tuned control, want to register messages individually or just prefer
        /// the lambda-style syntax for message registration, override this method and register manually.
        /// </remarks>
        protected override void OnActivated()
        {
            base.OnActivated();

            _cancellationTokenSource = new CancellationTokenSource();

            IsAnalyzeBeforeImportChecked = ServiceLocator.SettingService.GetValue<bool>("ImportMorphEntryViewModel.IsAnalyzeBeforeImportChecked");
            DataFilePath = ServiceLocator.SettingService.GetValue<string>("ImportMorphEntryViewModel.DataFilePath");
        }

        /// <summary>
        /// Raised whenever the <see cref="P:Microsoft.Toolkit.Mvvm.ComponentModel.ObservableRecipient.IsActive" /> property is set to <see langword="false" />.
        /// Use this method to unregister from messages and do general cleanup for this instance.
        /// </summary>
        /// <remarks>
        /// The base implementation unregisters all messages for this recipient. It does so by
        /// invoking <see cref="M:Microsoft.Toolkit.Mvvm.Messaging.IMessenger.UnregisterAll(System.Object)" />, which removes all registered
        /// handlers for a given subscriber, regardless of what token was used to register them.
        /// That is, all registered handlers across all subscription channels will be removed.
        /// </remarks>
        protected override void OnDeactivated()
        {
            base.OnDeactivated();

            if (!_cancellationTokenSource.IsCancellationRequested)
            {
                _cancellationTokenSource.Cancel();
            }

            _cancellationTokenSource = null;

            ServiceLocator.SettingService.SetValue("ImportMorphEntryViewModel.IsAnalyzeBeforeImportChecked", IsAnalyzeBeforeImportChecked);
            ServiceLocator.SettingService.SetValue("ImportMorphEntryViewModel.DataFilePath", DataFilePath);
        }

        #endregion

        #region Implementation of IImportMorphEntryViewModel

        /// <summary>
        /// Gets or sets whether the analyze before import is checked or not.
        /// </summary>
        public bool IsAnalyzeBeforeImportChecked
        {
            get => _isAnalyzeBeforeImportChecked;
            set => SetProperty(ref _isAnalyzeBeforeImportChecked, value);
        }

        /// <summary>
        /// Gets an analyze progress bar value.
        /// </summary>
        public int AnalyzeProgressBarValue
        {
            get => _analyzeProgressBarValue;
            private set => SetProperty(ref _analyzeProgressBarValue, value);
        }

        /// <summary>
        /// Gets or sets a file path with morphological entries.
        /// </summary>
        public string DataFilePath
        {
            get => _dataFilePath;
            set => SetProperty(ref _dataFilePath, value);
        }

        /// <summary>
        /// Gets a command to start importing.
        /// </summary>
        public ICommand StartCommand { get; }

        /// <summary>
        /// Gets a command to stop importing.
        /// </summary>
        public ICommand StopCommand { get; }

        /// <summary>
        /// Gets a command to reset view model values.
        /// </summary>
        public ICommand ResetCommand { get; }

        #endregion

        #region Implementation of INotificationHandler<in ImportMorphEntryNotification>

        /// <summary>Handles a notification</summary>
        /// <param name="notification">The notification</param>
        /// <param name="cancellationToken">Cancellation token</param>
        public Task Handle(ImportMorphEntryNotification notification, CancellationToken cancellationToken)
        {
            if (!IsActive) return Task.CompletedTask;

            switch (notification.State)
            {
                case ProcessState.InProgress:
                    IsBusy = true;
                    AnalyzeProgressBarValue = notification.AnalyzeProgressBarValue;
                    break;
                case ProcessState.Canceled:
                    IsBusy = false;
                    break;
                case ProcessState.Completed:
                    IsBusy = false;
                    AnalyzeProgressBarValue = notification.AnalyzeProgressBarValue;
                    break;
            }

            return Task.CompletedTask;
        }

        #endregion

        #region Private Fields

        /// <summary>
        ///  Backing field for the IsAnalyzeBeforeImportChecked property.
        /// </summary>
        private bool _isAnalyzeBeforeImportChecked;

        /// <summary>
        ///  Backing field for the AnalyzeProgressBarValue property.
        /// </summary>
        private int _analyzeProgressBarValue;

        /// <summary>
        ///  Backing field for the DataFilePath property.
        /// </summary>
        private string _dataFilePath;

        /// <summary>
        /// The cancellation token source.
        /// </summary>
        private CancellationTokenSource _cancellationTokenSource;

        #endregion

        #region Private Methods

        /// <summary>
        /// Start importing.
        /// </summary>
        private async Task StartAsync()
        {
            var inputData = new ImportMorphEntryInputPort
            {
                DataFilePath = DataFilePath,
                IsAnalyzeBeforeImportChecked = IsAnalyzeBeforeImportChecked
            };

            var result = await _interactor.StartImportMorphEntriesAsync(inputData, _cancellationTokenSource.Token);

            IsOperationResultSuccess(result);
        }

        /// <summary>
        /// Stops importing.
        /// </summary>
        private void Stop()
        {
            _cancellationTokenSource.Cancel();
            _cancellationTokenSource = new CancellationTokenSource();
        }

        /// <summary>
        /// Reset view model values.
        /// </summary>
        private void Reset()
        {
            if (IsBusy) return;

            IsAnalyzeBeforeImportChecked = true;
            AnalyzeProgressBarValue = 0;
            DataFilePath = string.Empty;
        }

        #endregion
    }
}
