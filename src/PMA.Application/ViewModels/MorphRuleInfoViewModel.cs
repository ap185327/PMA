// <copyright file="MorphRuleInfoViewModel.cs" company="Andrey Pospelov">
//     Copyright 2017-2021 Andrey Pospelov. All rights reserved.
// </copyright>

using Microsoft.Extensions.Logging;
using Microsoft.Toolkit.Mvvm.Messaging;
using PMA.Application.ViewModels.Base;
using PMA.Domain.Interfaces.Interactors.Primary;
using PMA.Domain.Interfaces.Locators;
using PMA.Domain.Interfaces.ViewModels;
using PMA.Domain.Interfaces.ViewModels.Controls;
using PMA.Domain.Messages;
using PMA.Utils.Extensions;
using System.Collections.ObjectModel;
using System.Threading;

namespace PMA.Application.ViewModels
{
    /// <summary>
    /// The morphological rule information view model class.
    /// </summary>
    public sealed class MorphRuleInfoViewModel : ViewModelBase<MorphRuleInfoViewModel>, IMorphRuleInfoViewModel, IRecipient<MorphRuleMessage>
    {
        /// <summary>
        /// The morphological rule view model interactor.
        /// </summary>
        private readonly IMorphRuleInteractor _interactor;

        /// <summary>
        /// Initializes the new instance of <see cref="MorphRuleInfoViewModel"/> class.
        /// </summary>
        /// <param name="interactor">The morphological rule view model interactor.</param>
        /// <param name="serviceLocator">The service locator.</param>
        /// <param name="logger">The logger.</param>
        /// <param name="messenger">The messenger.</param>
        public MorphRuleInfoViewModel(IMorphRuleInteractor interactor, IServiceLocator serviceLocator, ILogger<MorphRuleInfoViewModel> logger, IMessenger messenger) : base(serviceLocator, logger, messenger)
        {
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

        #region Implementation of IMorphRuleInfoViewModel

        /// <summary>
        /// Gets a collection of morphological and sandhi rule info item view models.
        /// </summary>
        public ObservableCollection<IRuleInfoItemViewModel> Rules { get; } = new();

        #endregion

        #region Implementation of IRecipient<in MorphRuleMessage>

        /// <summary>
        /// Receives a given <see cref="MorphRuleMessage"/> message instance.
        /// </summary>
        /// <param name="message">The message being received.</param>
        public async void Receive(MorphRuleMessage message)
        {
            Rules.Clear();

            if (message.MorphRules is not null)
            {
                var result = await _interactor.GetMorphRuleInfoItemViewModelsAsync(message.MorphRules, _cancellationTokenSource.Token);

                if (!IsOperationResultSuccess(result)) return;

                foreach (var rule in result.Result)
                {
                    Rules.Add(rule);
                }
            }

            if (message.SandhiMatches is not null)
            {
                var result = await _interactor.GetSandhiRuleInfoItemViewModelsAsync(message.SandhiMatches, _cancellationTokenSource.Token);

                if (!IsOperationResultSuccess(result)) return;

                foreach (var rule in result.Result)
                {
                    Rules.Add(rule);
                }
            }
        }

        #endregion

        #region Private Fields

        /// <summary>
        /// The cancellation token source.
        /// </summary>
        private CancellationTokenSource _cancellationTokenSource;

        #endregion
    }
}
