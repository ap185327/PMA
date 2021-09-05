// <copyright file="MorphSolutionViewModel.cs" company="Andrey Pospelov">
//     Copyright 2017-2021 Andrey Pospelov. All rights reserved.
// </copyright>

using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.Toolkit.Mvvm.Messaging;
using PMA.Application.ViewModels.Base;
using PMA.Domain.Enums;
using PMA.Domain.InputPorts;
using PMA.Domain.Interfaces.Interactors.Primary;
using PMA.Domain.Interfaces.Locators;
using PMA.Domain.Interfaces.ViewModels;
using PMA.Domain.Interfaces.ViewModels.Controls;
using PMA.Domain.Notifications;
using PMA.Utils.Extensions;
using System.Threading;
using System.Threading.Tasks;

namespace PMA.Application.ViewModels
{
    /// <summary>
    /// The morphological solution view model class.
    /// </summary>
    public sealed class MorphSolutionViewModel : ViewModelBase<MorphSolutionViewModel>, IMorphSolutionViewModel, INotificationHandler<MorphParserNotification>
    {
        /// <summary>
        /// The tree node view model interactor.
        /// </summary>
        private readonly ITreeNodeInteractor _interactor;

        /// <summary>
        /// Initializes the new instance of <see cref="MorphSolutionViewModel"/> class.
        /// </summary>
        /// <param name="interactor">The tree node view model interactor.</param>
        /// <param name="serviceLocator">The service locator.</param>
        /// <param name="logger">The logger.</param>
        /// <param name="messenger">The messenger.</param>
        public MorphSolutionViewModel(ITreeNodeInteractor interactor, IServiceLocator serviceLocator, ILogger<MorphSolutionViewModel> logger, IMessenger messenger) : base(serviceLocator, logger, messenger)
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

        #region Implementation of IMorphSolutionViewModel

        /// <summary>
        /// Gets a main wordform node view model.
        /// </summary>
        public IWordFormTreeNodeViewModel MainTreeNode
        {
            get => _mainTreeNode;
            private set => SetProperty(ref _mainTreeNode, value);
        }

        #endregion

        #region Implementation of INotificationHandler<in MorphParserNotification>

        /// <summary>Handles a notification</summary>
        /// <param name="notification">The notification</param>
        /// <param name="cancellationToken">Cancellation token</param>
        public async Task Handle(MorphParserNotification notification, CancellationToken cancellationToken)
        {
            if (!IsActive) return;

            switch (notification.State)
            {
                case ProcessState.InProgress:
                    IsBusy = true;

                    MainTreeNode = null;
                    return;
                case ProcessState.Completed:
                    if (notification.Result is null) return;

                    IsBusy = true;

                    var inputPort = new GetWordFormTreeNodeInputPort
                    {
                        WordForm = notification.Result
                    };

                    var result = await _interactor.GetWordFormTreeNodeAsync(inputPort, _cancellationTokenSource.Token);

                    IsBusy = false;

                    if (!IsOperationResultSuccess(result)) return;

                    MainTreeNode = result.Result;

                    return;
            }
        }

        #endregion

        #region Private Fields

        /// <summary>
        ///  Backing field for the MainTreeNode property.
        /// </summary>
        private IWordFormTreeNodeViewModel _mainTreeNode;

        /// <summary>
        /// The cancellation token source.
        /// </summary>
        private CancellationTokenSource _cancellationTokenSource;

        #endregion
    }
}
