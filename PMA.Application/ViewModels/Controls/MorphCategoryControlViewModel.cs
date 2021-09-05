// <copyright file="MorphCategoryControlViewModel.cs" company="Andrey Pospelov">
//     Copyright 2017-2021 Andrey Pospelov. All rights reserved.
// </copyright>

using Microsoft.Extensions.Logging;
using Microsoft.Toolkit.Mvvm.Messaging;
using PMA.Application.ViewModels.Base;
using PMA.Domain.InputPorts;
using PMA.Domain.Interfaces.Interactors.Primary;
using PMA.Domain.Interfaces.Locators;
using PMA.Domain.Interfaces.ViewModels.Controls;
using PMA.Utils.Extensions;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading;

namespace PMA.Application.ViewModels.Controls
{
    /// <summary>
    /// The morphological category control view model class.
    /// </summary>
    public sealed class MorphCategoryControlViewModel : ViewModelBase<MorphCategoryControlViewModel>, IMorphCategoryControlViewModel
    {
        /// <summary>
        /// The morphological property view model interactor.
        /// </summary>
        private readonly IMorphPropertyInteractor _interactor;

        /// <summary>
        /// Initializes the new instance of <see cref="MorphCategoryControlViewModel"/> class.
        /// </summary>
        /// <param name="properties">The collection of morphological property control view models.</param>
        /// <param name="interactor">The morphological property view model interactor.</param>
        /// <param name="serviceLocator">The service locator.</param>
        /// <param name="logger">The logger.</param>
        /// <param name="messenger">The messenger.</param>
        public MorphCategoryControlViewModel(IList<IMorphPropertyControlViewModel> properties,
            IMorphPropertyInteractor interactor,
            IServiceLocator serviceLocator,
            ILogger<MorphCategoryControlViewModel> logger,
            IMessenger messenger) : base(serviceLocator,
            logger,
            messenger)
        {
            Name = properties.First().Category;
            _properties = properties;
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
        protected override void OnActivated()
        {
            base.OnActivated();

            _cancellationTokenSource = new CancellationTokenSource();

            foreach (var property in _properties)
            {
                property.PropertyChanged += Property_PropertyChanged;
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

            foreach (var property in _properties)
            {
                property.PropertyChanged -= Property_PropertyChanged;
            }
        }

        #endregion

        #region Implementation of IMorphCategoryControlViewModel

        /// <summary>
        /// Gets a category name.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Gets a selected property terms for the category.
        /// </summary>
        public string Terms
        {
            get => _terms;
            private set => SetProperty(ref _terms, value);
        }

        /// <summary>
        /// Gets the collection of property view models.
        /// </summary>
        public IEnumerable<IMorphPropertyControlViewModel> Properties => _properties;

        #endregion

        #region Private Fields

        /// <summary>
        ///  Backing field for the Terms property.
        /// </summary>
        private string _terms;

        /// <summary>
        /// Backing field for the Properties property.
        /// </summary>
        private readonly IList<IMorphPropertyControlViewModel> _properties;

        /// <summary>
        /// The cancellation token source.
        /// </summary>
        private CancellationTokenSource _cancellationTokenSource;

        #endregion

        #region Private Methods

        private async void Property_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName != nameof(IMorphPropertyControlViewModel.SelectedTerm)) return;

            byte[] parameters = _properties.Select(x => x.TermIds[x.TermEntries.IndexOf(x.SelectedTerm)]).ToArray();

            var inputPort = new ExtractMorphInfoInputPort
            {
                UseVisibility = false,
                Parameters = parameters
            };

            var result = await _interactor.ExtractMorphInfoFromMorphParametersAsync(inputPort, _cancellationTokenSource.Token);

            if (!IsOperationResultSuccess(result)) return;

            Terms = result.Result;
        }

        #endregion
    }
}
