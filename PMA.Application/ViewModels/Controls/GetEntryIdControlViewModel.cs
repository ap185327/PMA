// <copyright file="GetEntryIdControlViewModel.cs" company="Andrey Pospelov">
//     Copyright 2017-2021 Andrey Pospelov. All rights reserved.
// </copyright>

using Microsoft.Extensions.Logging;
using Microsoft.Toolkit.Mvvm.Input;
using Microsoft.Toolkit.Mvvm.Messaging;
using PMA.Application.ViewModels.Base;
using PMA.Domain.Interfaces.Interactors.Primary;
using PMA.Domain.Interfaces.Locators;
using PMA.Domain.Interfaces.ViewModels.Controls;
using PMA.Domain.Messages;
using PMA.Domain.Models;
using System.Threading;
using System.Windows.Input;
using PMA.Domain.InputPorts;

namespace PMA.Application.ViewModels.Controls
{
    /// <summary>
    /// The get entry ID control view model class.
    /// </summary>
    public sealed class GetEntryIdControlViewModel : ViewModelBase<GetEntryIdControlViewModel>, IGetEntryIdControlViewModel
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
        /// Initializes the new instance of <see cref="GetEntryIdControlViewModel"/> class.
        /// </summary>
        /// <param name="morphEntry">The morphological entry.</param>
        /// <param name="interactor">The get entry ID view model interactor.</param>
        /// <param name="serviceLocator">The service locator.</param>
        /// <param name="logger">The logger.</param>
        /// <param name="messenger">The messenger.</param>
        public GetEntryIdControlViewModel(MorphEntry morphEntry, IGetEntryIdInteractor interactor, IServiceLocator serviceLocator, ILogger<GetEntryIdControlViewModel> logger, IMessenger messenger) : base(serviceLocator, logger, messenger)
        {
            _morphEntry = morphEntry;
            _interactor = interactor;

            SelectCommand = new RelayCommand(Select);
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

            var inputPort = new ExtractMorphInfoInputPort
            {
                UseVisibility = true,
                Parameters = _morphEntry.Parameters
            };

            var result = await _interactor.ExtractMorphInfoFromMorphParametersAsync(inputPort, _cancellationTokenSource.Token);

            if (!IsOperationResultSuccess(result)) return;

            Parameters = result.Result;
            Base = ServiceLocator.TranslateService.Translate(_morphEntry.Base);
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
        }

        #endregion

        #region Implementation of IGetEntryIdControlViewModel

        /// <summary>
        /// Gets a morphological entry ID.
        /// </summary>
        public int Id => _morphEntry.Id;

        /// <summary>
        /// Gets a morphological entry.
        /// </summary>
        public string Entry => _morphEntry.Entry;

        /// <summary>
        /// Gets or sets a morphological parameters.
        /// </summary>
        public string Parameters { get; private set; }

        /// <summary>
        /// Gets a morphological base.
        /// </summary>
        public string Base { get; private set; }

        /// <summary>
        /// Gets a left morphological entry.
        /// </summary>
        public string Left => _morphEntry.Left is null ? string.Empty : _morphEntry.Left.Entry;

        /// <summary>
        /// Gets a right morphological entry.
        /// </summary>
        public string Right => _morphEntry.Right is null ? string.Empty : _morphEntry.Right.Entry;

        /// <summary>
        /// Gets whether the morphological entry is virtual (doesn't exist in the live language) or not.
        /// </summary>
        public bool IsVirtual => _morphEntry.IsVirtual.HasValue && _morphEntry.IsVirtual.Value;

        /// <summary>
        /// Gets or sets whether the morphological entry is selected or not.
        /// </summary>
        public bool IsSelected
        {
            get => _isSelected;
            set => SetProperty(ref _isSelected, value);
        }

        /// <summary>
        /// Gets a command to select the morphological entry.
        /// </summary>
        public ICommand SelectCommand { get; }

        #endregion

        #region Private Fields

        /// <summary>
        ///  Backing field for the IsSelected property.
        /// </summary>
        private bool _isSelected;

        /// <summary>
        /// The cancellation token source.
        /// </summary>
        private CancellationTokenSource _cancellationTokenSource;

        #endregion

        #region Private Methods

        /// <summary>
        /// Selects the morphological entry.
        /// </summary>
        private void Select()
        {
            var message = new MorphEntryMessage
            {
                MorphEntry = _morphEntry
            };

            Messenger.Send(message);
        }

        #endregion
    }
}
