// <copyright file="ExcelDataService.cs" company="Andrey Pospelov">
//     Copyright 2017-2021 Andrey Pospelov. All rights reserved.
// </copyright>

using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.Extensions.Logging;
using PMA.Domain.Interfaces.Services;
using PMA.Infrastructure.Services.Base;
using PMA.Utils.Exceptions;
using PMA.Utils.Extensions;
using System.Data;
using System.Linq;

namespace PMA.Infrastructure.Services
{
    /// <summary>
    /// The excel data service class.
    /// </summary>
    public class ExcelDataService : ServiceBase<ExcelDataService>, IExcelDataService
    {
        /// <summary>
        /// The excel document.
        /// </summary>
        private SpreadsheetDocument _document;

        /// <summary>
        /// Initializes the new instance of <see cref="ExcelDataService"/> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        public ExcelDataService(ILogger<ExcelDataService> logger) : base(logger)
        {
            Logger.LogInit();
        }

        #region Implementation of IExcelDataService

        /// <summary>
        /// Gets whether an excel file is opened or not.
        /// </summary>
        public bool IsOpened => _document is not null;

        /// <summary>
        /// Opens a new excel file.
        /// </summary>
        /// <param name="path">A excel file path.</param>
        public void Open(string path)
        {
            _document = SpreadsheetDocument.Open(path, false);
        }

        /// <summary>
        /// Closes an excel file.
        /// </summary>
        public void Close()
        {
            if (_document is null) return;

            _document.Close();
            _document.Dispose();
            _document = null;
        }

        /// <summary>
        /// Gets a data from the excel file by table name.
        /// </summary>
        /// <param name="tableName">A table name.</param>
        /// <returns>A data table.</returns>
        public DataTable GetData(string tableName)
        {
            foreach (var worksheetPart in _document.WorkbookPart.WorksheetParts)
            {
                foreach (var tableDefinitionPart in worksheetPart.TableDefinitionParts)
                {
                    if (tableDefinitionPart.Table.Name != tableName) continue;

                    var dataTable = new DataTable
                    {
                        TableName = tableName
                    };

                    foreach (var tableColumn in tableDefinitionPart.Table.TableColumns)
                    {
                        var column = (TableColumn)tableColumn;
                        var dataColumn = new DataColumn
                        {
                            DataType = typeof(string),
                            ColumnName = column.Name
                        };

                        dataTable.Columns.Add(dataColumn);
                    }

                    var sheetData = worksheetPart.Worksheet.Descendants<SheetData>().First();

                    int rowIndex = 0;

                    foreach (var row in sheetData.Elements<Row>())
                    {
                        int cellIndex = 0;

                        var dataRow = dataTable.NewRow();

                        foreach (var cell in row.Elements<Cell>())
                        {
                            if (CellReferenceToIndex(cell) == cellIndex)
                            {
                                dataRow[cellIndex] = GetCellValue(cell);
                            }
                            else
                            {
                                do
                                {
                                    if (CellReferenceToIndex(cell) == cellIndex)
                                    {
                                        dataRow[cellIndex] = GetCellValue(cell);
                                        break;
                                    }

                                    dataRow[cellIndex] = string.Empty;

                                    cellIndex++;

                                    if (cellIndex == dataTable.Columns.Count) break;
                                } while (true);
                            }

                            cellIndex++;

                            if (cellIndex >= dataTable.Columns.Count) break;
                        }

                        if (rowIndex == 63) rowIndex++;

                        dataTable.Rows.Add(dataRow);

                        rowIndex++;
                    }

                    dataTable.Rows.Remove(dataTable.Rows[0]);

                    return dataTable;
                }
            }

            throw new CustomException($"Cannot find {tableName} table");
        }

        #endregion

        /// <summary>
        /// Gets a cell value.
        /// </summary>
        /// <param name="cell">The cell.</param>
        /// <returns>A cell value.</returns>
        private string GetCellValue(CellType cell)
        {
            if (cell?.DataType == null || cell.DataType.Value != CellValues.SharedString) return cell?.CellValue != null ? cell.CellValue.InnerText : string.Empty;

            var stringTable = _document.WorkbookPart.GetPartsOfType<SharedStringTablePart>().FirstOrDefault();

            if (stringTable != null) return stringTable.SharedStringTable.ElementAt(int.Parse(cell.InnerText)).InnerText;

            return cell.CellValue != null ? cell.CellValue.InnerText : string.Empty;
        }

        /// <summary>
        /// Gets a cell index.
        /// </summary>
        /// <param name="cell">The cell.</param>
        /// <returns>A cell index.</returns>
        private static int CellReferenceToIndex(CellType cell)
        {
            int index = 0;

            string reference = cell.CellReference.ToString().ToUpper();

            foreach (char ch in reference)
            {
                if (char.IsLetter(ch))
                {
                    int value = ch - 'A';
                    index = index == 0 ? value : (index + 1) * 26 + value;
                }
                else
                {
                    return index;
                }
            }

            return index;
        }
    }
}
