// <copyright file="UpdateDbViewModel.cs" company="Andrey Pospelov">
//     Copyright 2017-2021 Andrey Pospelov. All rights reserved.
// </copyright>

using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.Toolkit.Mvvm.Input;
using PMA.Application.ViewModels.Base;
using PMA.Domain.Enums;
using PMA.Domain.InputPorts;
using PMA.Domain.Interfaces.Interactors.Primary;
using PMA.Domain.Interfaces.Locators;
using PMA.Domain.Interfaces.ViewModels;
using PMA.Domain.Notifications;
using PMA.Utils.Extensions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;

namespace PMA.Application.ViewModels
{
    /// <summary>
    /// The database update view model class.
    /// </summary>
    public sealed class UpdateDbViewModel : ViewModelBase<UpdateDbViewModel>, IUpdateDbViewModel, INotificationHandler<UpdateDbNotification>
    {
        /// <summary>
        /// The update database view model interactor.
        /// </summary>
        private readonly IUpdateDbInteractor _interactor;

        /// <summary>
        /// Initializes the new instance of <see cref="UpdateDbViewModel"/> class.
        /// </summary>
        /// <param name="interactor">The update database view model interactor.</param>
        /// <param name="serviceLocator">The service locator.</param>
        /// <param name="mediator">The mediator.</param>
        /// <param name="logger">The logger.</param>
        public UpdateDbViewModel(IUpdateDbInteractor interactor, IServiceLocator serviceLocator, IMediator mediator, ILogger<UpdateDbViewModel> logger) : base(serviceLocator, mediator, logger)
        {
            _interactor = interactor;

            Logger.LogInit();

            StartCommand = new RelayCommand(Start);
            StopCommand = new RelayCommand(Stop);
            ResetCommand = new RelayCommand(Reset);
        }

        #region Overrides of ViewModelBase

        /// <summary>
        /// Action when the view appears.
        /// </summary>
        public override void OnAppearing()
        {
            base.OnAppearing();

            IsSandhiGroupDbTableChecked = ServiceLocator.SettingService.GetValue<bool>("UpdateDbViewModel.IsSandhiGroupDbTableChecked");
            IsSandhiRuleDbTableChecked = ServiceLocator.SettingService.GetValue<bool>("UpdateDbViewModel.IsSandhiRuleDbTableChecked");
            IsMorphRuleDbTableChecked = ServiceLocator.SettingService.GetValue<bool>("UpdateDbViewModel.IsMorphRuleDbTableChecked");
            IsMorphCombinationDbTableChecked = ServiceLocator.SettingService.GetValue<bool>("UpdateDbViewModel.IsMorphCombinationDbTableChecked");
            DataFilePath = ServiceLocator.SettingService.GetValue<string>("UpdateDbViewModel.DataFilePath");
        }

        /// <summary>
        /// Action when the view disappears.
        /// </summary>
        public override void OnDisappearing()
        {
            base.OnDisappearing();

            ServiceLocator.SettingService.SetValue("UpdateDbViewModel.IsSandhiGroupDbTableChecked", IsSandhiGroupDbTableChecked);
            ServiceLocator.SettingService.SetValue("UpdateDbViewModel.IsSandhiRuleDbTableChecked", IsSandhiRuleDbTableChecked);
            ServiceLocator.SettingService.SetValue("UpdateDbViewModel.IsMorphRuleDbTableChecked", IsMorphRuleDbTableChecked);
            ServiceLocator.SettingService.SetValue("UpdateDbViewModel.IsMorphCombinationDbTableChecked", IsMorphCombinationDbTableChecked);
            ServiceLocator.SettingService.SetValue("UpdateDbViewModel.DataFilePath", DataFilePath);
        }

        #endregion

        #region Implementation of IDbUpdateViewModel

        /// <summary>
        ///  Backing field for the IsSandhiGroupDbTableChecked property.
        /// </summary>
        private bool _isSandhiGroupDbTableChecked;

        /// <summary>
        /// Gets or sets whether the sandhi group database table is checked or not.
        /// </summary>
        public bool IsSandhiGroupDbTableChecked
        {
            get => _isSandhiGroupDbTableChecked;
            set => SetProperty(ref _isSandhiGroupDbTableChecked, value);
        }

        /// <summary>
        ///  Backing field for the IsSandhiRuleDbTableChecked property.
        /// </summary>
        private bool _isSandhiRuleDbTableChecked;

        /// <summary>
        /// Gets or sets whether the sandhi rule database table is checked or not.
        /// </summary>
        public bool IsSandhiRuleDbTableChecked
        {
            get => _isSandhiRuleDbTableChecked;
            set => SetProperty(ref _isSandhiRuleDbTableChecked, value);
        }

        /// <summary>
        ///  Backing field for the IsMorphRuleDbTableChecked property.
        /// </summary>
        private bool _isMorphRuleDbTableChecked;

        /// <summary>
        /// Gets or sets whether the morphological rule database table is checked or not.
        /// </summary>
        public bool IsMorphRuleDbTableChecked
        {
            get => _isMorphRuleDbTableChecked;
            set => SetProperty(ref _isMorphRuleDbTableChecked, value);
        }

        /// <summary>
        ///  Backing field for the IsMorphCombinationDbTableChecked property.
        /// </summary>
        private bool _isMorphCombinationDbTableChecked;

        /// <summary>
        /// Gets or sets whether the morphological combination database table is checked or not.
        /// </summary>
        public bool IsMorphCombinationDbTableChecked
        {
            get => _isMorphCombinationDbTableChecked;
            set => SetProperty(ref _isMorphCombinationDbTableChecked, value);
        }

        /// <summary>
        ///  Backing field for the DataFilePath property.
        /// </summary>
        private string _dataFilePath;

        /// <summary>
        /// Gets or sets a file path with PMA tables.
        /// </summary>
        public string DataFilePath
        {
            get => _dataFilePath;
            set => SetProperty(ref _dataFilePath, value);
        }

        /// <summary>
        /// Gets a command to start importing.
        /// </summary>
        public ICommand StartCommand { get; }

        /// <summary>
        /// Gets a command to stop importing.
        /// </summary>
        public ICommand StopCommand { get; }

        /// <summary>
        /// Gets a command to reset view model values.
        /// </summary>
        public ICommand ResetCommand { get; }

        #endregion

        #region Implementation of INotificationHandler<in UpdateDbNotification>

        /// <summary>Handles a notification</summary>
        /// <param name="notification">The notification</param>
        /// <param name="cancellationToken">Cancellation token</param>
        public Task Handle(UpdateDbNotification notification, CancellationToken cancellationToken)
        {
            switch (notification.State)
            {
                case ProcessState.Canceling:
                case ProcessState.InProgress:
                    IsBusy = true;
                    break;
                case ProcessState.Idle:
                case ProcessState.Canceled:
                case ProcessState.Completed:
                case ProcessState.Error:
                    IsBusy = false;
                    break;
            }

            return Task.CompletedTask;
        }

        #endregion

        /// <summary>
        /// Start importing.
        /// </summary>
        private void Start()
        {
            var inputData = new UpdateDbInputPort
            {
                DataFilePath = DataFilePath,
                IsSandhiGroupDbTableChecked = IsSandhiGroupDbTableChecked,
                IsSandhiRuleDbTableChecked = IsSandhiRuleDbTableChecked,
                IsMorphRuleDbTableChecked = IsMorphRuleDbTableChecked,
                IsMorphCombinationDbTableChecked = IsMorphCombinationDbTableChecked
            };

            var result = _interactor.StartDbUpdating(inputData);

            if (!result.Success)
            {
                Logger.LogErrors(result.Messages);
            }
        }

        /// <summary>
        /// Stops importing.
        /// </summary>
        private void Stop()
        {
            var result = _interactor.StopDbUpdating();

            if (!result.Success)
            {
                Logger.LogErrors(result.Messages);
            }
        }

        /// <summary>
        /// Reset view model values.
        /// </summary>
        private void Reset()
        {
            if (!IsBusy)
            {
                IsSandhiGroupDbTableChecked = true;
                IsSandhiRuleDbTableChecked = true;
                IsMorphRuleDbTableChecked = true;
                IsMorphCombinationDbTableChecked = true;
                DataFilePath = string.Empty;
            }
        }
    }
}
