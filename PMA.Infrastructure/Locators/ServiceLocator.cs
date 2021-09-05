// <copyright file="ServiceLocator.cs" company="Andrey Pospelov">
//     Copyright 2017-2021 Andrey Pospelov. All rights reserved.
// </copyright>

using PMA.Domain.Interfaces.Locators;
using PMA.Domain.Interfaces.Services;

namespace PMA.Infrastructure.Locators
{
    /// <summary>
    /// The service locator class.
    /// </summary>
    public class ServiceLocator : IServiceLocator
    {
        /// <summary>
        /// Initializes the new instance of <see cref="ServiceLocator"/> class.
        /// </summary>
        /// <param name="excelDataService">The excel data service.</param>
        /// <param name="logMessageService">The log message service.</param>
        /// <param name="settingService">The setting service.</param>
        /// <param name="translateService">The translate service.</param>
        /// <param name="modalDialogService">The modal dialog service.</param>
        public ServiceLocator(IExcelDataService excelDataService,
            ILogMessageService logMessageService,
            ISettingService settingService,
            ITranslateService translateService,
            IModalDialogService modalDialogService)
        {
            ExcelDataService = excelDataService;
            LogMessageService = logMessageService;
            SettingService = settingService;
            TranslateService = translateService;
            ModalDialogService = modalDialogService;
        }

        #region Implementation of IServiceLocator

        /// <summary>
        /// Gets an excel data service.
        /// </summary>
        public IExcelDataService ExcelDataService { get; }

        /// <summary>
        /// Gets a log message service.
        /// </summary>
        public ILogMessageService LogMessageService { get; }

        /// <summary>
        /// Gets a setting service.
        /// </summary>
        public ISettingService SettingService { get; }

        /// <summary>
        /// Gets a translate service.
        /// </summary>
        public ITranslateService TranslateService { get; }

        /// <summary>
        /// Gets a modal dialog service.
        /// </summary>
        public IModalDialogService ModalDialogService { get; }

        #endregion
    }
}
