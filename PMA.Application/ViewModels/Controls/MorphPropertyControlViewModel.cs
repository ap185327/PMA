// <copyright file="MorphPropertyControlViewModel.cs" company="Andrey Pospelov">
//     Copyright 2017-2021 Andrey Pospelov. All rights reserved.
// </copyright>

using MediatR;
using Microsoft.Extensions.Logging;
using PMA.Application.ViewModels.Base;
using PMA.Domain.Constants;
using PMA.Domain.Interfaces.Interactors.Primary;
using PMA.Domain.Interfaces.Locators;
using PMA.Domain.Interfaces.ViewModels.Controls;
using PMA.Domain.Models;
using PMA.Utils.Extensions;
using System.Collections.Generic;

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
        /// The morphological parameter for this property.
        /// </summary>
        private readonly MorphParameter _morphParameter;

        /// <summary>
        /// Initializes the new instance of <see cref="MorphPropertyControlViewModel"/> class.
        /// </summary>
        /// <param name="index">The property index.</param>
        /// <param name="interactor">The morphological property view model interactor.</param>
        /// <param name="serviceLocator">The service locator.</param>
        /// <param name="mediator">The mediator.</param>
        /// <param name="logger">The logger.</param>
        public MorphPropertyControlViewModel(int index, IMorphPropertyInteractor interactor, IServiceLocator serviceLocator, IMediator mediator, ILogger<MorphPropertyControlViewModel> logger) : base(serviceLocator, mediator, logger)
        {
            Index = index;
            _interactor = interactor;

            Logger.LogInit();

            var result = _interactor.GetMorphParameterById(Index);

            if (!result.Success)
            {
                Logger.LogErrors(result.Messages);
            }
            else
            {
                _morphParameter = result.Result;
            }
        }

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
        public string Name => _morphParameter.Name;

        /// <summary>
        /// Gets the property description.
        /// </summary>
        public string Description => _morphParameter.Description;

        /// <summary>
        ///  Backing field for the SelectedIndex property.
        /// </summary>
        private int _selectedIndex;

        /// <summary>
        /// Gets or sets the selected term index.
        /// </summary>
        public int SelectedIndex
        {
            get => _selectedIndex;
            set
            {
                Logger.LogDebug($"Try to set SelectedIndex: {value} (SelectedIndex = {_selectedIndex}; Index = {Index}; Name = {Name}; TermEntries = {string.Join(",", TermEntries)}; TermIds = {string.Join(",", TermIds)})");

                SetProperty(ref _selectedIndex, value);
            }
        }

        /// <summary>
        ///  Backing field for the TermEntries property.
        /// </summary>
        private IList<string> _termEntries;

        /// <summary>
        /// Gets the collection of property term entries.
        /// </summary>
        public IList<string> TermEntries
        {
            get => _termEntries;
            private set => SetProperty(ref _termEntries, value);
        }

        /// <summary>
        ///  Backing field for the TermIds property.
        /// </summary>
        private IList<byte> _termIds = new List<byte> { MorphConstants.UnknownTermId };

        /// <summary>
        /// Gets or sets the collection of property term IDs.
        /// </summary>
        public IList<byte> TermIds
        {
            get => _termIds;
            set
            {
                byte oldSelectedValue = _termIds[SelectedIndex];

                if (!SetProperty(ref _termIds, value)) return;

                UpdateTermProperties(oldSelectedValue);
            }
        }

        #endregion

        /// <summary>
        /// Updates term properties.
        /// </summary>
        /// <param name="oldSelectedValue">The old selected term ID value.</param>
        private void UpdateTermProperties(byte oldSelectedValue)
        {
            var result = _interactor.GetTermEntriesByIds(_termIds, _morphParameter.UseAltPropertyEntry);

            if (!result.Success)
            {
                Logger.LogErrors(result.Messages);
            }
            else
            {
                TermEntries = result.Result;
            }

            int newSelectedIndex = _termIds.IndexOf(oldSelectedValue);

            if (newSelectedIndex < 0)
            {
                if (SelectedIndex == 0)
                {
                    OnPropertyChanged(nameof(SelectedIndex));
                }
                else
                {
                    SelectedIndex = 0;
                }
            }
            else
            {
                SelectedIndex = newSelectedIndex;
            }
        }
    }
}
