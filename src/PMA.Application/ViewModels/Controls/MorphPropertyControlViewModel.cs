// <copyright file="MorphPropertyControlViewModel.cs" company="Andrey Pospelov">
//     Copyright 2017-2021 Andrey Pospelov. All rights reserved.
// </copyright>

using Microsoft.Extensions.Logging;
using Microsoft.Toolkit.Mvvm.Messaging;
using PMA.Application.ViewModels.Base;
using PMA.Domain.Constants;
using PMA.Domain.InputPorts;
using PMA.Domain.Interfaces.Interactors.Primary;
using PMA.Domain.Interfaces.Locators;
using PMA.Domain.Interfaces.ViewModels.Controls;
using PMA.Domain.Models;
using PMA.Utils.Extensions;
using System.Collections.Generic;
using System.Threading;

namespace PMA.Application.ViewModels.Controls
{
    /// <summary>
    /// The morphological property control view model class.
    /// </summary>
    public sealed class MorphPropertyControlViewModel : ViewModelBase<MorphPropertyControlViewModel>, IMorphPropertyControlViewModel
    {
        /// <summary>
        /// The morphological property view model interactor.
        /// </summary>
        private readonly IMorphPropertyInteractor _interactor;

        /// <summary>
        /// Initializes the new instance of <see cref="MorphPropertyControlViewModel"/> class.
        /// </summary>
        /// <param name="index">The property index.</param>
        /// <param name="interactor">The morphological property view model interactor.</param>
        /// <param name="serviceLocator">The service locator.</param>
        /// <param name="logger">The logger.</param>
        /// <param name="messenger">The messenger.</param>
        public MorphPropertyControlViewModel(int index, IMorphPropertyInteractor interactor, IServiceLocator serviceLocator, ILogger<MorphPropertyControlViewModel> logger, IMessenger messenger) : base(serviceLocator, logger, messenger)
        {
            Index = index;
            _interactor = interactor;

            Logger.LogInit();
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

            var result = await _interactor.GetMorphParameterAsync(Index, _cancellationTokenSource.Token);

            if (!IsOperationResultSuccess(result)) return;

            _morphParameter = result.Result;

            UpdateTermProperties();
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

        #region Implementation of IMorphPropertyControlViewModel

        /// <summary>
        /// Gets the property index.
        /// </summary>
        public int Index { get; }

        /// <summary>
        /// Gets the property category.
        /// </summary>
        public string Category => _morphParameter.Category;

        /// <summary>
        /// Gets the property name.
        /// </summary>
        public string Name => _morphParameter.PropertyName;

        /// <summary>
        /// Gets the property description.
        /// </summary>
        public string Description => _morphParameter.Description;

        /// <summary>
        /// Gets or sets the selected term.
        /// </summary>
        public string SelectedTerm
        {
            get => _selectedTerm;
            set => SetProperty(ref _selectedTerm, value);
        }

        /// <summary>
        /// Gets the collection of alternative property term entries.
        /// </summary>
        public IList<string> TermEntries
        {
            get => _termEntries;
            private set
            {
                if (Equals(_termEntries, value)) return;

                int newSelectedIndex = value.IndexOf(_selectedTerm);

                if (newSelectedIndex == -1)
                {
                    _selectedTerm = value[0];
                }

                SetProperty(ref _termEntries, value);

                if (newSelectedIndex == -1)
                {
                    OnPropertyChanged(nameof(SelectedTerm));
                }
            }
        }

        /// <summary>
        /// Gets or sets the collection of property term IDs.
        /// </summary>
        public IList<byte> TermIds
        {
            get => _termIds;
            set
            {
                if (!SetProperty(ref _termIds, value)) return;

                UpdateTermProperties();
            }
        }

        #endregion

        #region Private Fields

        /// <summary>
        /// The morphological parameter for this property.
        /// </summary>
        private MorphParameter _morphParameter;

        /// <summary>
        ///  Backing field for the SelectedTerm property.
        /// </summary>
        private string _selectedTerm;

        /// <summary>
        ///  Backing field for the TermEntries property.
        /// </summary>
        private IList<string> _termEntries;

        /// <summary>
        ///  Backing field for the TermIds property.
        /// </summary>
        private IList<byte> _termIds = new List<byte> { MorphConstants.UnknownTermId };

        /// <summary>
        /// The cancellation token source.
        /// </summary>
        private CancellationTokenSource _cancellationTokenSource;

        #endregion

        /// <summary>
        /// Updates term properties.
        /// </summary>
        private async void UpdateTermProperties()
        {
            var inputPort = new GetTermEntriesByIdsInputPort
            {
                TermIds = TermIds,
                UseAltPropertyEntry = _morphParameter.UseAltPropertyEntry
            };

            var result = await _interactor.GetTermEntriesByIdsAsync(inputPort, _cancellationTokenSource.Token);

            if (!IsOperationResultSuccess(result)) return;

            TermEntries = result.Result;
        }
    }
}
