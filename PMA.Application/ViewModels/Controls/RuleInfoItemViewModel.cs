// <copyright file="RuleInfoItemViewModel.cs" company="Andrey Pospelov">
//     Copyright 2017-2021 Andrey Pospelov. All rights reserved.
// </copyright>

using Microsoft.Extensions.Logging;
using Microsoft.Toolkit.Mvvm.Messaging;
using PMA.Application.ViewModels.Base;
using PMA.Domain.Enums;
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
        /// <param name="type">The rule type.</param>
        /// <param name="id">The rule ID.</param>
        /// <param name="description">The rule description.</param>
        /// <param name="serviceLocator">The service locator.</param>
        /// <param name="logger">The logger.</param>
        /// <param name="messenger">The messenger.</param>
        public RuleInfoItemViewModel(RuleType type, int id, string description, IServiceLocator serviceLocator, ILogger<RuleInfoItemViewModel> logger, IMessenger messenger) : base(serviceLocator, logger, messenger)
        {
            Type = type;
            Id = id;
            Description = description;

            Logger.LogInit();
        }

        #region Implementation of IRuleInfoItemViewModel

        /// <summary>
        /// Gets a rule type.
        /// </summary>
        public RuleType Type { get; }

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
