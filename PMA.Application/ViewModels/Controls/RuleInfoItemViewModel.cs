// <copyright file="RuleInfoItemViewModel.cs" company="Andrey Pospelov">
//     Copyright 2017-2021 Andrey Pospelov. All rights reserved.
// </copyright>

using MediatR;
using Microsoft.Extensions.Logging;
using PMA.Application.ViewModels.Base;
using PMA.Domain.Interfaces.Locators;
using PMA.Domain.Interfaces.ViewModels.Controls;
using PMA.Utils.Extensions;

namespace PMA.Application.ViewModels.Controls
{
    /// <summary>
    /// The rule information item view model class.
    /// </summary>
    public sealed class RuleInfoItemViewModel : ViewModelBase<RuleInfoItemViewModel>, IRuleInfoItemViewModel
    {
        /// <summary>
        /// Initializes the new instance of <see cref="RuleInfoItemViewModel"/> class.
        /// </summary>
        /// <param name="id">An rule ID.</param>
        /// <param name="description">A rule description.</param>
        /// <param name="serviceLocator">The service locator.</param>
        /// <param name="mediator">The mediator.</param>
        /// <param name="logger">The logger.</param>
        public RuleInfoItemViewModel(int id, string description, IServiceLocator serviceLocator, IMediator mediator, ILogger<RuleInfoItemViewModel> logger) : base(serviceLocator, mediator, logger)
        {
            Id = id;
            Description = description;

            Logger.LogInit();
        }

        #region Implementation of IRuleInfoItemViewModel

        /// <summary>
        /// Gets a rule ID.
        /// </summary>
        public int Id { get; }

        /// <summary>
        /// Gets a rule description.
        /// </summary>
        public string Description { get; }

        #endregion
    }
}
