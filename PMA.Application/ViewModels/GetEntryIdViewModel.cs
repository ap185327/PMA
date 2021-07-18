// <copyright file="GetEntryIdViewModel.cs" company="Andrey Pospelov">
//     Copyright 2017-2021 Andrey Pospelov. All rights reserved.
// </copyright>

using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.Toolkit.Mvvm.Input;
using PMA.Application.ViewModels.Base;
using PMA.Application.ViewModels.Controls;
using PMA.Domain.Enums;
using PMA.Domain.Interfaces.Interactors.Primary;
using PMA.Domain.Interfaces.Locators;
using PMA.Domain.Interfaces.ViewModels;
using PMA.Domain.Interfaces.ViewModels.Controls;
using PMA.Domain.Models;
using PMA.Domain.Requests;
using PMA.Utils.Extensions;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows.Input;

namespace PMA.Application.ViewModels
{
    /// <summary>
    /// The get entry ID view model class.
    /// </summary>
    public sealed class GetEntryIdViewModel : ViewModelBase<GetEntryIdViewModel>, IGetEntryIdViewModel
    {
        /// <summary>
        /// The get entry ID view model interactor.
        /// </summary>
        private readonly IGetEntryIdInteractor _interactor;

        /// <summary>
        /// The control logger.
        /// </summary>
        private readonly ILogger<GetEntryIdControlViewModel> _controlLogger;

        /// <summary>
        /// The current modal dialog.
        /// </summary>
        private ModalDialogName _currentModalDialog = ModalDialogName.None;

        /// <summary>
        /// Initializes the new instance of <see cref="GetEntryIdViewModel"/> class.
        /// </summary>
        /// <param name="interactor">The get entry ID view model interactor.</param>
        /// <param name="serviceLocator">The service locator.</param>
        /// <param name="mediator">The mediator.</param>
        /// <param name="controlLogger">The control logger.</param>
        /// <param name="logger">The logger.</param>
        public GetEntryIdViewModel(IGetEntryIdInteractor interactor, IServiceLocator serviceLocator, IMediator mediator, ILogger<GetEntryIdControlViewModel> controlLogger, ILogger<GetEntryIdViewModel> logger) : base(serviceLocator, mediator, logger)
        {
            _interactor = interactor;
            _controlLogger = controlLogger;

            Logger.LogInit();

            DeleteCommand = new RelayCommand(Delete);
        }

        #region Implementation of IGetEntryIdViewModel

        /// <summary>
        ///  Backing field for the Entry property.
        /// </summary>
        private string _entry;

        /// <summary>
        /// Gets or sets a morphological entry.
        /// </summary>
        public string Entry
        {
            get => _entry;
            private set => SetProperty(ref _entry, value);
        }

        /// <summary>
        /// Gets a collection of morphological entries.
        /// </summary>
        public ObservableCollection<IGetEntryIdControlViewModel> MorphEntries { get; } = new();

        /// <summary>
        /// Gets a command to delete the selected morphological entry.
        /// </summary>
        public ICommand DeleteCommand { get; }

        #endregion

        #region Overrides of ViewModelBase

        /// <summary>
        /// Presses a modal dialog button.
        /// </summary>
        /// <param name="modalButtonIndex">A button index.</param>
        protected override void PressModalDialogButton(int modalButtonIndex)
        {
            base.PressModalDialogButton(modalButtonIndex);

            switch (_currentModalDialog)
            {
                case ModalDialogName.DeleteMorphEntry:
                    switch (modalButtonIndex)
                    {
                        case 0: // Yes
                            TryToDeleteMorphEntries();
                            break;
                        case 1: // No
                            break;
                    }
                    break;
                case ModalDialogName.MorphEntryNotFound:
                case ModalDialogName.MorphEntryDeleted:
                    break;
            }

            _currentModalDialog = ModalDialogName.None;
        }

        /// <summary>
        /// Action when the view appears.
        /// </summary>
        public override void OnAppearing()
        {
            base.OnAppearing();

            var morphEntry = Mediator.Send(new GetMorphEntryRequest()).Result;

            Entry = morphEntry.Entry;

            var result = _interactor.GetMorphEntriesByMorphEntry(morphEntry);

            if (!result.Success)
            {
                Logger.LogErrors(result.Messages);
            }
            else
            {
                if (result.Result.Count == 0)
                {
                    ShowMorphEntryNotFoundModalDialog(morphEntry.Entry);
                }
                else
                {
                    foreach (var entry in result.Result)
                    {
                        MorphEntries.Add(new GetEntryIdControlViewModel(entry, _interactor, ServiceLocator, Mediator, _controlLogger));
                    }
                }
            }
        }

        /// <summary>
        /// Action when the view disappears.
        /// </summary>
        public override void OnDisappearing()
        {
            base.OnDisappearing();

            Entry = null;
            MorphEntries.Clear();
        }

        #endregion

        /// <summary>
        /// Deletes selected morphological entries.
        /// </summary>
        private void Delete()
        {
            var selectedIds = MorphEntries.Where(x => x.IsSelected).Select(x => x.Id).ToList();

            ShowDeleteMorphEntryModalDialog(selectedIds.Count);
        }

        /// <summary>
        /// Shows DeleteMorphEntry modal dialog.
        /// </summary>
        /// <param name="count">Number of morphological entries.</param>
        private void ShowDeleteMorphEntryModalDialog(int count)
        {
            _currentModalDialog = ModalDialogName.DeleteMorphEntry;

            OnShowModalDialog(ServiceLocator.TranslateService.Translate("GetEntryIdViewModel.MessageBoxTitle"),
                ServiceLocator.TranslateService.Translate("GetEntryIdViewModel.DeleteEntryMessageBoxText", count),
                new[]
                {
                    ServiceLocator.TranslateService.Translate("MessageBox.Button.Yes"),
                    ServiceLocator.TranslateService.Translate("MessageBox.Button.No")
                },
                ModalDialogType.Question);
        }

        /// <summary>
        /// Shows MorphEntryDeleted modal dialog.
        /// </summary>
        /// <param name="deletedIds">The deleted morphological entry IDs.</param>
        /// <param name="errorIds">The error morphological entry ID.</param>
        /// <param name="errorParents">The parents of morphological entries.</param>
        private void ShowMorphEntryDeletedModalDialog(IReadOnlyCollection<int> deletedIds, ICollection errorIds, List<MorphEntry> errorParents)
        {
            _currentModalDialog = ModalDialogName.MorphEntryDeleted;

            var stringBuilder = new StringBuilder();

            if (deletedIds.Count > 0)
            {
                stringBuilder.AppendLine(ServiceLocator.TranslateService.Translate("GetEntryIdViewModel.EntryDeletedMessageBoxText", string.Join(",", deletedIds)));
                stringBuilder.AppendLine();
            }

            if (errorIds.Count > 0)
            {
                errorParents = errorParents.Distinct().ToList();

                stringBuilder.Append(ServiceLocator.TranslateService.Translate("GetEntryIdViewModel.EntryCannotBeDeletedHeader"));
                stringBuilder.AppendLine();

                for (int i = 0; i < errorParents.Count; i++)
                {
                    if (i < 10)
                    {
                        stringBuilder.Append(ServiceLocator.TranslateService.Translate("GetEntryIdViewModel.EntryCannotBeDeletedEntryRow", errorParents[i].Id, errorParents[i].Entry));
                    }
                    else
                    {
                        stringBuilder.Append(ServiceLocator.TranslateService.Translate("GetEntryIdViewModel.EntryCannotBeDeletedOtherRow"));
                        break;
                    }
                }
            }

            OnShowModalDialog(ServiceLocator.TranslateService.Translate("GetEntryIdViewModel.MessageBoxTitle"),
                stringBuilder.ToString(),
                ModalDialogType.Information);
        }

        /// <summary>
        /// Shows MorphEntryNotFound modal dialog.
        /// </summary>
        /// <param name="entry">The morphological entry.</param>
        private void ShowMorphEntryNotFoundModalDialog(string entry)
        {
            _currentModalDialog = ModalDialogName.MorphEntryNotFound;

            OnShowModalDialog(ServiceLocator.TranslateService.Translate("GetEntryIdViewModel.MessageBoxTitle"),
                ServiceLocator.TranslateService.Translate("GetEntryIdViewModel.NotFoundEntryMessageBoxText", entry),
                ModalDialogType.Information);
        }

        /// <summary>
        /// Tries to delete a collection of morphological entries from the database.
        /// </summary>
        private void TryToDeleteMorphEntries()
        {
            var selectedEntries = MorphEntries.Where(x => x.IsSelected).ToList();

            var result = _interactor.TryToDeleteMorphEntries(selectedEntries.Select(x => x.Id).ToList());

            if (!result.Success)
            {
                Logger.LogErrors(result.Messages);
                return;
            }

            var deletedIds = result.Result.Where(x => x.IsDeleted).Select(x => x.Id).ToList();
            var errorIds = result.Result.Where(x => !x.IsDeleted).Select(x => x.Id).ToList();
            var errorParents = result.Result.Where(x => !x.IsDeleted).SelectMany(x => x.MorphEntries).ToList();

            foreach (IGetEntryIdControlViewModel entry in selectedEntries.Where(entry => deletedIds.Contains(entry.Id)))
            {
                MorphEntries.Remove(entry);
            }

            ShowMorphEntryDeletedModalDialog(deletedIds, errorIds, errorParents);
        }
    }
}
