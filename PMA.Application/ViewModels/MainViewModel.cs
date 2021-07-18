// <copyright file="MainViewModel.cs" company="Andrey Pospelov">
//     Copyright 2017-2021 Andrey Pospelov. All rights reserved.
// </copyright>

using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.Toolkit.Mvvm.Input;
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
        /// The current process state.
        /// </summary>
        private ProcessState _currentProcessState;

        /// <summary>
        /// The current depth level.
        /// </summary>
        private int _currentDepthLevel;

        /// <summary>
        /// Initializes the new instance of <see cref="MainViewModel"/> class.
        /// </summary>
        /// <param name="interactor">The main view model interactor.</param>
        /// <param name="serviceLocator">The service locator.</param>
        /// <param name="mediator">The mediator.</param>
        /// <param name="logger">The logger.</param>
        public MainViewModel(IMainInteractor interactor, IServiceLocator serviceLocator, IMediator mediator, ILogger<MainViewModel> logger) : base(serviceLocator, mediator, logger)
        {
            _interactor = interactor;

            Logger.LogInit();

            InputWatermark = ServiceLocator.TranslateService.Translate("MainViewModel.MorphologicalWatermark");
            StatusLabel = ServiceLocator.TranslateService.Translate($"MainViewModel.StatusLabel.{ProcessState.Idle}");

            StartCommand = new RelayCommand(StartAnalysis);
            StopCommand = new RelayCommand(StopAnalysis);
        }

        #region Implementation of IMainViewModel

        /// <summary>
        ///  Backing field for the InputText property.
        /// </summary>
        private string _inputText = string.Empty;

        /// <summary>
        /// Gets or sets an input text for analysis.
        /// </summary>
        public string InputText
        {
            get => _inputText;
            set => SetProperty(ref _inputText, value);
        }

        /// <summary>
        ///  Backing field for the InputWatermark property.
        /// </summary>
        private string _inputWatermark;

        /// <summary>
        /// Gets an input watermark.
        /// </summary>
        public string InputWatermark
        {
            get => _inputWatermark;
            private init => SetProperty(ref _inputWatermark, value);
        }

        /// <summary>
        ///  Backing field for the StatusLabel property.
        /// </summary>
        private string _statusLabel;

        /// <summary>
        /// Gets a status label text.
        /// </summary>
        public string StatusLabel
        {
            get => _statusLabel;
            private set => SetProperty(ref _statusLabel, value);
        }

        /// <summary>
        ///  Backing field for the Layer property.
        /// </summary>
        private int _layer;

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
        ///  Backing field for the MaxDepthLevel property.
        /// </summary>
        private int _maxDepthLevel;

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
        ///  Backing field for the AutoSymbolReplace property.
        /// </summary>
        private bool _autoSymbolReplace;

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
        /// Gets the command to start analysis.
        /// </summary>
        public ICommand StartCommand { get; }

        /// <summary>
        /// Gets the command to stop analysis.
        /// </summary>
        public ICommand StopCommand { get; }

        #endregion

        #region Overrides of ViewModelBase

        /// <summary>
        /// Action when the view appears.
        /// </summary>
        public override void OnAppearing()
        {
            base.OnAppearing();

            Layer = ServiceLocator.SettingService.GetValue<int>("Options.Layer");
            MaxDepthLevel = ServiceLocator.SettingService.GetValue<int>("Options.MaxDepthLevel");
            AutoSymbolReplace = ServiceLocator.SettingService.GetValue<bool>("Options.AutoSymbolReplace");

            _updateStatusLabelTimer.Elapsed += Timer_Elapsed;
        }

        /// <summary>
        /// Action when the view disappears.
        /// </summary>
        public override void OnDisappearing()
        {
            base.OnDisappearing();

            _updateStatusLabelTimer.Elapsed -= Timer_Elapsed;
        }

        #endregion

        #region Implementation of INotificationHandler<in MorphParserNotification>

        /// <summary>Handles a notification</summary>
        /// <param name="notification">The notification</param>
        /// <param name="cancellationToken">Cancellation token</param>
        public Task Handle(MorphParserNotification notification, CancellationToken cancellationToken)
        {
            _currentProcessState = notification.State;

            switch (_currentProcessState)
            {
                case ProcessState.InProgress:
                    {
                        IsBusy = true;

                        _morphAnalysisTimer.Restart();
                        _updateStatusLabelTimer.Start();

                        (string sizeValue, var sizeType) = GcHelper.GetTotalMemory();
                        string memory = sizeValue + " " + ServiceLocator.TranslateService.Translate(sizeType);

                        StatusLabel = ServiceLocator.TranslateService.Translate($"MainViewModel.StatusLabel.{_currentProcessState}",
                            _morphAnalysisTimer.GetTime(),
                            memory,
                            _currentDepthLevel);

                        break;
                    }
                case ProcessState.Canceling:
                    {
                        (string sizeValue, var sizeType) = GcHelper.GetTotalMemory();
                        string memory = sizeValue + " " + ServiceLocator.TranslateService.Translate(sizeType);

                        StatusLabel = ServiceLocator.TranslateService.Translate(
                            $"MainViewModel.StatusLabel.{_currentProcessState}",
                            _morphAnalysisTimer.GetTime(),
                            memory,
                            _currentDepthLevel);

                        break;
                    }
                case ProcessState.Completed:
                    {
                        IsBusy = false;

                        _morphAnalysisTimer.Stop();
                        _updateStatusLabelTimer.Stop();

                        (string sizeValue, var sizeType) = GcHelper.GetTotalMemory();
                        string memory = sizeValue + " " + ServiceLocator.TranslateService.Translate(sizeType);

                        StatusLabel = ServiceLocator.TranslateService.Translate($"MainViewModel.StatusLabel.{_currentProcessState}",
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

                        _morphAnalysisTimer.Stop();
                        _updateStatusLabelTimer.Stop();

                        StatusLabel = ServiceLocator.TranslateService.Translate($"MainViewModel.StatusLabel.{_currentProcessState}");

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
            _currentDepthLevel = notification.CurrentDepthLevel; ;

            return Task.CompletedTask;
        }

        #endregion

        /// <summary>
        /// Starts analysis.
        /// </summary>
        private void StartAnalysis()
        {
            var inputData = new MorphParserInputPort
            {
                MorphEntry = MorphEntryFactory.Create(InputText),
                ParsingType = ServiceLocator.SettingService.GetValue<bool>("Options.DebugMode") ? MorphParsingType.Debug : MorphParsingType.Release,
                MaxDepthLevel = MaxDepthLevel
            };

            var result = _interactor.StartAnalysis(inputData);

            if (!result.Success)
            {
                Logger.LogErrors(result.Messages);
            }
        }

        /// <summary>
        /// Stops analysis.
        /// </summary>
        private void StopAnalysis()
        {
            var result = _interactor.StopAnalysis();

            if (!result.Success)
            {
                Logger.LogErrors(result.Messages);
            }
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
                    {
                        (string sizeValue, var sizeType) = GcHelper.GetTotalMemory();
                        string memory = sizeValue + " " + ServiceLocator.TranslateService.Translate(sizeType);

                        StatusLabel = ServiceLocator.TranslateService.Translate($"MainViewModel.StatusLabel.{_currentProcessState}",
                            _morphAnalysisTimer.GetTime(),
                            memory,
                            _currentDepthLevel);

                        break;
                    }
                case ProcessState.Canceling:
                    {
                        (string sizeValue, var sizeType) = GcHelper.GetTotalMemory();
                        string memory = sizeValue + " " + ServiceLocator.TranslateService.Translate(sizeType);

                        StatusLabel = ServiceLocator.TranslateService.Translate($"MainViewModel.StatusLabel.{_currentProcessState}",
                            _morphAnalysisTimer.GetTime(),
                            memory,
                            _currentDepthLevel);

                        break;
                    }
            }
        }
    }
}
