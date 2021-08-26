// <copyright file="GetEntryIdControlViewModel.cs" company="Andrey Pospelov">
//     Copyright 2017-2021 Andrey Pospelov. All rights reserved.
// </copyright>

using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.Toolkit.Mvvm.Input;
using PMA.Application.ViewModels.Base;
using PMA.Domain.Interfaces.Interactors.Primary;
using PMA.Domain.Interfaces.Locators;
using PMA.Domain.Interfaces.ViewModels.Controls;
using PMA.Domain.Models;
using PMA.Domain.Notifications;
using PMA.Utils.Extensions;
using System.Windows.Input;

namespace PMA.Application.ViewModels.Controls
{
    /// <summary>
    /// The get entry ID control view model class.
    /// </summary>
    public sealed class GetEntryIdControlViewModel : ViewModelBase<GetEntryIdControlViewModel>, IGetEntryIdControlViewModel
    {
        /// <summary>
        /// The get entry ID view model interactor.
        /// </summary>
        private readonly IGetEntryIdInteractor _interactor;

        /// <summary>
        /// The morphological entry.
        /// </summary>
        private readonly MorphEntry _morphEntry;

        /// <summary>
        /// Initializes the new instance of <see cref="GetEntryIdControlViewModel"/> class.
        /// </summary>
        /// <param name="morphEntry">The morphological entry.</param>
        /// <param name="interactor">The get entry ID view model interactor.</param>
        /// <param name="serviceLocator">The service locator.</param>
        /// <param name="mediator">The mediator.</param>
        /// <param name="logger">The logger.</param>
        public GetEntryIdControlViewModel(MorphEntry morphEntry, IGetEntryIdInteractor interactor, IServiceLocator serviceLocator, IMediator mediator, ILogger<GetEntryIdControlViewModel> logger) : base(serviceLocator, mediator, logger)
        {
            _morphEntry = morphEntry;
            _interactor = interactor;

            Logger.LogInit();

            SelectCommand = new RelayCommand(Select);

            Id = _morphEntry.Id;
            Entry = _morphEntry.Entry;
            Parameters = GetParameters();
            Base = _morphEntry.Base.ToString().ToLower();
            Left = _morphEntry.Left is null ? string.Empty : _morphEntry.Left.Entry;
            Right = _morphEntry.Right is null ? string.Empty : _morphEntry.Right.Entry;
            IsVirtual = _morphEntry.IsVirtual.HasValue && _morphEntry.IsVirtual.Value;
        }

        #region Implementation of IGetEntryIdControlViewModel

        /// <summary>
        /// Gets a morphological entry ID.
        /// </summary>
        public int Id { get; }

        /// <summary>
        /// Gets a morphological entry.
        /// </summary>
        public string Entry { get; }

        /// <summary>
        /// Gets a morphological parameters.
        /// </summary>
        public string Parameters { get; }

        /// <summary>
        /// Gets a morphological base.
        /// </summary>
        public string Base { get; }

        /// <summary>
        /// Gets a left morphological entry.
        /// </summary>
        public string Left { get; }

        /// <summary>
        /// Gets a right morphological entry.
        /// </summary>
        public string Right { get; }

        /// <summary>
        /// Gets whether the morphological entry is virtual (doesn't exist in the live language) or not.
        /// </summary>
        public bool IsVirtual { get; }

        /// <summary>
        ///  Backing field for the IsSelected property.
        /// </summary>
        private bool _isSelected;

        /// <summary>
        /// Gets or sets whether the morphological entry is selected or not.
        /// </summary>
        public bool IsSelected
        {
            get => _isSelected;
            set => SetProperty(ref _isSelected, value);
        }

        /// <summary>
        /// Gets a command to select the morphological entry.
        /// </summary>
        public ICommand SelectCommand { get; }

        #endregion

        /// <summary>
        /// Selects the morphological entry.
        /// </summary>
        private void Select()
        {
            Mediator.Publish(new MorphEntryNotification
            {
                MorphEntry = _morphEntry
            });
        }

        /// <summary>
        /// Gets a morphological information from parameters.
        /// </summary>
        /// <returns></returns>
        private string GetParameters()
        {
            var result = _interactor.ExtractMorphInfoFromMorphParameters(_morphEntry.Parameters);

            if (!result.Success)
            {
                Logger.LogErrors(result.Messages);
            }

            return result.Result;
        }
    }
}
