// <copyright file="OptionViewModel.cs" company="Andrey Pospelov">
//     Copyright 2017-2021 Andrey Pospelov. All rights reserved.
// </copyright>

using Microsoft.Extensions.Logging;
using Microsoft.Toolkit.Mvvm.Input;
using Microsoft.Toolkit.Mvvm.Messaging;
using PMA.Application.ViewModels.Base;
using PMA.Domain.InputPorts;
using PMA.Domain.Interfaces.Interactors.Primary;
using PMA.Domain.Interfaces.Locators;
using PMA.Domain.Interfaces.ViewModels;
using PMA.Utils.Extensions;
using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;

namespace PMA.Application.ViewModels
{
    /// <summary>
    /// The option view model class.
    /// </summary>
    public sealed class OptionViewModel : ViewModelBase<OptionViewModel>, IOptionViewModel
    {
        /// <summary>
        /// The option view model interactor.
        /// </summary>
        private readonly IOptionInteractor _interactor;

        /// <summary>
        /// Initializes the new instance of <see cref="OptionViewModel"/> class.
        /// </summary>
        /// <param name="interactor">The option view model interactor.</param>
        /// <param name="serviceLocator">The service locator.</param>
        /// <param name="logger">The logger.</param>
        /// <param name="messenger">The messenger.</param>
        public OptionViewModel(IOptionInteractor interactor, IServiceLocator serviceLocator, ILogger<OptionViewModel> logger, IMessenger messenger) : base(serviceLocator, logger, messenger)
        {
            _interactor = interactor;

            Logger.LogInit();

            SaveCommand = new AsyncRelayCommand(SaveAsync);
            AddTermCommand = new RelayCommand<string>(AddTerm);
            RemoveTermCommand = new RelayCommand<string>(RemoveTerm);
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

            var result = await _interactor.GetCurrentOptionValuesAsync(_cancellationTokenSource.Token);

            if (!IsOperationResultSuccess(result)) return;

            DebugMode = result.Result.DebugMode;
            AvailableTerms = new ObservableCollection<string>(result.Result.AvailableTerms);
            ShownTerms = new ObservableCollection<string>(result.Result.ShownTerms);
            FreqRatingRatio = result.Result.FreqRatingRatio;
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

        #region Implementation of IOptionViewModel

        /// <summary>
        /// Gets or sets a option allows you to select the way how the wordform will be analyzed: False – only successful solutions sorted by rating; True – all solutions, including unsuccessful ones.
        /// </summary>
        public bool DebugMode { get; set; }

        /// <summary>
        /// Gets a collection of available terms.
        /// </summary>
        public ObservableCollection<string> AvailableTerms { get; private set; }

        /// <summary>
        /// Gets a collection of shown terms.
        /// </summary>
        public ObservableCollection<string> ShownTerms { get; private set; }

        /// <summary>
        /// Gets a frequency rating ratio.
        /// </summary>
        public double FreqRatingRatio { get; set; }

        /// <summary>
        /// Gets a command to save settings.
        /// </summary>
        public ICommand SaveCommand { get; }

        /// <summary>
        /// Gets a command to add a term to collection of shown terms.
        /// </summary>
        public ICommand AddTermCommand { get; }

        /// <summary>
        /// Gets a command to remove a term from collection of shown terms.
        /// </summary>
        public ICommand RemoveTermCommand { get; }

        #endregion

        #region Private Fields

        /// <summary>
        /// The cancellation token source.
        /// </summary>
        private CancellationTokenSource _cancellationTokenSource;

        #endregion

        #region Private Methods

        /// <summary>
        /// Saves settings.
        /// </summary>
        private async Task SaveAsync()
        {
            var inputData = new OptionValueInputPort
            {
                DebugMode = DebugMode,
                AvailableTerms = AvailableTerms,
                ShownTerms = ShownTerms,
                FreqRatingRatio = FreqRatingRatio
            };

            IsBusy = true;

            var result = await _interactor.SaveOptionValuesAsync(inputData, _cancellationTokenSource.Token);

            IsBusy = false;

            IsOperationResultSuccess(result);
        }

        /// <summary>
        /// Adds a term to collection of shown terms.
        /// </summary>
        /// <param name="termName">The term name.</param>
        private void AddTerm(string termName)
        {
            AvailableTerms.Remove(termName);
            ShownTerms.Add(termName);
        }

        /// <summary>
        /// Removes a term from collection of shown terms.
        /// </summary>
        /// <param name="termName">The term name.</param>
        private void RemoveTerm(string termName)
        {
            AvailableTerms.Add(termName);
            ShownTerms.Remove(termName);
        }

        #endregion
    }
}
