// <copyright file="GetEntryIdViewModel.cs" company="Andrey Pospelov">
//     Copyright 2017-2021 Andrey Pospelov. All rights reserved.
// </copyright>

using Microsoft.Extensions.Logging;
using Microsoft.Toolkit.Mvvm.Input;
using Microsoft.Toolkit.Mvvm.Messaging;
using PMA.Application.ViewModels.Base;
using PMA.Domain.Constants;
using PMA.Domain.Enums;
using PMA.Domain.Interfaces.Interactors.Primary;
using PMA.Domain.Interfaces.Locators;
using PMA.Domain.Interfaces.ViewModels;
using PMA.Domain.Interfaces.ViewModels.Controls;
using PMA.Domain.Models;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using PMA.Domain.Messages;

namespace PMA.Application.ViewModels
{
    /// <summary>
    /// The get entry ID view model class.
    /// </summary>
    public sealed class GetEntryIdViewModel : ViewModelBase<GetEntryIdViewModel>, IGetEntryIdViewModel
    {
        /// <summary>
        /// The morphological entry.
        /// </summary>
        private readonly MorphEntry _morphEntry;

        /// <summary>
        /// The get entry ID view model interactor.
        /// </summary>
        private readonly IGetEntryIdInteractor _interactor;

        /// <summary>
        /// Initializes the new instance of <see cref="GetEntryIdViewModel"/> class.
        /// </summary>
        /// <param name="morphEntry">The morphological entry.</param>
        /// <param name="interactor">The get entry ID view model interactor.</param>
        /// <param name="serviceLocator">The service locator.</param>
        /// <param name="logger">The logger.</param>
        /// <param name="messenger">The messenger.</param>
        public GetEntryIdViewModel(MorphEntry morphEntry,
            IGetEntryIdInteractor interactor,
            IServiceLocator serviceLocator,
            ILogger<GetEntryIdViewModel> logger,
            IMessenger messenger) : base(serviceLocator,
            logger,
            messenger)
        {
            _morphEntry = morphEntry;
            _interactor = interactor;

            DeleteCommand = new RelayCommand(Delete);
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

            _cancellationTokenSource = new CancellationTokenSource();

            var result = await _interactor.GetGetEntryIdControlViewModelsAsync(_morphEntry, _cancellationTokenSource.Token);

            if (!IsOperationResultSuccess(result)) return;

            if (result.Result.Count == 0)
            {
                ShowMorphEntryNotFoundModalDialog();

                IsActive = false;
            }
            else
            {
                foreach (var entry in result.Result)
                {
                    MorphEntries.Add(entry);
                }
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

            if (!_cancellationTokenSource.IsCancellationRequested)
            {
                _cancellationTokenSource.Cancel();
            }

            _cancellationTokenSource = null;

            MorphEntries.Clear();

            Messenger.Send(new CloseGetEntryIdViewModelMessage());
        }

        #endregion

        #region Implementation of IGetEntryIdViewModel

        /// <summary>
        /// Gets or sets an entry.
        /// </summary>
        public string Entry => _morphEntry.Entry;

        /// <summary>
        /// Gets or sets a collection of morphological entries.
        /// </summary>
        public ObservableCollection<IGetEntryIdControlViewModel> MorphEntries { get; } = new();

        /// <summary>
        /// Gets a command to delete the selected morphological entry.
        /// </summary>
        public ICommand DeleteCommand { get; }

        #endregion

        #region Private Fields

        /// <summary>
        /// The cancellation token source.
        /// </summary>
        private CancellationTokenSource _cancellationTokenSource;

        #endregion

        #region Private Methods

        /// <summary>
        /// Deletes selected morphological entries.
        /// </summary>
        private void Delete()
        {
            var selectedIds = MorphEntries.Where(x => x.IsSelected).Select(x => x.Id).ToList();

            ShowDeleteMorphEntryModalDialog(selectedIds);
        }

        /// <summary>
        /// Tries to delete a collection of morphological entries from the database.
        /// </summary>
        private async Task TryToDeleteMorphEntriesAsync()
        {
            var selectedEntries = MorphEntries.Where(x => x.IsSelected).ToList();

            IsBusy = true;

            var result = await _interactor.TryToDeleteMorphEntriesAsync(selectedEntries.Select(x => x.Id).ToList(), _cancellationTokenSource.Token);

            IsBusy = false;

            if (!IsOperationResultSuccess(result)) return;

            var deletedIds = result.Result.Where(x => x.IsDeleted).Select(x => x.Id).ToList();
            var errorIds = result.Result.Where(x => !x.IsDeleted).Select(x => x.Id).ToList();
            var errorParents = result.Result.Where(x => !x.IsDeleted).SelectMany(x => x.MorphEntries).ToList();

            foreach (IGetEntryIdControlViewModel entry in selectedEntries.Where(entry => deletedIds.Contains(entry.Id)))
            {
                MorphEntries.Remove(entry);
            }

            ShowMorphEntriesDeletedModalDialog(deletedIds, errorIds, errorParents);
        }

        /// <summary>
        /// Shows DeleteMorphEntry modal dialog.
        /// </summary>
        /// <param name="entryIds">The collection of morphological entry IDs.</param>
        private void ShowDeleteMorphEntryModalDialog(IEnumerable<int> entryIds)
        {
            string title = ServiceLocator.TranslateService.Translate(ModalDialogTitleConstants.Information);
            string message = ServiceLocator.TranslateService.Translate(ModalDialogMessageConstants.DeleteMorphEntries, string.Join(",", entryIds));

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
                    await TryToDeleteMorphEntriesAsync();
                    break;
            }
        }

        /// <summary>
        /// Shows MorphEntriesDeleted modal dialog.
        /// </summary>
        /// <param name="deletedIds">The deleted morphological entry IDs.</param>
        /// <param name="errorIds">The error morphological entry ID.</param>
        /// <param name="errorParents">The parents of morphological entries.</param>
        private void ShowMorphEntriesDeletedModalDialog(ICollection<int> deletedIds, ICollection<int> errorIds, IEnumerable<MorphEntry> errorParents)
        {
            string title;
            string message;

            if (deletedIds.Count > 0)
            {
                title = ServiceLocator.TranslateService.Translate(ModalDialogTitleConstants.Information);
                message = ServiceLocator.TranslateService.Translate(ModalDialogMessageConstants.MorphEntriesDeleted, string.Join(",", deletedIds));

                ServiceLocator.ModalDialogService.ShowInformationModalDialog(title, message);
            }

            if (errorIds.Count == 0) return;

            title = ServiceLocator.TranslateService.Translate(ModalDialogTitleConstants.Error);
            message = ServiceLocator.TranslateService.Translate(ModalDialogMessageConstants.DeleteMorphEntryError, string.Join(",", errorIds), string.Join(", ", errorParents.Distinct().Select(x => $"{x.Entry} ({x.Id})")));

            ServiceLocator.ModalDialogService.ShowErrorModalDialog(title, message);
        }

        /// <summary>
        /// Shows MorphEntryNotFound modal dialog.
        /// </summary>
        private void ShowMorphEntryNotFoundModalDialog()
        {
            string title = ServiceLocator.TranslateService.Translate(ModalDialogTitleConstants.Information);
            string message = ServiceLocator.TranslateService.Translate(ModalDialogMessageConstants.MorphEntryNotFound, Entry);

            ServiceLocator.ModalDialogService.ShowInformationModalDialog(title, message);
        }

        #endregion
    }
}
