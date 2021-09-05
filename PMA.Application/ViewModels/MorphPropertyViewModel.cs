// <copyright file="MorphPropertyViewModel.cs" company="Andrey Pospelov">
//     Copyright 2017-2021 Andrey Pospelov. All rights reserved.
// </copyright>

using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.Toolkit.Mvvm.Input;
using Microsoft.Toolkit.Mvvm.Messaging;
using PMA.Application.Extensions;
using PMA.Application.Factories;
using PMA.Application.ViewModels.Base;
using PMA.Application.ViewModels.Controls;
using PMA.Domain.Constants;
using PMA.Domain.Enums;
using PMA.Domain.EventArguments;
using PMA.Domain.InputPorts;
using PMA.Domain.Interfaces.Interactors.Primary;
using PMA.Domain.Interfaces.Locators;
using PMA.Domain.Interfaces.ViewModels;
using PMA.Domain.Interfaces.ViewModels.Controls;
using PMA.Domain.Messages;
using PMA.Domain.Models;
using PMA.Domain.Notifications;
using PMA.Utils.Extensions;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;

namespace PMA.Application.ViewModels
{
    /// <summary>
    /// The morphological property view model class.
    /// </summary>
    public sealed class MorphPropertyViewModel : ViewModelBase<MorphPropertyViewModel>, IMorphPropertyViewModel, INotificationHandler<MorphParserNotification>, IRecipient<MorphEntryMessage>, IRecipient<CloseGetEntryIdViewModelMessage>
    {
        /// <summary>
        /// The morphological property view model interactor.
        /// </summary>
        private readonly IMorphPropertyInteractor _interactor;

        /// <summary>
        /// Initializes the new instance of <see cref="MorphPropertyViewModel"/> class.
        /// </summary>
        /// <param name="interactor">The morphological property view model interactor.</param>
        /// <param name="serviceLocator">The service locator.</param>
        /// <param name="logger">The logger.</param>
        /// <param name="messenger">The messenger.</param>
        public MorphPropertyViewModel(IMorphPropertyInteractor interactor, IServiceLocator serviceLocator, ILogger<MorphPropertyViewModel> logger, IMessenger messenger) : base(serviceLocator, logger, messenger)
        {
            _interactor = interactor;

            Logger.LogInit();

            ExecuteCommand = new AsyncRelayCommand<bool>(StartOrStopAsync);
            ResetCommand = new RelayCommand(Reset);
            SaveCommand = new RelayCommand(Save);
            DeleteCommand = new RelayCommand(Delete);
            GetEntryIdCommand = new AsyncRelayCommand(GetEntryIdAsync);
            GetLeftIdCommand = new AsyncRelayCommand(GetLeftIdAsync);
            GetRightIdCommand = new AsyncRelayCommand(GetRightIdAsync);
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
        protected override async void OnActivated()
        {
            base.OnActivated();

            ServiceLocator.SettingService.SettingChanged += OnSettingChangedHandler;

            _cancellationTokenSource = new CancellationTokenSource();

            _autoSymbolReplace = ServiceLocator.SettingService.GetValue<bool>("Options.AutoSymbolReplace");
            _debugMode = ServiceLocator.SettingService.GetValue<bool>("Options.DebugMode");

            var result = await _interactor.GetMorphPropertyControlViewModelsAsync(_cancellationTokenSource.Token);

            if (!IsOperationResultSuccess(result)) return;

            _properties = result.Result;

            var result2 = await _interactor.GetMorphCategoryControlViewModelsAsync(_properties, _cancellationTokenSource.Token);

            if (!IsOperationResultSuccess(result)) return;

            _categories = result2.Result;

            var inputData = new UpdateMorphPropertyInputPort
            {
                Properties = _properties,
                StartIndex = 0
            };

            var result3 = await _interactor.UpdateAllMorphPropertiesAsync(inputData, _cancellationTokenSource.Token);

            if (!IsOperationResultSuccess(result3)) return;

            foreach (var property in Properties)
            {
                property.PropertyChanged += Property_PropertyChangedAsync;
            }
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

            ServiceLocator.SettingService.SettingChanged -= OnSettingChangedHandler;

            if (!_cancellationTokenSource.IsCancellationRequested)
            {
                _cancellationTokenSource.Cancel();
            }

            _cancellationTokenSource = null;

            foreach (var property in Properties)
            {
                property.PropertyChanged -= Property_PropertyChangedAsync;
            }

            _properties = null;
        }

        #endregion

        #region Implementation of IMorphPropertyViewModel

        /// <summary>
        /// Gets or sets the morphological entry text.
        /// </summary>
        public string Entry
        {
            get => _entry;
            set
            {
                if (_autoSymbolReplace)
                {
                    value = value.LatinToPali();
                }

                if (!SetProperty(ref _entry, value)) return;

                if (!IsLockEntryIdChecked)
                {
                    EntryId = 0;
                }
            }
        }

        /// <summary>
        /// Gets or sets the left part text of the morphological entry.
        /// </summary>
        public string LeftEntry
        {
            get => _leftEntry;
            set
            {
                if (_autoSymbolReplace)
                {
                    value = value.LatinToPali();
                }

                if (!SetProperty(ref _leftEntry, value)) return;

                LeftId = 0;
            }
        }

        /// <summary>
        /// Gets or sets the right part text of the morphological entry.
        /// </summary>
        public string RightEntry
        {
            get => _rightEntry;
            set
            {
                if (_autoSymbolReplace)
                {
                    value = value.LatinToPali();
                }

                if (!SetProperty(ref _rightEntry, value)) return;

                RightId = 0;
            }
        }

        /// <summary>
        /// Gets the input watermark.
        /// </summary>
        public string InputWatermark => ServiceLocator.TranslateService.Translate("MorphPropertyViewModel.Watermark");

        /// <summary>
        /// Gets the morphological entry ID.
        /// </summary>
        public int EntryId
        {
            get => _entryId;
            private set => SetProperty(ref _entryId, value);
        }

        /// <summary>
        /// Gets the ID for the left part of the morphological entry.
        /// </summary>
        public int LeftId
        {
            get => _leftId;
            private set => SetProperty(ref _leftId, value);
        }

        /// <summary>
        /// Gets the ID for the right part of the morphological entry.
        /// </summary>
        public int RightId
        {
            get => _rightId;
            private set => SetProperty(ref _rightId, value);
        }

        /// <summary>
        /// Gets or sets the morphological base.
        /// </summary>
        public MorphBase Base
        {
            get => _base;
            set => SetProperty(ref _base, value);
        }

        /// <summary>
        /// Gets or sets whether the input morphological entry is virtual (doesn't exist in the live language) or not.
        /// </summary>
        public bool? IsVirtual
        {
            get => _isVirtual;
            set => SetProperty(ref _isVirtual, value);
        }

        /// <summary>
        /// Gets or sets whether the left part of the morphological entry is used or not.
        /// </summary>
        public bool IsLeftChecked
        {
            get => _isLeftChecked;
            set => SetProperty(ref _isLeftChecked, value);
        }

        /// <summary>
        /// Gets or sets whether the right part of the morphological entry is used or not.
        /// </summary>
        public bool IsRightChecked
        {
            get => _isRightChecked;
            set => SetProperty(ref _isRightChecked, value);
        }

        /// <summary>
        /// Gets or sets whether the entry ID is locked or not.
        /// </summary>
        public bool IsLockEntryIdChecked
        {
            get => _isLockEntryIdChecked;
            set => SetProperty(ref _isLockEntryIdChecked, value);
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

        /// <summary>
        /// Gets the collection of property category models.
        /// </summary>
        public IEnumerable<IMorphCategoryControlViewModel> Categories => _categories;

        /// <summary>
        /// Gets the collection of property view models.
        /// </summary>
        public IEnumerable<IMorphPropertyControlViewModel> Properties => _properties;

        /// <summary>
        /// Gets a get entry ID view model. The value is null if get entry ID view model is closed.
        /// </summary>
        public IGetEntryIdViewModel GetEntryIdViewModel
        {
            get => _getEntryIdViewModel;
            private set => SetProperty(ref _getEntryIdViewModel, value);
        }

        /// <summary>
        /// Gets the command to save the morphological entry.
        /// </summary>
        public ICommand SaveCommand { get; }

        /// <summary>
        /// Gets the command to delete the morphological entry.
        /// </summary>
        public ICommand DeleteCommand { get; }

        /// <summary>
        /// Gets the command to reset values.
        /// </summary>
        public ICommand ResetCommand { get; }

        /// <summary>
        /// Gets the command to get the morphological entry ID.
        /// </summary>
        public ICommand GetEntryIdCommand { get; }

        /// <summary>
        /// Gets the command to get the left part ID of morphological entry.
        /// </summary>
        public ICommand GetLeftIdCommand { get; }

        /// <summary>
        /// Gets the command to get the right part ID of morphological entry.
        /// </summary>
        public ICommand GetRightIdCommand { get; }

        #endregion

        #region Implementation of INotificationHandler<in MorphParserNotification>

        /// <summary>Handles a notification</summary>
        /// <param name="notification">The notification</param>
        /// <param name="cancellationToken">Cancellation token</param>
        public Task Handle(MorphParserNotification notification, CancellationToken cancellationToken)
        {
            if (!IsActive) return Task.CompletedTask;

            switch (notification.State)
            {
                case ProcessState.InProgress:
                    IsBusy = true;

                    if (!_analysisStarted)
                    {
                        ExecuteCommandDisabled = true;
                    }

                    break;
                case ProcessState.Completed:
                case ProcessState.Canceled:
                    IsBusy = false;
                    ExecuteCommandDisabled = false;
                    _analysisStarted = false;
                    break;
            }

            return Task.CompletedTask;
        }

        #endregion

        #region Implementation of IRecipient<in MorphEntryMessage>

        /// <summary>
        /// Receives a given <see cref="MorphEntryMessage"/> message instance.
        /// </summary>
        /// <param name="message">The message being received.</param>
        public void Receive(MorphEntryMessage message)
        {
            if (message.MorphEntry is null) return;

            switch (_expectedEntryType)
            {
                case GetEntryIdType.Left:
                    LeftEntry = message.MorphEntry.Entry;
                    LeftId = message.MorphEntry.Id;
                    break;
                case GetEntryIdType.Right:
                    RightEntry = message.MorphEntry.Entry;
                    RightId = message.MorphEntry.Id;
                    break;
                default:
                    Entry = message.MorphEntry.Entry;
                    EntryId = message.MorphEntry.Id;
                    Base = message.MorphEntry.Base;
                    IsVirtual = message.MorphEntry.IsVirtual;

                    if (message.MorphEntry.Left != null)
                    {
                        IsLeftChecked = true;
                        LeftEntry = message.MorphEntry.Left.Entry;
                        LeftId = message.MorphEntry.Left.Id;
                    }
                    else
                    {
                        IsLeftChecked = false;
                        LeftEntry = string.Empty;
                        LeftId = 0;
                    }

                    if (message.MorphEntry.Right != null)
                    {
                        IsRightChecked = true;
                        RightEntry = message.MorphEntry.Right.Entry;
                        RightId = message.MorphEntry.Right.Id;
                    }
                    else
                    {
                        IsRightChecked = false;
                        RightEntry = string.Empty;
                        RightId = 0;
                    }

                    for (int i = 0; i < message.MorphEntry.Parameters.Length; i++)
                    {
                        var property = _properties[i];

                        int selectedIndex = property.TermIds.IndexOf(message.MorphEntry.Parameters[i]);

                        property.SelectedTerm = property.TermEntries[selectedIndex];
                    }
                    break;
            }

            _expectedEntryType = GetEntryIdType.Main;

            GetEntryIdViewModel = null;
        }

        #endregion

        #region Implementation of IRecipient<in CloseGetEntryIdViewModelMessage>

        /// <summary>
        /// Receives a given <see cref="CloseGetEntryIdViewModelMessage"/> message instance.
        /// </summary>
        /// <param name="message">The message being received.</param>
        public void Receive(CloseGetEntryIdViewModelMessage message)
        {
            _expectedEntryType = GetEntryIdType.Main;

            GetEntryIdViewModel = null;
        }

        #endregion

        #region Private Fields

        /// <summary>
        /// The cancellation token source.
        /// </summary>
        private CancellationTokenSource _cancellationTokenSource;

        /// <summary>
        /// Backing field for the Entry property.
        /// </summary>
        private string _entry = string.Empty;

        /// <summary>
        /// Backing field for the LeftEntry property.
        /// </summary>
        private string _leftEntry = string.Empty;

        /// <summary>
        /// Backing field for the RightEntry property.
        /// </summary>
        private string _rightEntry = string.Empty;

        /// <summary>
        /// Backing field for the EntryId property.
        /// </summary>
        private int _entryId;

        /// <summary>
        /// Backing field for the LeftId property.
        /// </summary>
        private int _leftId;

        /// <summary>
        /// Backing field for the RightId property.
        /// </summary>
        private int _rightId;

        /// <summary>
        /// Backing field for the Base property.
        /// </summary>
        private MorphBase _base = MorphBase.Unknown;

        /// <summary>
        /// Backing field for the IsVirtual property.
        /// </summary>
        private bool? _isVirtual;

        /// <summary>
        /// Backing field for the IsLeftChecked property.
        /// </summary>
        private bool _isLeftChecked;

        /// <summary>
        /// Backing field for the IsRightChecked property.
        /// </summary>
        private bool _isRightChecked;

        /// <summary>
        /// Backing field for the IsLockEntryIdChecked property.
        /// </summary>
        private bool _isLockEntryIdChecked;

        /// <summary>
        ///  Backing field for the ExecuteCommandDisabled property.
        /// </summary>
        private bool _executeCommandDisabled;

        /// <summary>
        /// Backing field for the Properties property.
        /// </summary>
        private IList<IMorphPropertyControlViewModel> _properties;

        /// <summary>
        /// Backing field for the Categories property.
        /// </summary>
        private IList<IMorphCategoryControlViewModel> _categories;

        /// <summary>
        /// Backing field for the GetEntryIdViewModel property.
        /// </summary>
        private IGetEntryIdViewModel _getEntryIdViewModel;

        /// <summary>
        /// Th auto symbol replace option: aa -> ā, ii -> ī, etc.
        /// </summary>
        private bool _autoSymbolReplace;

        /// <summary>
        /// The option allows you to select the way how the wordform will be analyzed: False – only successful solutions sorted by rating; True – all solutions, including unsuccessful ones.
        /// </summary>
        private bool _debugMode;

        /// <summary>
        /// Whether the analysis started from this view model.
        /// </summary>
        private bool _analysisStarted;

        /// <summary>
        /// The expected entry type.
        /// </summary>
        private GetEntryIdType _expectedEntryType = GetEntryIdType.Main;

        #endregion

        #region Private Methods

        /// <summary>
        /// Starts or stops analysis.
        /// </summary>
        /// <param name="start">Starts analysis if the value is True; stops analysis if the value is False.</param>
        /// <returns></returns>
        private async Task StartOrStopAsync(bool start)
        {
            if (string.IsNullOrEmpty(Entry) || ExecuteCommandDisabled) return;

            if (start)
            {
                _analysisStarted = true;

                var morphEntry = GetMorphEntryFromProperties();
                morphEntry.Id = 0;

                var inputData = new MorphParserInputPort
                {
                    MorphEntry = morphEntry,
                    ParsingType = _debugMode ? MorphParsingType.Debug : MorphParsingType.Release
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
        /// Sets view model parameters to default values.
        /// </summary>
        private void Reset()
        {
            Base = MorphBase.Unknown;
            LeftEntry = string.Empty;
            IsLeftChecked = false;
            LeftId = 0;
            RightEntry = string.Empty;
            IsRightChecked = false;
            RightId = 0;
            IsVirtual = null;

            foreach (var property in Properties)
            {
                property.SelectedTerm = property.TermEntries[0];
            }
        }

        /// <summary>
        /// Saves a morphological entry.
        /// </summary>
        private void Save()
        {
            if (IsLeftChecked && LeftId == 0) return;
            if (IsRightChecked && RightId == 0) return;
            if (IsVirtual is null) return;

            ShowSaveMorphEntryModalDialog();
        }

        /// <summary>
        /// Deletes the morphological entry.
        /// </summary>
        private void Delete()
        {
            if (EntryId == 0) return;

            ShowDeleteMorphEntryModalDialog();
        }

        /// <summary>
        /// Gets the morphological entry ID.
        /// </summary>
        private async Task GetEntryIdAsync()
        {
            var morphEntry = GetMorphEntryFromProperties();

            var result = await _interactor.GetGetEntryIdViewModelAsync(morphEntry, _cancellationTokenSource.Token);

            if (!IsOperationResultSuccess(result)) return;

            _expectedEntryType = GetEntryIdType.Main;

            GetEntryIdViewModel = result.Result;
        }

        /// <summary>
        /// Gets the left part ID of morphological entry.
        /// </summary>
        private async Task GetLeftIdAsync()
        {
            var morphEntry = MorphEntryFactory.Create(LeftId, LeftEntry);

            var result = await _interactor.GetGetEntryIdViewModelAsync(morphEntry, _cancellationTokenSource.Token);

            if (!IsOperationResultSuccess(result)) return;

            _expectedEntryType = GetEntryIdType.Left;

            GetEntryIdViewModel = result.Result;
        }

        /// <summary>
        /// Gets the right part ID of morphological entry.
        /// </summary>
        private async Task GetRightIdAsync()
        {
            var morphEntry = MorphEntryFactory.Create(RightId, RightEntry);

            var result = await _interactor.GetGetEntryIdViewModelAsync(morphEntry, _cancellationTokenSource.Token);

            if (!IsOperationResultSuccess(result)) return;

            _expectedEntryType = GetEntryIdType.Right;

            GetEntryIdViewModel = result.Result;
        }

        /// <summary>
        /// Gets a morphological entry from view model properties.
        /// </summary>
        /// <returns>A morphological entry.</returns>
        private MorphEntry GetMorphEntryFromProperties()
        {
            var morphEntry = MorphEntryFactory.Create(EntryId,
                Entry,
                Properties
                    .OrderBy(x => x.Index)
                    .Select(x => x.TermIds[x.TermEntries.IndexOf(x.SelectedTerm)])
                    .ToArray(),
                Base,
                IsVirtual,
                MorphEntrySource.User);

            if (Base != MorphBase.None)
            {
                if (IsLeftChecked)
                {
                    morphEntry.Left = MorphEntryFactory.Create(LeftId, LeftEntry);
                }

                if (IsRightChecked)
                {
                    morphEntry.Right = MorphEntryFactory.Create(RightId, RightEntry);
                }
            }

            return morphEntry;
        }

        /// <summary>
        /// Event handler for the view model property changed.
        /// </summary>
        /// <param name="sender">Object sender.</param>
        /// <param name="e">Event arguments.</param>
        private async void Property_PropertyChangedAsync(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName != nameof(IMorphPropertyControlViewModel.SelectedTerm)) return;

            var property = (MorphPropertyControlViewModel)sender;

            int startIndex = _properties.IndexOf(property) + 1;

            if (startIndex == MorphConstants.ParameterCount) return;

            foreach (var item in Properties)
            {
                item.PropertyChanged -= Property_PropertyChangedAsync;
            }

            var inputData = new UpdateMorphPropertyInputPort
            {
                Properties = _properties,
                StartIndex = startIndex
            };

            var result = await _interactor.UpdateAllMorphPropertiesAsync(inputData, _cancellationTokenSource.Token);

            if (!IsOperationResultSuccess(result)) return;

            foreach (var item in _properties)
            {
                item.PropertyChanged += Property_PropertyChangedAsync;
            }
        }

        /// <summary>
        /// Event handler for the setting changed.
        /// </summary>
        /// <param name="sender">Object sender.</param>
        /// <param name="e">Event arguments.</param>
        private void OnSettingChangedHandler(object sender, SettingEventArgs e)
        {
            switch (e.SettingName)
            {
                case "Options.AutoSymbolReplace":
                    _autoSymbolReplace = ServiceLocator.SettingService.GetValue<bool>(e.SettingName);
                    break;
                case "Options.DebugMode":
                    _debugMode = ServiceLocator.SettingService.GetValue<bool>(e.SettingName);
                    break;
            }
        }

        /// <summary>
        /// Tries to add the morphological entry to the database.
        /// </summary>
        private async Task TryToAddMorphEntryAsync()
        {
            var morphEntry = GetMorphEntryFromProperties();
            morphEntry.Id = 0;

            var result = await _interactor.TryToAddMorphEntryAsync(morphEntry, _cancellationTokenSource.Token);

            if (!IsOperationResultSuccess(result)) return;

            if (!string.IsNullOrEmpty(result.Result))
            {
                ShowSaveMorphEntryErrorModalDialog(result.Result);
                return;
            }

            EntryId = morphEntry.Id;

            ShowMorphEntryInsertedModalDialog(Entry, EntryId);
        }

        /// <summary>
        /// Tries to add the morphological entry to the database.
        /// </summary>
        private async Task TryToUpdateMorphEntryAsync()
        {
            var morphEntry = GetMorphEntryFromProperties();

            var result = await _interactor.TryToUpdateMorphEntryAsync(morphEntry, _cancellationTokenSource.Token);

            if (!IsOperationResultSuccess(result)) return;

            if (!string.IsNullOrEmpty(result.Result.Error))
            {
                ShowSaveMorphEntryErrorModalDialog(result.Result.Error);
                return;
            }

            ShowMorphEntryUpdatedModalDialog(Entry, result.Result.Ids);
        }

        /// <summary>
        /// Tries to delete the morphological entry from the database.
        /// </summary>
        private async Task TryToDeleteMorphEntryAsync()
        {
            var result = await _interactor.TryToDeleteMorphEntryAsync(EntryId, _cancellationTokenSource.Token);

            if (!IsOperationResultSuccess(result)) return;

            if (!result.Result.IsDeleted)
            {
                ShowDeleteMorphEntryErrorModalDialog(result.Result.MorphEntries);
            }
            else
            {
                EntryId = 0;
                Reset();

                ShowMorphEntryDeletedModalDialog();
            }
        }

        /// <summary>
        /// Shows SaveMorphEntryError modal dialog.
        /// </summary>
        /// <param name="error">The error.</param>
        private void ShowSaveMorphEntryErrorModalDialog(string error)
        {
            string title = ServiceLocator.TranslateService.Translate(ModalDialogTitleConstants.Error);

            ServiceLocator.ModalDialogService.ShowErrorModalDialog(title, error);
        }

        /// <summary>
        /// Shows SaveMorphEntry modal dialog.
        /// </summary>
        private void ShowSaveMorphEntryModalDialog()
        {
            string title = ServiceLocator.TranslateService.Translate(ModalDialogTitleConstants.Information);
            string message = ServiceLocator.TranslateService.Translate(ModalDialogMessageConstants.SaveMorphEntry, Entry);

            ServiceLocator.ModalDialogService.ShowModalDialog(title,
                message,
                ModalDialogType.Question,
                new[]
                {
                    ModalButtonType.Yes,
                    ModalButtonType.No
                },
                ShowSaveMorphEntryModalDialogCallBackAsync);
        }

        /// <summary>
        /// SaveMorphEntry modal dialog callback.
        /// </summary>
        /// <param name="button">A pressed button type.</param>
        private async void ShowSaveMorphEntryModalDialogCallBackAsync(ModalButtonType button)
        {
            switch (button)
            {
                case ModalButtonType.Yes:
                    if (EntryId > 0)
                    {
                        ShowMorphEntryIsExistModalDialog(new List<int> { EntryId });
                        return;
                    }

                    var result = await _interactor.GetSimilarMorphEntryIdsAsync(GetMorphEntryFromProperties(), _cancellationTokenSource.Token);

                    if (!IsOperationResultSuccess(result)) return;

                    if (result.Result.Count > 0)
                    {
                        ShowMorphEntryIsExistModalDialog(result.Result);
                        return;
                    }

                    await TryToAddMorphEntryAsync();
                    return;
            }
        }

        /// <summary>
        /// Shows DeleteMorphEntry modal dialog.
        /// </summary>
        private void ShowDeleteMorphEntryModalDialog()
        {
            string title = ServiceLocator.TranslateService.Translate(ModalDialogTitleConstants.Information);
            string message = ServiceLocator.TranslateService.Translate(ModalDialogMessageConstants.DeleteMorphEntry, EntryId);

            ServiceLocator.ModalDialogService.ShowModalDialog(title,
                message,
                ModalDialogType.Question,
                new[]
                {
                    ModalButtonType.Yes,
                    ModalButtonType.No
                },
                ShowDeleteMorphEntryModalDialogCallBackAsync);
        }

        /// <summary>
        /// DeleteMorphEntry modal dialog callback.
        /// </summary>
        /// <param name="button">A pressed button type.</param>
        private async void ShowDeleteMorphEntryModalDialogCallBackAsync(ModalButtonType button)
        {
            switch (button)
            {
                case ModalButtonType.Yes:
                    await TryToDeleteMorphEntryAsync();
                    return;
            }
        }

        /// <summary>
        /// Shows MorphEntryIsExist modal dialog.
        /// </summary>
        /// <param name="ids">The collection of existing morphological entry IDs.</param>
        private void ShowMorphEntryIsExistModalDialog(IEnumerable<int> ids)
        {
            string title = ServiceLocator.TranslateService.Translate(ModalDialogTitleConstants.Information);
            string message = ServiceLocator.TranslateService.Translate(ModalDialogMessageConstants.MorphEntryIsExist, Entry, string.Join(",", ids));

            ServiceLocator.ModalDialogService.ShowModalDialog(title,
                message,
                ModalDialogType.Question,
                new[]
                {
                    ModalButtonType.Yes,
                    ModalButtonType.No,
                    ModalButtonType.Cancel
                },
                ShowMorphEntryIsExistModalDialogCallBackAsync);
        }

        /// <summary>
        /// MorphEntryIsExist modal dialog callback.
        /// </summary>
        /// <param name="button">A pressed button type.</param>
        private async void ShowMorphEntryIsExistModalDialogCallBackAsync(ModalButtonType button)
        {
            switch (button)
            {
                case ModalButtonType.Yes:
                    await TryToUpdateMorphEntryAsync();
                    return;
                case ModalButtonType.No:
                    await TryToAddMorphEntryAsync();
                    return;
            }
        }

        /// <summary>
        /// Shows DeleteMorphEntryError modal dialog.
        /// </summary>
        /// <param name="parentMorphEntries">The collection of parent morphological entries.</param>
        private void ShowDeleteMorphEntryErrorModalDialog(IEnumerable<MorphEntry> parentMorphEntries)
        {
            string title = ServiceLocator.TranslateService.Translate(ModalDialogTitleConstants.Error);
            string message = ServiceLocator.TranslateService.Translate(ModalDialogMessageConstants.DeleteMorphEntryError, string.Join(", ", parentMorphEntries.Select(x => $"{x.Entry}(ID={x.Id})")));

            ServiceLocator.ModalDialogService.ShowErrorModalDialog(title, message);
        }

        /// <summary>
        /// Shows MorphEntryInserted modal dialog.
        /// </summary>
        /// <param name="entry">The morphological entry.</param>
        /// <param name="entryId">The inserted morphological entry ID.</param>
        private void ShowMorphEntryInsertedModalDialog(string entry, int entryId)
        {
            string title = ServiceLocator.TranslateService.Translate(ModalDialogTitleConstants.Information);
            string message = ServiceLocator.TranslateService.Translate(ModalDialogMessageConstants.MorphEntryInserted, entry, entryId);

            ServiceLocator.ModalDialogService.ShowInformationModalDialog(title, message);
        }

        /// <summary>
        /// Shows MorphEntryUpdated modal dialog.
        /// </summary>
        /// <param name="entry">The morphological entry.</param>
        /// <param name="ids">The collection of updated morphological entry IDs.</param>
        private void ShowMorphEntryUpdatedModalDialog(string entry, IEnumerable<int> ids)
        {
            string title = ServiceLocator.TranslateService.Translate(ModalDialogTitleConstants.Information);
            string message = ServiceLocator.TranslateService.Translate(ModalDialogMessageConstants.MorphEntryUpdated, entry, string.Join(",", ids));

            ServiceLocator.ModalDialogService.ShowInformationModalDialog(title, message);
        }

        /// <summary>
        /// Shows MorphEntryDeleted modal dialog.
        /// </summary>
        private void ShowMorphEntryDeletedModalDialog()
        {
            string title = ServiceLocator.TranslateService.Translate(ModalDialogTitleConstants.Information);
            string message = ServiceLocator.TranslateService.Translate(ModalDialogMessageConstants.MorphEntryDeleted, Entry, EntryId);

            ServiceLocator.ModalDialogService.ShowInformationModalDialog(title, message);
        }

        #endregion
    }
}
