// <copyright file="MainViewModel.cs" company="Andrey Pospelov">
//     Copyright 2017-2021 Andrey Pospelov. All rights reserved.
// </copyright>

using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.Toolkit.Mvvm.Input;
using Microsoft.Toolkit.Mvvm.Messaging;
using PMA.Application.Extensions;
using PMA.Application.Factories;
using PMA.Application.Helpers;
using PMA.Application.ViewModels.Base;
using PMA.Domain.Enums;
using PMA.Domain.InputPorts;
using PMA.Domain.Interfaces.Interactors.Primary;
using PMA.Domain.Interfaces.Locators;
using PMA.Domain.Interfaces.ViewModels;
using PMA.Domain.Notifications;
using PMA.Utils.Extensions;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Input;
using Timer = System.Timers.Timer;

namespace PMA.Application.ViewModels
{
    /// <summary>
    /// The main view model class.
    /// </summary>
    public sealed class MainViewModel : ViewModelBase<MainViewModel>, IMainViewModel, INotificationHandler<MorphParserNotification>, INotificationHandler<DepthLevelNotification>
    {
        /// <summary>
        /// The main view model interactor.
        /// </summary>
        private readonly IMainInteractor _interactor;

        /// <summary>
        /// The morphological analysis timer.
        /// </summary>
        private readonly Stopwatch _morphAnalysisTimer = new();

        /// <summary>
        /// The status label update timer.
        /// </summary>
        private readonly Timer _updateStatusLabelTimer = new(1000);

        /// <summary>
        /// Initializes the new instance of <see cref="MainViewModel"/> class.
        /// </summary>
        /// <param name="interactor">The main view model interactor.</param>
        /// <param name="serviceLocator">The service locator.</param>
        /// <param name="logger">The logger.</param>
        /// <param name="messenger">The messenger.</param>
        public MainViewModel(IMainInteractor interactor, IServiceLocator serviceLocator, ILogger<MainViewModel> logger, IMessenger messenger) : base(serviceLocator, logger, messenger)
        {
            _interactor = interactor;

            Logger.LogInit();

            ExecuteCommand = new AsyncRelayCommand<bool>(StartOrStopAsync);
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

            InputWatermark = ServiceLocator.TranslateService.Translate("MainViewModel.Watermark");
            StatusLabel = ServiceLocator.TranslateService.Translate($"MainViewModel.StatusLabel.{ProcessState.Idle}");

            Layer = ServiceLocator.SettingService.GetValue<int>("Options.Layer");
            MaxDepthLevel = ServiceLocator.SettingService.GetValue<int>("Options.MaxDepthLevel");
            AutoSymbolReplace = ServiceLocator.SettingService.GetValue<bool>("Options.AutoSymbolReplace");

            _updateStatusLabelTimer.Elapsed += Timer_Elapsed;
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

            _updateStatusLabelTimer.Elapsed -= Timer_Elapsed;
        }

        #endregion

        #region Implementation of IMainViewModel

        /// <summary>
        /// Gets or sets an input text for analysis.
        /// </summary>
        public string InputText
        {
            get => _inputText;
            set
            {
                if (AutoSymbolReplace)
                {
                    value = value.LatinToPali();
                }

                SetProperty(ref _inputText, value);
            }
        }

        /// <summary>
        /// Gets an input watermark.
        /// </summary>
        public string InputWatermark
        {
            get => _inputWatermark;
            private set => SetProperty(ref _inputWatermark, value);
        }

        /// <summary>
        /// Gets a status label text.
        /// </summary>
        public string StatusLabel
        {
            get => _statusLabel;
            private set => SetProperty(ref _statusLabel, value);
        }

        /// <summary>
        /// Gets or sets a chronological layer for calculating the rating of solutions.
        /// </summary>
        public int Layer
        {
            get => _layer;
            set
            {
                if (SetProperty(ref _layer, value))
                {
                    ServiceLocator.SettingService.SetValue("Options.Layer", value);
                }
            }
        }

        /// <summary>
        /// Gets or sets a maximum depth of wordform analysis according to morphological rules.
        /// </summary>
        public int MaxDepthLevel
        {
            get => _maxDepthLevel;
            set
            {
                if (SetProperty(ref _maxDepthLevel, value))
                {
                    ServiceLocator.SettingService.SetValue("Options.MaxDepthLevel", value);
                }
            }
        }

        /// <summary>
        /// Gets or sets auto symbol replace option: aa -> ā, ii -> ī, etc.
        /// </summary>
        public bool AutoSymbolReplace
        {
            get => _autoSymbolReplace;
            set
            {
                if (SetProperty(ref _autoSymbolReplace, value))
                {
                    ServiceLocator.SettingService.SetValue("Options.AutoSymbolReplace", value);
                }
            }
        }

        /// <summary>
        /// Gets whether the execute command is disabled.
        /// </summary>
        public bool ExecuteCommandDisabled
        {
            get => _executeCommandDisabled;
            private set => SetProperty(ref _executeCommandDisabled, value);
        }

        /// <summary>
        /// Gets the command to start or stop analysis.
        /// </summary>
        public ICommand ExecuteCommand { get; }

        #endregion

        #region Implementation of INotificationHandler<in MorphParserNotification>

        /// <summary>Handles a notification</summary>
        /// <param name="notification">The notification</param>
        /// <param name="cancellationToken">Cancellation token</param>
        public Task Handle(MorphParserNotification notification, CancellationToken cancellationToken)
        {
            if (!IsActive) return Task.CompletedTask;

            _currentProcessState = notification.State;

            switch (_currentProcessState)
            {
                case ProcessState.InProgress:
                    {
                        IsBusy = true;

                        if (!_analysisStarted)
                        {
                            ExecuteCommandDisabled = true;
                        }

                        _morphAnalysisTimer.Restart();
                        _updateStatusLabelTimer.Start();

                        (string sizeValue, var sizeType) = GcHelper.GetTotalMemory();
                        string memory = sizeValue + " " + ServiceLocator.TranslateService.Translate(sizeType);

                        StatusLabel = ServiceLocator.TranslateService.Translate("MainViewModel.StatusLabel.InProgress",
                            _morphAnalysisTimer.GetTime(),
                            memory,
                            _currentDepthLevel);
                        break;
                    }
                case ProcessState.Completed:
                    {
                        IsBusy = false;
                        ExecuteCommandDisabled = false;
                        _analysisStarted = false;

                        _morphAnalysisTimer.Stop();
                        _updateStatusLabelTimer.Stop();

                        (string sizeValue, var sizeType) = GcHelper.GetTotalMemory();
                        string memory = sizeValue + " " + ServiceLocator.TranslateService.Translate(sizeType);

                        StatusLabel = ServiceLocator.TranslateService.Translate("MainViewModel.StatusLabel.Completed",
                            notification.Result.TotalSolutionCount(),
                            _morphAnalysisTimer.GetTime(),
                            memory,
                            _currentDepthLevel,
                            notification.Result.GetKey());
                        break;
                    }
                case ProcessState.Canceled:
                    {
                        IsBusy = false;
                        ExecuteCommandDisabled = false;
                        _analysisStarted = false;

                        _morphAnalysisTimer.Stop();
                        _updateStatusLabelTimer.Stop();

                        StatusLabel = ServiceLocator.TranslateService.Translate("MainViewModel.StatusLabel.Canceled");
                        break;
                    }
            }

            return Task.CompletedTask;
        }

        #endregion

        #region Implementation of INotificationHandler<in DepthLevelNotification>

        /// <summary>Handles a notification</summary>
        /// <param name="notification">The notification</param>
        /// <param name="cancellationToken">Cancellation token</param>
        public Task Handle(DepthLevelNotification notification, CancellationToken cancellationToken)
        {
            if (!IsActive) return Task.CompletedTask;

            _currentDepthLevel = notification.CurrentDepthLevel;

            return Task.CompletedTask;
        }

        #endregion

        #region Private Fields

        /// <summary>
        ///  Backing field for the InputText property.
        /// </summary>
        private string _inputText = string.Empty;

        /// <summary>
        ///  Backing field for the InputWatermark property.
        /// </summary>
        private string _inputWatermark;

        /// <summary>
        ///  Backing field for the StatusLabel property.
        /// </summary>
        private string _statusLabel;

        /// <summary>
        ///  Backing field for the Layer property.
        /// </summary>
        private int _layer;

        /// <summary>
        ///  Backing field for the MaxDepthLevel property.
        /// </summary>
        private int _maxDepthLevel;

        /// <summary>
        ///  Backing field for the AutoSymbolReplace property.
        /// </summary>
        private bool _autoSymbolReplace;

        /// <summary>
        ///  Backing field for the ExecuteCommandDisabled property.
        /// </summary>
        private bool _executeCommandDisabled;

        /// <summary>
        /// The cancellation token source.
        /// </summary>
        private CancellationTokenSource _cancellationTokenSource;

        /// <summary>
        /// The current process state.
        /// </summary>
        private ProcessState _currentProcessState;

        /// <summary>
        /// The current depth level.
        /// </summary>
        private int _currentDepthLevel;

        /// <summary>
        /// Whether the analysis started from this view model.
        /// </summary>
        private bool _analysisStarted;

        #endregion

        #region Private Methods

        /// <summary>
        /// Starts or stops analysis.
        /// </summary>
        /// <param name="start">Starts analysis if the value is True; stops analysis if the value is False.</param>
        private async Task StartOrStopAsync(bool start)
        {
            if (string.IsNullOrEmpty(InputText) || ExecuteCommandDisabled) return;

            if (start)
            {
                _analysisStarted = true;

                var inputData = new MorphParserInputPort
                {
                    MorphEntry = MorphEntryFactory.Create(InputText),
                    ParsingType = ServiceLocator.SettingService.GetValue<bool>("Options.DebugMode") ? MorphParsingType.Debug : MorphParsingType.Release
                };

                var result = await _interactor.StartAnalysisAsync(inputData, _cancellationTokenSource.Token);

                IsOperationResultSuccess(result);
                return;
            }

            _cancellationTokenSource.Cancel();
            _cancellationTokenSource = new CancellationTokenSource();

            ExecuteCommandDisabled = true;
        }

        /// <summary>
        /// Event handler for the timer elapsed.
        /// </summary>
        /// <param name="sender">Object sender.</param>
        /// <param name="e">Event arguments.</param>
        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            switch (_currentProcessState)
            {
                case ProcessState.InProgress:
                    (string sizeValue, var sizeType) = GcHelper.GetTotalMemory();
                    string memory = sizeValue + " " + ServiceLocator.TranslateService.Translate(sizeType);

                    StatusLabel = ServiceLocator.TranslateService.Translate("MainViewModel.StatusLabel.InProgress",
                        _morphAnalysisTimer.GetTime(),
                        memory,
                        _currentDepthLevel);
                    break;
            }
        }

        #endregion
    }
}
