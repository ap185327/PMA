// <copyright file="IExcelDataService.cs" company="Andrey Pospelov">
//     Copyright 2017-2021 Andrey Pospelov. All rights reserved.
// </copyright>

using PMA.Domain.Interfaces.Services.Base;
using System.Data;

namespace PMA.Domain.Interfaces.Services
{
    /// <summary>
    /// Initializes a new instance of the <see cref="IExcelDataService"/> interfacing class.
    /// </summary>
    public interface IExcelDataService : IService
    {
        /// <summary>
        /// Gets whether an excel file is opened or not.
        /// </summary>
        bool IsOpened { get; }

        /// <summary>
        /// Opens a new excel file.
        /// </summary>
        /// <param name="path">A excel file path.</param>
        void Open(string path);

        /// <summary>
        /// Closes an excel file.
        /// </summary>
        void Close();

        /// <summary>
        /// Gets a data from the excel file by table name.
        /// </summary>
        /// <param name="tableName">A table name.</param>
        /// <returns>A data table.</returns>
        DataTable GetData(string tableName);
    }
}
