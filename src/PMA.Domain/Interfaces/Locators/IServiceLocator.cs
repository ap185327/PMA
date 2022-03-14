// <copyright file="IServiceLocator.cs" company="Andrey Pospelov">
//     Copyright 2017-2021 Andrey Pospelov. All rights reserved.
// </copyright>

using PMA.Domain.Interfaces.Services;

namespace PMA.Domain.Interfaces.Locators
{
    /// <summary>
    /// Initializes a new instance of the <see cref="IServiceLocator"/> interfacing class.
    /// </summary>
    public interface IServiceLocator
    {
        /// <summary>
        /// Gets an excel data service.
        /// </summary>
        IExcelDataService ExcelDataService { get; }

        /// <summary>
        /// Gets a log message service.
        /// </summary>
        ILogMessageService LogMessageService { get; }

        /// <summary>
        /// Gets a setting service.
        /// </summary>
        ISettingService SettingService { get; }

        /// <summary>
        /// Gets a translate service.
        /// </summary>
        ITranslateService TranslateService { get; }

        /// <summary>
        /// Gets a modal dialog service.
        /// </summary>
        IModalDialogService ModalDialogService { get; }
    }
}
