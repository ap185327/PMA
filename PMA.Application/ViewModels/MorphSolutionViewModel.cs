// <copyright file="MorphSolutionViewModel.cs" company="Andrey Pospelov">
//     Copyright 2017-2021 Andrey Pospelov. All rights reserved.
// </copyright>

using MediatR;
using Microsoft.Extensions.Logging;
using PMA.Application.ViewModels.Base;
using PMA.Application.ViewModels.Controls;
using PMA.Domain.Enums;
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
        /// The wordform logger.
        /// </summary>
        private readonly ILogger<WordFormTreeNodeViewModel> _wordFormLogger;

        /// <summary>
        /// The solution logger.
        /// </summary>
        private readonly ILogger<SolutionTreeNodeViewModel> _solutionLogger;

        /// <summary>
        /// Initializes the new instance of <see cref="MorphSolutionViewModel"/> class.
        /// </summary>
        /// <param name="interactor">The tree node view model interactor.</param>
        /// <param name="serviceLocator">The service locator.</param>
        /// <param name="mediator">The mediator.</param>
        /// <param name="wordFormLogger">The wordform logger.</param>
        /// <param name="solutionLogger">The solution logger.</param>
        /// <param name="logger">The logger.</param>
        public MorphSolutionViewModel(ITreeNodeInteractor interactor,
            IServiceLocator serviceLocator,
            IMediator mediator,
            ILogger<WordFormTreeNodeViewModel> wordFormLogger,
            ILogger<SolutionTreeNodeViewModel> solutionLogger,
            ILogger<MorphSolutionViewModel> logger) : base(serviceLocator,
            mediator,
            logger)
        {
            _interactor = interactor;
            _wordFormLogger = wordFormLogger;
            _solutionLogger = solutionLogger;

            Logger.LogInit();
        }

        #region Implementation of IMorphSolutionViewModel

        /// <summary>
        ///  Backing field for the MainTreeNode property.
        /// </summary>
        private IWordFormTreeNodeViewModel _mainTreeNode;

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
        public Task Handle(MorphParserNotification notification, CancellationToken cancellationToken)
        {
            MainTreeNode = notification.State switch
            {
                ProcessState.InProgress => null,
                ProcessState.Completed => notification.Result is null
                    ? null
                    : new WordFormTreeNodeViewModel(null, notification.Result, _interactor, ServiceLocator, Mediator, _solutionLogger, _wordFormLogger),
                _ => MainTreeNode
            };

            return Task.CompletedTask;
        }

        #endregion
    }
}
