// <copyright file="ImportMorphEntryViewModel.cs" company="Andrey Pospelov">
//     Copyright 2017-2021 Andrey Pospelov. All rights reserved.
// </copyright>

using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.Toolkit.Mvvm.Input;
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
        /// <param name="mediator">The mediator.</param>
        /// <param name="logger">The logger.</param>
        public ImportMorphEntryViewModel(IImportMorphEntryInteractor interactor,
            IServiceLocator serviceLocator,
            IMediator mediator,
            ILogger<ImportMorphEntryViewModel> logger) : base(serviceLocator,
            mediator,
            logger)
        {
            _interactor = interactor;

            Logger.LogInit();

            StartCommand = new RelayCommand(Start);
            StopCommand = new RelayCommand(Stop);
            ResetCommand = new RelayCommand(Reset);
        }

        #region Implementation of IImportMorphEntryViewModel

        /// <summary>
        ///  Backing field for the IsAnalyzeBeforeImportChecked property.
        /// </summary>
        private bool _isAnalyzeBeforeImportChecked;

        /// <summary>
        /// Gets or sets whether the analyze before import is checked or not.
        /// </summary>
        public bool IsAnalyzeBeforeImportChecked
        {
            get => _isAnalyzeBeforeImportChecked;
            set => SetProperty(ref _isAnalyzeBeforeImportChecked, value);
        }

        /// <summary>
        ///  Backing field for the AnalyzeProgressBarValue property.
        /// </summary>
        private int _analyzeProgressBarValue;

        /// <summary>
        /// Gets an analyze progress bar value.
        /// </summary>
        public int AnalyzeProgressBarValue
        {
            get => _analyzeProgressBarValue;
            private set => SetProperty(ref _analyzeProgressBarValue, value);
        }

        /// <summary>
        ///  Backing field for the DataFilePath property.
        /// </summary>
        private string _dataFilePath;

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

        #region Overrides of ViewModelBase

        /// <summary>
        /// Action when the view appears.
        /// </summary>
        public override void OnAppearing()
        {
            base.OnAppearing();

            IsAnalyzeBeforeImportChecked = ServiceLocator.SettingService.GetValue<bool>("ImportMorphEntryViewModel.IsAnalyzeBeforeImportChecked");
            DataFilePath = ServiceLocator.SettingService.GetValue<string>("ImportMorphEntryViewModel.DataFilePath");
        }

        /// <summary>
        /// Action when the view disappears.
        /// </summary>
        public override void OnDisappearing()
        {
            base.OnDisappearing();

            ServiceLocator.SettingService.SetValue("ImportMorphEntryViewModel.IsAnalyzeBeforeImportChecked", IsAnalyzeBeforeImportChecked);
            ServiceLocator.SettingService.SetValue("ImportMorphEntryViewModel.DataFilePath", DataFilePath);
        }

        #endregion

        #region Implementation of INotificationHandler<in ImportMorphEntryNotification>

        /// <summary>Handles a notification</summary>
        /// <param name="notification">The notification</param>
        /// <param name="cancellationToken">Cancellation token</param>
        public Task Handle(ImportMorphEntryNotification notification, CancellationToken cancellationToken)
        {
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

        /// <summary>
        /// Start importing.
        /// </summary>
        private void Start()
        {
            var inputData = new ImportMorphEntryInputPort
            {
                DataFilePath = DataFilePath,
                IsAnalyzeBeforeImportChecked = IsAnalyzeBeforeImportChecked
            };

            var result = _interactor.StartImportMorphEntries(inputData);

            if (!result.Success)
            {
                Logger.LogErrors(result.Messages);
            }
        }

        /// <summary>
        /// Stops importing.
        /// </summary>
        private void Stop()
        {
            var result = _interactor.StopImportMorphEntries();

            if (!result.Success)
            {
                Logger.LogErrors(result.Messages);
            }
        }

        /// <summary>
        /// Reset view model values.
        /// </summary>
        private void Reset()
        {
            if (!IsBusy)
            {
                IsAnalyzeBeforeImportChecked = true;
                AnalyzeProgressBarValue = 0;
                DataFilePath = string.Empty;
            }
        }
    }
}
