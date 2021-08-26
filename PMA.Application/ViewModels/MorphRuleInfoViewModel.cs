// <copyright file="MorphRuleInfoViewModel.cs" company="Andrey Pospelov">
//     Copyright 2017-2021 Andrey Pospelov. All rights reserved.
// </copyright>

using MediatR;
using Microsoft.Extensions.Logging;
using PMA.Application.ViewModels.Base;
using PMA.Application.ViewModels.Controls;
using PMA.Domain.Interfaces.Locators;
using PMA.Domain.Interfaces.ViewModels;
using PMA.Domain.Interfaces.ViewModels.Controls;
using PMA.Domain.Models;
using PMA.Domain.Notifications;
using PMA.Utils.Extensions;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace PMA.Application.ViewModels
{
    /// <summary>
    /// The morphological rule information view model class.
    /// </summary>
    public sealed class MorphRuleInfoViewModel : ViewModelBase<MorphRuleInfoViewModel>, IMorphRuleInfoViewModel, INotificationHandler<MorphRuleNotification>
    {
        /// <summary>
        /// The control logger.
        /// </summary>
        private readonly ILogger<RuleInfoItemViewModel> _controlLogger;

        /// <summary>
        /// Initializes the new instance of <see cref="MorphRuleInfoViewModel"/> class.
        /// </summary>
        /// <param name="serviceLocator">The service locator.</param>
        /// <param name="mediator">The mediator.</param>
        /// <param name="controlLogger">The control logger.</param>
        /// <param name="logger">The logger.</param>
        public MorphRuleInfoViewModel(IServiceLocator serviceLocator, IMediator mediator, ILogger<RuleInfoItemViewModel> controlLogger, ILogger<MorphRuleInfoViewModel> logger) : base(serviceLocator, mediator, logger)
        {
            _controlLogger = controlLogger;

            Logger.LogInit();
        }

        #region Implementation of IMorphRuleInfoViewModel

        /// <summary>
        /// Gets a collection of morphological rule info item view models.
        /// </summary>
        public ObservableCollection<IRuleInfoItemViewModel> MorphRules { get; } = new();

        /// <summary>
        /// Gets a collection of sandhi rule info item view models.
        /// </summary>
        public ObservableCollection<IRuleInfoItemViewModel> SandhiRules { get; } = new();

        #endregion

        #region Implementation of INotificationHandler<in MorphRuleNotification>

        /// <summary>Handles a notification</summary>
        /// <param name="notification">The notification</param>
        /// <param name="cancellationToken">Cancellation token</param>
        public Task Handle(MorphRuleNotification notification, CancellationToken cancellationToken)
        {
            var morphRules = notification.MorphRules;
            var sandhiMatches = notification.SandhiMatches;
            var sandhiRules = new List<SandhiRule>();

            MorphRules.Clear();
            SandhiRules.Clear();

            if (morphRules != null)
            {
                morphRules = morphRules.DistinctBy(x => x.Id).ToList();

                foreach (var morphRule in morphRules)
                {
                    MorphRules.Add(new RuleInfoItemViewModel(morphRule.Id, morphRule.Description, ServiceLocator, Mediator, _controlLogger));
                }
            }

            if (sandhiMatches != null)
            {
                foreach (var sandhiMatch in sandhiMatches)
                {
                    if (sandhiMatch.Rules != null)
                    {
                        sandhiRules.AddRange(sandhiMatch.Rules);
                    }
                }
            }

            sandhiRules = sandhiRules.DistinctBy(x => x.Id).ToList();

            foreach (var sandhiRule in sandhiRules)
            {
                SandhiRules.Add(new RuleInfoItemViewModel(sandhiRule.Id, sandhiRule.Description, ServiceLocator, Mediator, _controlLogger));
            }

            return Task.CompletedTask;
        }

        #endregion
    }
}
