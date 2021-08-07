// <copyright file="OptionViewModel.cs" company="Andrey Pospelov">
//     Copyright 2017-2021 Andrey Pospelov. All rights reserved.
// </copyright>

using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.Toolkit.Mvvm.Input;
using PMA.Application.ViewModels.Base;
using PMA.Domain.InputPorts;
using PMA.Domain.Interfaces.Interactors.Primary;
using PMA.Domain.Interfaces.Locators;
using PMA.Domain.Interfaces.ViewModels;
using PMA.Utils.Extensions;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        /// <param name="mediator">The mediator.</param>
        /// <param name="logger">The logger.</param>
        public OptionViewModel(IOptionInteractor interactor, IServiceLocator serviceLocator, IMediator mediator, ILogger<OptionViewModel> logger) : base(serviceLocator, mediator, logger)
        {
            _interactor = interactor;

            Logger.LogInit();

            SaveCommand = new RelayCommand(Save);
            AddTermCommand = new RelayCommand<string>(AddTerm);
            RemoveTermCommand = new RelayCommand<string>(RemoveTerm);
        }

        #region Implementation of IOptionViewModel

        /// <summary>
        /// Gets or sets a option allows you to select the way how the wordform will be analyzed: False – only successful solutions sorted by rating; True – all solutions, including unsuccessful ones.
        /// </summary>
        public bool DebugMode { get; set; }

        /// <summary>
        /// Gets or sets s selected root term index.
        /// </summary>
        public int SelectedRootTermIndex { get; set; }

        /// <summary>
        /// Gets a collection of root terms.
        /// </summary>
        public IList<string> RootTerms { get; private set; }

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

        #region Overrides of ViewModelBase

        /// <summary>
        /// Action when the view appears.
        /// </summary>
        public override void OnAppearing()
        {
            base.OnAppearing();

            var result = _interactor.GetCurrentOptionValues();

            if (!result.Success)
            {
                Logger.LogErrors(result.Messages);
                ShowErrorModalDialog(result.Messages);
            }
            else
            {
                DebugMode = result.Result.DebugMode;

                RootTerms = result.Result.RootTerms;

                SelectedRootTermIndex = result.Result.SelectedRootTermIndex;

                AvailableTerms = new ObservableCollection<string>(result.Result.AvailableTerms);

                ShownTerms = new ObservableCollection<string>(result.Result.ShownTerms);

                FreqRatingRatio = result.Result.FreqRatingRatio;
            }
        }

        #endregion

        /// <summary>
        /// Saves settings.
        /// </summary>
        private void Save()
        {
            var inputData = new OptionValueInputPort
            {
                DebugMode = DebugMode,
                RootTerms = RootTerms,
                SelectedRootTermIndex = SelectedRootTermIndex,
                AvailableTerms = AvailableTerms,
                ShownTerms = ShownTerms,
                FreqRatingRatio = FreqRatingRatio
            };

            var result = _interactor.SaveOptionValues(inputData);

            if (!result.Success)
            {
                Logger.LogErrors(result.Messages);
                ShowErrorModalDialog(result.Messages);
            }
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
    }
}
