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
        /// <param name="settingService">A setting service.</param>
        /// <param name="translateService">A translate service.</param>
        public ServiceLocator(IExcelDataService excelDataService,
            ILogMessageService logMessageService,
            ISettingService settingService,
            ITranslateService translateService)
        {
            ExcelDataService = excelDataService;
            LogMessageService = logMessageService;
            SettingService = settingService;
            TranslateService = translateService;
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

        #endregion
    }
}
