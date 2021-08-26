// <copyright file="MorphPropertyViewModel.cs" company="Andrey Pospelov">
//     Copyright 2017-2021 Andrey Pospelov. All rights reserved.
// </copyright>

using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.Toolkit.Mvvm.Input;
using PMA.Application.Factories;
using PMA.Application.ViewModels.Base;
using PMA.Application.ViewModels.Controls;
using PMA.Domain.Constants;
using PMA.Domain.Enums;
using PMA.Domain.InputPorts;
using PMA.Domain.Interfaces.Interactors.Primary;
using PMA.Domain.Interfaces.Locators;
using PMA.Domain.Interfaces.ViewModels;
using PMA.Domain.Interfaces.ViewModels.Controls;
using PMA.Domain.Models;
using PMA.Domain.Notifications;
using PMA.Domain.Requests;
using PMA.Utils.Extensions;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;

namespace PMA.Application.ViewModels
{
    /// <summary>
    /// The morphological property view model class.
    /// </summary>
    public sealed class MorphPropertyViewModel : ViewModelBase<MorphPropertyViewModel>, IMorphPropertyViewModel, INotificationHandler<SettingChangeNotification>, INotificationHandler<MorphParserNotification>, INotificationHandler<MorphEntryNotification>, IRequestHandler<GetMorphEntryRequest, MorphEntry>
    {
        /// <summary>
        /// The morphological property view model interactor.
        /// </summary>
        private readonly IMorphPropertyInteractor _interactor;

        /// <summary>
        /// The last get ID button clicked.
        /// </summary>
        private GetIdButton _lastGetIdButtonClicked = GetIdButton.Main;

        /// <summary>
        /// Initializes the new instance of <see cref="MorphPropertyViewModel"/> class.
        /// </summary>
        /// <param name="interactor">The morphological property view model interactor.</param>
        /// <param name="serviceLocator">The service locator.</param>
        /// <param name="mediator">The mediator.</param>
        /// <param name="controlLogger">The control logger.</param>
        /// <param name="logger">The logger.</param>
        public MorphPropertyViewModel(IMorphPropertyInteractor interactor, IServiceLocator serviceLocator, IMediator mediator, ILogger<MorphPropertyControlViewModel> controlLogger, ILogger<MorphPropertyViewModel> logger) : base(serviceLocator, mediator, logger)
        {
            _interactor = interactor;

            Logger.LogInit();

            for (int i = 0; i < MorphConstants.ParameterCount; i++)
            {
                Properties.Add(new MorphPropertyControlViewModel(i, interactor, ServiceLocator, Mediator, controlLogger));
            }

            var inputData = new UpdateMorphPropertyInputPort
            {
                Properties = Properties,
                StartIndex = 0
            };

            var result = _interactor.UpdateAllMorphProperties(inputData);

            if (!result.Success)
            {
                Logger.LogErrors(result.Messages);
                ShowErrorModalDialog(result.Messages);
            }

            foreach (var property in Properties)
            {
                property.PropertyChanged += Property_PropertyChanged;
            }

            StartCommand = new RelayCommand(Start);
            StopCommand = new RelayCommand(Stop);
            ResetCommand = new RelayCommand(Reset);
            SaveCommand = new RelayCommand(Save);
            DeleteCommand = new RelayCommand(Delete);
            GetEntryIdCommand = new RelayCommand(GetEntryId);
            GetLeftIdCommand = new RelayCommand(GetLeftId);
            GetRightIdCommand = new RelayCommand(GetRightId);

            AutoSymbolReplace = ServiceLocator.SettingService.GetValue<bool>("Options.AutoSymbolReplace");
        }

        #region Implementation of IMorphPropertyViewModel

        /// <summary>
        ///  Backing field for the Entry property.
        /// </summary>
        private string _entry = string.Empty;

        /// <summary>
        /// Gets or sets the morphological entry text.
        /// </summary>
        public string Entry
        {
            get => _entry;
            set
            {
                SetProperty(ref _entry, value);

                if (!IsLockEntryIdChecked)
                {
                    EntryId = 0;
                }
            }
        }

        /// <summary>
        ///  Backing field for the LeftEntry property.
        /// </summary>
        private string _leftEntry = string.Empty;

        /// <summary>
        /// Gets or sets the left part text of the morphological entry.
        /// </summary>
        public string LeftEntry
        {
            get => _leftEntry;
            set
            {
                SetProperty(ref _leftEntry, value);
                LeftId = 0;
            }
        }

        /// <summary>
        ///  Backing field for the RightEntry property.
        /// </summary>
        private string _rightEntry = string.Empty;

        /// <summary>
        /// Gets or sets the right part text of the morphological entry.
        /// </summary>
        public string RightEntry
        {
            get => _rightEntry;
            set
            {
                SetProperty(ref _rightEntry, value);
                RightId = 0;
            }
        }

        /// <summary>
        /// Gets the input watermark.
        /// </summary>
        public string InputWatermark => ServiceLocator.TranslateService.Translate("MorphPropertyViewModel.Watermark");

        /// <summary>
        ///  Backing field for the AutoSymbolReplace property.
        /// </summary>
        private bool _autoSymbolReplace;

        /// <summary>
        /// Gets auto symbol replace option: aa -> ā, ii -> ī, etc.
        /// </summary>
        public bool AutoSymbolReplace
        {
            get => _autoSymbolReplace;
            private set => SetProperty(ref _autoSymbolReplace, value);
        }

        /// <summary>
        ///  Backing field for the EntryId property.
        /// </summary>
        private int _entryId;

        /// <summary>
        /// Gets the morphological entry ID.
        /// </summary>
        public int EntryId
        {
            get => _entryId;
            private set => SetProperty(ref _entryId, value);
        }

        /// <summary>
        ///  Backing field for the LeftId property.
        /// </summary>
        private int _leftId;

        /// <summary>
        /// Gets the ID for the left part of the morphological entry.
        /// </summary>
        public int LeftId
        {
            get => _leftId;
            private set => SetProperty(ref _leftId, value);
        }

        /// <summary>
        ///  Backing field for the RightId property.
        /// </summary>
        private int _rightId;

        /// <summary>
        /// Gets the ID for the right part of the morphological entry.
        /// </summary>
        public int RightId
        {
            get => _rightId;
            private set => SetProperty(ref _rightId, value);
        }

        /// <summary>
        ///  Backing field for the Base property.
        /// </summary>
        private MorphBase _base = MorphBase.Unknown;

        /// <summary>
        /// Gets or sets the morphological base.
        /// </summary>
        public MorphBase Base
        {
            get => _base;
            set => SetProperty(ref _base, value);
        }

        /// <summary>
        ///  Backing field for the IsVirtual property.
        /// </summary>
        private bool? _isVirtual;

        /// <summary>
        /// Gets or sets whether the input morphological entry is virtual (doesn't exist in the live language) or not.
        /// </summary>
        public bool? IsVirtual
        {
            get => _isVirtual;
            set => SetProperty(ref _isVirtual, value);
        }

        /// <summary>
        ///  Backing field for the IsLeftChecked property.
        /// </summary>
        private bool _isLeftChecked;

        /// <summary>
        /// Gets or sets whether the left part of the morphological entry is used or not.
        /// </summary>
        public bool IsLeftChecked
        {
            get => _isLeftChecked;
            set => SetProperty(ref _isLeftChecked, value);
        }

        /// <summary>
        ///  Backing field for the IsRightChecked property.
        /// </summary>
        private bool _isRightChecked;

        /// <summary>
        /// Gets or sets whether the right part of the morphological entry is used or not.
        /// </summary>
        public bool IsRightChecked
        {
            get => _isRightChecked;
            set => SetProperty(ref _isRightChecked, value);
        }

        /// <summary>
        ///  Backing field for the IsLockEntryIdChecked property.
        /// </summary>
        private bool _isLockEntryIdChecked;

        /// <summary>
        /// Gets or sets whether the entry ID is locked or not.
        /// </summary>
        public bool IsLockEntryIdChecked
        {
            get => _isLockEntryIdChecked;
            set => SetProperty(ref _isLockEntryIdChecked, value);
        }

        /// <summary>
        /// Gets the command to start analysis.
        /// </summary>
        public ICommand StartCommand { get; }

        /// <summary>
        /// Gets the command to stop analysis.
        /// </summary>
        public ICommand StopCommand { get; }

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

        /// <summary>
        /// Gets the collection of property view models.
        /// </summary>
        public IList<IMorphPropertyControlViewModel> Properties { get; } = new List<IMorphPropertyControlViewModel>();

        #endregion

        #region Overrides of ViewModelBase

        #region Overrides of ViewModelBase<MorphPropertyViewModel>

        /// <summary>
        /// Action when the view appears.
        /// </summary>
        public override void OnAppearing()
        {
            base.OnAppearing();

            _lastGetIdButtonClicked = GetIdButton.Main;
        }

        #endregion

        /// <summary>
        /// Presses a modal dialog button.
        /// </summary>
        /// <param name="modalButtonIndex">A button index.</param>
        protected override void PressModalDialogButton(int modalButtonIndex)
        {
            switch (CurrentModalDialog)
            {
                case ModalDialogName.SaveMorphEntry:
                    switch (modalButtonIndex)
                    {
                        case 0: // Yes
                            if (EntryId > 0)
                            {
                                ShowMorphEntryIsExistModalDialog(new List<int> { EntryId });
                                break;
                            }

                            var result = _interactor.GetSimilarMorphEntryIds(GetMorphEntryFromProperties());

                            if (!result.Success)
                            {
                                Logger.LogErrors(result.Messages);
                            }
                            else
                            {
                                if (result.Result.Count > 0)
                                {
                                    ShowMorphEntryIsExistModalDialog(result.Result);
                                    break;
                                }

                                TryToAddMorphEntry();
                            }
                            break;
                        case 1: // No
                            break;
                    }
                    break;
                case ModalDialogName.MorphEntryIsExist:
                    switch (modalButtonIndex)
                    {
                        case 0: // Yes
                            TryToUpdateMorphEntry();
                            break;
                        case 1: // No
                            TryToAddMorphEntry();
                            break;
                        case 2: // Cancel
                            break;
                    }
                    break;
                case ModalDialogName.SaveMorphEntryError:
                case ModalDialogName.DeleteMorphEntryError:
                case ModalDialogName.MorphEntryInserted:
                case ModalDialogName.MorphEntryUpdated:
                    break;
                case ModalDialogName.DeleteMorphEntry:
                    switch (modalButtonIndex)
                    {
                        case 0: // Yes
                            TryToDeleteMorphEntry();
                            break;
                        case 1: // No
                            break;
                    }
                    break;
                case ModalDialogName.MorphEntryDeleted:
                    EntryId = 0;
                    Reset();
                    break;
            }

            base.PressModalDialogButton(modalButtonIndex);
        }

        #endregion

        #region Implementation of INotificationHandler<in SettingChangeNotification>

        /// <summary>Handles a notification</summary>
        /// <param name="notification">The notification</param>
        /// <param name="cancellationToken">Cancellation token</param>
        public Task Handle(SettingChangeNotification notification, CancellationToken cancellationToken)
        {
            if (notification.SettingName == "Options.AutoSymbolReplace")
            {
                AutoSymbolReplace = ServiceLocator.SettingService.GetValue<bool>(notification.SettingName);
            }

            return Task.CompletedTask;
        }

        #endregion

        #region Implementation of INotificationHandler<in MorphParserNotification>

        /// <summary>Handles a notification</summary>
        /// <param name="notification">The notification</param>
        /// <param name="cancellationToken">Cancellation token</param>
        public Task Handle(MorphParserNotification notification, CancellationToken cancellationToken)
        {
            switch (notification.State)
            {
                case ProcessState.InProgress:
                case ProcessState.Canceling:
                    IsBusy = true;
                    break;
                case ProcessState.Completed:
                case ProcessState.Canceled:
                    IsBusy = false;
                    break;
            }

            return Task.CompletedTask;
        }

        #endregion

        #region Implementation of IRequestHandler<in MorphEntryRequest,MorphEntry>

        /// <summary>Handles a request</summary>
        /// <param name="request">The request</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Response from the request</returns>
        public Task<MorphEntry> Handle(GetMorphEntryRequest request, CancellationToken cancellationToken)
        {
            return Task.FromResult(GetMorphEntryFromProperties());
        }

        #endregion

        #region Implementation of INotificationHandler<in MorphEntryNotification>

        /// <summary>Handles a notification</summary>
        /// <param name="notification">The notification</param>
        /// <param name="cancellationToken">Cancellation token</param>
        public Task Handle(MorphEntryNotification notification, CancellationToken cancellationToken)
        {
            SetPropertiesFromMorphEntry(notification.MorphEntry);
            return Task.CompletedTask;
        }

        #endregion

        /// <summary>
        /// Starts the morphological analysis.
        /// </summary>
        private void Start()
        {
            var morphEntry = GetMorphEntryFromProperties();
            morphEntry.Id = 0;

            var inputData = new MorphParserInputPort
            {
                MorphEntry = morphEntry,
                ParsingType = ServiceLocator.SettingService.GetValue<bool>("Options.DebugMode") ? MorphParsingType.Debug : MorphParsingType.Release
            };

            var result = _interactor.StartAnalysis(inputData);

            if (result.Success) return;

            Logger.LogErrors(result.Messages);
            ShowErrorModalDialog(result.Messages);
        }

        /// <summary>
        /// Stops the morphological analysis.
        /// </summary>
        private void Stop()
        {
            var result = _interactor.StopAnalysis();

            if (result.Success) return;

            Logger.LogErrors(result.Messages);
            ShowErrorModalDialog(result.Messages);
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
                property.SelectedIndex = 0;
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
        private void GetEntryId()
        {
            _lastGetIdButtonClicked = GetIdButton.Main;
        }

        /// <summary>
        /// Gets the left part ID of morphological entry.
        /// </summary>
        private void GetLeftId()
        {
            _lastGetIdButtonClicked = GetIdButton.Left;
        }

        /// <summary>
        /// Gets the right part ID of morphological entry.
        /// </summary>
        private void GetRightId()
        {
            _lastGetIdButtonClicked = GetIdButton.Right;
        }

        /// <summary>
        /// Gets a morphological entry from view model properties.
        /// </summary>
        /// <returns>A morphological entry.</returns>
        private MorphEntry GetMorphEntryFromProperties()
        {
            MorphEntry morphEntry;

            switch (_lastGetIdButtonClicked)
            {
                case GetIdButton.Left:
                    morphEntry = MorphEntryFactory.Create(LeftId, LeftEntry);
                    break;
                case GetIdButton.Right:
                    morphEntry = MorphEntryFactory.Create(RightId, RightEntry);
                    break;
                default:
                    morphEntry = MorphEntryFactory.Create(EntryId, 
                        Entry, 
                        Properties.OrderBy(x => x.Index)
                            .Select(x => x.TermIds[x.SelectedIndex])
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

                    break;
            }

            return morphEntry;
        }

        /// <summary>
        /// Set properties from the morphological entry.
        /// </summary>
        /// <param name="morphEntry">The morphological entry.</param>
        private void SetPropertiesFromMorphEntry(MorphEntry morphEntry)
        {
            if (morphEntry is not null)
            {
                switch (_lastGetIdButtonClicked)
                {
                    case GetIdButton.Left:
                        LeftEntry = morphEntry.Entry;
                        LeftId = morphEntry.Id;
                        break;
                    case GetIdButton.Right:
                        RightEntry = morphEntry.Entry;
                        RightId = morphEntry.Id;
                        break;
                    default:
                        Entry = morphEntry.Entry;
                        EntryId = morphEntry.Id;
                        Base = morphEntry.Base;
                        IsVirtual = morphEntry.IsVirtual;

                        if (morphEntry.Left != null)
                        {
                            IsLeftChecked = true;
                            LeftEntry = morphEntry.Left.Entry;
                            LeftId = morphEntry.Left.Id;
                        }
                        else
                        {
                            IsLeftChecked = false;
                            LeftEntry = string.Empty;
                            LeftId = 0;
                        }

                        if (morphEntry.Right != null)
                        {
                            IsRightChecked = true;
                            RightEntry = morphEntry.Right.Entry;
                            RightId = morphEntry.Right.Id;
                        }
                        else
                        {
                            IsRightChecked = false;
                            RightEntry = string.Empty;
                            RightId = 0;
                        }

                        for (int i = 0; i < morphEntry.Parameters.Length; i++)
                        {
                            var property = Properties[i];

                            property.SelectedIndex =
                                property.TermIds.Count == 1 && morphEntry.Parameters[i] == MorphConstants.UnknownTermId
                                    ? 0
                                    : property.TermIds.IndexOf(morphEntry.Parameters[i]);
                        }

                        break;
                }
            }
        }

        /// <summary>
        /// Event handler for the view model property changed.
        /// </summary>
        /// <param name="sender">Object sender.</param>
        /// <param name="e">Event arguments.</param>
        private void Property_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "SelectedIndex")
            {
                int startIndex = Properties.IndexOf((MorphPropertyControlViewModel)sender) + 1;

                if (startIndex < MorphConstants.ParameterCount)
                {
                    foreach (var property in Properties)
                    {
                        property.PropertyChanged -= Property_PropertyChanged;
                    }

                    var inputData = new UpdateMorphPropertyInputPort
                    {
                        Properties = Properties,
                        StartIndex = startIndex
                    };

                    var result = _interactor.UpdateAllMorphProperties(inputData);

                    if (!result.Success)
                    {
                        Logger.LogErrors(result.Messages);
                        ShowErrorModalDialog(result.Messages);
                    }

                    foreach (var property in Properties)
                    {
                        property.PropertyChanged += Property_PropertyChanged;
                    }
                }
            }
        }

        /// <summary>
        /// Shows SaveMorphEntry modal dialog.
        /// </summary>
        private void ShowSaveMorphEntryModalDialog()
        {
            CurrentModalDialog = ModalDialogName.SaveMorphEntry;

            OnShowModalDialog(ServiceLocator.TranslateService.Translate("MorphPropertyViewModel.MessageBoxTitle"),
                ServiceLocator.TranslateService.Translate("MorphPropertyViewModel.SaveEntryMessageBoxText", Entry),
                new[]
                {
                    ServiceLocator.TranslateService.Translate("MessageBox.Button.Yes"),
                    ServiceLocator.TranslateService.Translate("MessageBox.Button.No")
                },
                ModalDialogType.Question);
        }

        /// <summary>
        /// Shows DeleteMorphEntry modal dialog.
        /// </summary>
        private void ShowDeleteMorphEntryModalDialog()
        {
            CurrentModalDialog = ModalDialogName.DeleteMorphEntry;

            OnShowModalDialog(ServiceLocator.TranslateService.Translate("MorphPropertyViewModel.MessageBoxTitle"),
                ServiceLocator.TranslateService.Translate("MorphPropertyViewModel.DeleteEntryMessageBoxText", EntryId),
                new[]
                {
                    ServiceLocator.TranslateService.Translate("MessageBox.Button.Yes"),
                    ServiceLocator.TranslateService.Translate("MessageBox.Button.No")
                },
                ModalDialogType.Question);
        }

        /// <summary>
        /// Shows MorphEntryIsExist modal dialog.
        /// </summary>
        /// <param name="ids">The collection of existing morphological entry IDs.</param>
        private void ShowMorphEntryIsExistModalDialog(IEnumerable<int> ids)
        {
            CurrentModalDialog = ModalDialogName.MorphEntryIsExist;

            OnShowModalDialog(ServiceLocator.TranslateService.Translate("MorphPropertyViewModel.MessageBoxTitle"),
                ServiceLocator.TranslateService.Translate("MorphPropertyViewModel.EntryIsExistMessageBoxText", Entry, string.Join(",", ids)),
                new[]
                {
                    ServiceLocator.TranslateService.Translate("MessageBox.Button.Yes"),
                    ServiceLocator.TranslateService.Translate("MessageBox.Button.No"),
                    ServiceLocator.TranslateService.Translate("MessageBox.Button.Cancel")
                },
                ModalDialogType.Question);
        }

        /// <summary>
        /// Shows SaveMorphEntryError modal dialog.
        /// </summary>
        /// <param name="error">The morphological entry error.</param>
        private void ShowSaveMorphEntryErrorModalDialog(string error)
        {
            CurrentModalDialog = ModalDialogName.SaveMorphEntryError;

            OnShowModalDialog(ServiceLocator.TranslateService.Translate("MorphPropertyViewModel.MessageBoxTitle"),
                error,
                ModalDialogType.Error);
        }

        /// <summary>
        /// Shows DeleteMorphEntryError modal dialog.
        /// </summary>
        /// <param name="parentMorphEntries">The collection of parent morphological entries.</param>
        private void ShowDeleteMorphEntryErrorModalDialog(IList<MorphEntry> parentMorphEntries)
        {
            CurrentModalDialog = ModalDialogName.DeleteMorphEntryError;

            var stringBuilder = new StringBuilder();
            stringBuilder.Append(ServiceLocator.TranslateService.Translate("MorphPropertyViewModel.EntryCannotBeDeletedHeader"));

            for (int i = 0; i < parentMorphEntries.Count; i++)
            {
                if (i < 10)
                {
                    stringBuilder.Append(ServiceLocator.TranslateService.Translate("MorphPropertyViewModel.EntryCannotBeDeletedEntryRow", parentMorphEntries[i].Id, parentMorphEntries[i].Entry));
                }
                else
                {
                    stringBuilder.Append(ServiceLocator.TranslateService.Translate("MorphPropertyViewModel.EntryCannotBeDeletedOtherRow"));
                    break;
                }
            }

            OnShowModalDialog(ServiceLocator.TranslateService.Translate("MorphPropertyViewModel.MessageBoxTitle"),
                stringBuilder.ToString(),
                ModalDialogType.Error);
        }

        /// <summary>
        /// Shows MorphEntryInserted modal dialog.
        /// </summary>
        /// <param name="entry">The morphological entry.</param>
        /// <param name="entryId">The inserted morphological entry ID.</param>
        private void ShowMorphEntryInsertedModalDialog(string entry, int entryId)
        {
            CurrentModalDialog = ModalDialogName.MorphEntryInserted;

            OnShowModalDialog(ServiceLocator.TranslateService.Translate("MorphPropertyViewModel.MessageBoxTitle"),
                ServiceLocator.TranslateService.Translate("MorphPropertyViewModel.EntryInsertedMessageBoxText", entry, entryId),
                ModalDialogType.Information);
        }

        /// <summary>
        /// Shows MorphEntryUpdated modal dialog.
        /// </summary>
        /// <param name="entry">The morphological entry.</param>
        /// <param name="ids">The collection of updated morphological entry IDs.</param>
        private void ShowMorphEntryUpdatedModalDialog(string entry, IEnumerable<int> ids)
        {
            CurrentModalDialog = ModalDialogName.MorphEntryUpdated;

            OnShowModalDialog(ServiceLocator.TranslateService.Translate("MorphPropertyViewModel.MessageBoxTitle"),
                ServiceLocator.TranslateService.Translate("MorphPropertyViewModel.EntryUpdatedMessageBoxText", entry, string.Join(",", ids)),
                ModalDialogType.Information);
        }

        /// <summary>
        /// Shows MorphEntryDeleted modal dialog.
        /// </summary>
        /// <param name="id">The deleted morphological entry ID.</param>
        private void ShowMorphEntryDeletedModalDialog(int id)
        {
            CurrentModalDialog = ModalDialogName.MorphEntryDeleted;

            OnShowModalDialog(ServiceLocator.TranslateService.Translate("MorphPropertyViewModel.MessageBoxTitle"),
                ServiceLocator.TranslateService.Translate("MorphPropertyViewModel.EntryDeletedMessageBoxText", id),
                ModalDialogType.Information);
        }

        /// <summary>
        /// Tries to add the morphological entry to the database.
        /// </summary>
        private void TryToAddMorphEntry()
        {
            var morphEntry = GetMorphEntryFromProperties();
            morphEntry.Id = 0;

            var result = _interactor.TryToAddMorphEntry(morphEntry);

            if (!result.Success)
            {
                Logger.LogErrors(result.Messages);
                ShowErrorModalDialog(result.Messages);
            }
            else
            {
                if (result.Result != null)
                {
                    ShowSaveMorphEntryErrorModalDialog(result.Result);
                }
                else
                {
                    EntryId = morphEntry.Id;

                    ShowMorphEntryInsertedModalDialog(Entry, EntryId);
                }
            }
        }

        /// <summary>
        /// Tries to add the morphological entry to the database.
        /// </summary>
        private void TryToUpdateMorphEntry()
        {
            var result = _interactor.TryToUpdateMorphEntry(GetMorphEntryFromProperties());

            if (!result.Success)
            {
                Logger.LogErrors(result.Messages);
                ShowErrorModalDialog(result.Messages);
            }
            else
            {
                if (result.Result.Error != null)
                {
                    ShowSaveMorphEntryErrorModalDialog(result.Result.Error);
                }
                else
                {
                    ShowMorphEntryUpdatedModalDialog(Entry, result.Result.Ids);
                }
            }
        }

        /// <summary>
        /// Tries to delete the morphological entry from the database.
        /// </summary>
        private void TryToDeleteMorphEntry()
        {
            var result = _interactor.TryToDeleteMorphEntry(EntryId);

            if (!result.Success)
            {
                Logger.LogErrors(result.Messages);
                ShowErrorModalDialog(result.Messages);
            }
            else
            {
                if (!result.Result.IsDeleted)
                {
                    ShowDeleteMorphEntryErrorModalDialog(result.Result.MorphEntries);
                }
                else
                {
                    ShowMorphEntryDeletedModalDialog(EntryId);
                }
            }
        }
    }
}
