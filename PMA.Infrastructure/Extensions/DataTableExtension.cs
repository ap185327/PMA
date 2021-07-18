// <copyright file="DataTableExtension.cs" company="Andrey Pospelov">
//     Copyright 2017-2021 Andrey Pospelov. All rights reserved.
// </copyright>

using PMA.Domain.Enums;
using PMA.Infrastructure.Models;
using PMA.Utils.Extensions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;

namespace PMA.Infrastructure.Extensions
{
    /// <summary>
    /// Defines method extensions for data table.
    /// </summary>
    internal static class DataTableExtension
    {
        /// <summary>
        /// Validates a data table structure.
        /// </summary>
        /// <param name="dataTable">This data table.</param>
        /// <param name="structure">An excel data table structure.</param>
        /// <returns>The validation result.</returns>
        public static List<RawLogMessage> Validate(this DataTable dataTable, ExcelTableStructure structure)
        {
            var result = new List<RawLogMessage>();

            if (dataTable.Columns.Count != structure.AllColumns.Length)
            {
                result.Add(new RawLogMessage
                {
                    Type = LogMessageType.DataTableIncorrectColumnNumber,
                    Parameters = new object[] { dataTable.Columns.Count, structure.AllColumns.Length }
                });
                return result;
            }

            for (int i = 0; i < structure.AllColumns.Length; i++)
            {
                string columnName = structure.AllColumns[i];

                if (dataTable.Columns[i].ColumnName != columnName)
                {
                    result.Add(new RawLogMessage
                    {
                        Type = LogMessageType.DataTableIncorrectColumnName,
                        Parameters = new object[] { dataTable.Columns[i].ColumnName, columnName }
                    });
                }
            }

            if (result.Any())
            {
                return result;
            }

            for (int i = 0; i < structure.UniqueColumns.Length; i++)
            {
                result.AddRange(dataTable.ValidateUniqueValues(structure.UniqueColumns[i]));
            }

            for (int i = 0; i < structure.NonEmptyColumns.Length; i++)
            {
                result.AddRange(dataTable.ValidateNonEmptyValues(structure.NonEmptyColumns[i]));
            }

            for (int i = 0; i < structure.IntegerColumns.Length; i++)
            {
                result.AddRange(dataTable.ValidateIntegerValues(structure.IntegerColumns[i]));
            }

            for (int i = 0; i < structure.DoubleColumns.Length; i++)
            {
                result.AddRange(dataTable.ValidateDoubleValues(structure.DoubleColumns[i]));
            }

            for (int i = 0; i < structure.BooleanColumns.Length; i++)
            {
                result.AddRange(dataTable.ValidateBooleanValues(structure.BooleanColumns[i]));
            }

            for (int i = 0; i < structure.RegExpColumns.Length; i++)
            {
                result.AddRange(dataTable.ValidateRegexValues(structure.RegExpColumns[i]));
            }

            return result;
        }

        /// <summary>
        /// Validates a data table column unique values.
        /// </summary>
        /// <param name="dataTable">This dataTable.</param>
        /// <param name="columnName">The unique column name.</param>
        /// <returns>The validation result.</returns>
        private static IEnumerable<RawLogMessage> ValidateUniqueValues(this DataTable dataTable, string columnName)
        {
            var result = new List<RawLogMessage>();

            var rows = dataTable.AsEnumerable()
                .GroupBy(r => r[columnName])
                .Where(g => g.Count() > 1)
                .Select(g => g.First())
                .Select(x => dataTable.Rows.IndexOf(x) + 1)
                .Distinct()
                .ToList();

            if (rows.Any())
            {
                result.Add(new RawLogMessage
                {
                    Type = LogMessageType.DataTableUniqueValueValidationFailed,
                    Parameters = new object[] { string.Join(",", rows), columnName }
                });
            }

            return result;
        }

        /// <summary>
        /// Validates a data table column non-empty values.
        /// </summary>
        /// <param name="dataTable">This dataTable.</param>
        /// <param name="columnName">The non-empty column name.</param>
        /// <returns>The validation result.</returns>
        private static IEnumerable<RawLogMessage> ValidateNonEmptyValues(this DataTable dataTable, string columnName)
        {
            var result = new List<RawLogMessage>();

            var rows = dataTable.AsEnumerable()
                .Where(r => string.IsNullOrEmpty((string)r[columnName]))
                .Select(x => dataTable.Rows.IndexOf(x) + 1)
                .ToList();

            if (rows.Any())
            {
                result.Add(new RawLogMessage
                {
                    Type = LogMessageType.DataTableNonEmptyValueValidationFailed,
                    Parameters = new object[] { string.Join(",", rows), columnName }
                });
            }

            return result;
        }

        /// <summary>
        /// Validates a data table column integer values.
        /// </summary>
        /// <param name="dataTable">This dataTable.</param>
        /// <param name="columnName">The integer column name.</param>
        /// <returns>The validation result.</returns>
        private static IEnumerable<RawLogMessage> ValidateIntegerValues(this DataTable dataTable, string columnName)
        {
            var result = new List<RawLogMessage>();

            var rows = dataTable.AsEnumerable()
                .Where(r => !int.TryParse((string)r[columnName], out _))
                .Select(x => dataTable.Rows.IndexOf(x) + 1)
                .ToList();

            if (rows.Any())
            {
                result.Add(new RawLogMessage
                {
                    Type = LogMessageType.DataTableIntegerValueValidationFailed,
                    Parameters = new object[] { string.Join(",", rows), columnName }
                });
            }

            return result;
        }

        /// <summary>
        /// Validates a data table column double values.
        /// </summary>
        /// <param name="dataTable">This dataTable.</param>
        /// <param name="columnName">The double column name.</param>
        /// <returns>The validation result.</returns>
        private static IEnumerable<RawLogMessage> ValidateDoubleValues(this DataTable dataTable, string columnName)
        {
            var result = new List<RawLogMessage>();

            var rows = dataTable.AsEnumerable()
                .Where(r => !double.TryParse((string)r[columnName], out _))
                .Select(x => dataTable.Rows.IndexOf(x) + 1)
                .ToList();

            if (rows.Any())
            {
                result.Add(new RawLogMessage
                {
                    Type = LogMessageType.DataTableDoubleValueValidationFailed,
                    Parameters = new object[] { string.Join(",", rows), columnName }
                });
            }

            return result;
        }

        /// <summary>
        /// Validates a data table column boolean values.
        /// </summary>
        /// <param name="dataTable">This dataTable.</param>
        /// <param name="columnName">The boolean column name.</param>
        /// <returns>The validation result.</returns>
        private static IEnumerable<RawLogMessage> ValidateBooleanValues(this DataTable dataTable, string columnName)
        {
            var result = new List<RawLogMessage>();

            var rows = dataTable.AsEnumerable()
                .Where(r => !bool.TryParse((string)r[columnName], out _))
                .Select(x => dataTable.Rows.IndexOf(x) + 1)
                .ToList();

            if (rows.Any())
            {
                result.Add(new RawLogMessage
                {
                    Type = LogMessageType.DataTableBooleanValueValidationFailed,
                    Parameters = new object[] { string.Join(",", rows), columnName }
                });
            }

            return result;
        }

        /// <summary>
        /// Validates a data table column regex values.
        /// </summary>
        /// <param name="dataTable">This dataTable.</param>
        /// <param name="columnName">The regex column name.</param>
        /// <returns>The validation result.</returns>
        public static List<RawLogMessage> ValidateRegexValues(this DataTable dataTable, string columnName)
        {
            var result = new List<RawLogMessage>();

            var indexes = new List<int>();

            foreach (DataRow row in dataTable.Rows)
            {
                try
                {
                    _ = new Regex((string)row[columnName]);
                }
                catch (ArgumentException)
                {
                    indexes.Add(dataTable.Rows.IndexOf(row) + 1);
                }
            }

            if (indexes.Any())
            {
                result.Add(new RawLogMessage
                {
                    Type = LogMessageType.DataTableRegExpValueValidationFailed,
                    Parameters = new object[] { string.Join(",", indexes), columnName }
                });
            }

            return result;
        }

        /// <summary>
        /// Validates a data table column enum values.
        /// </summary>
        /// <typeparam name="T">The enum.</typeparam>
        /// <param name="dataTable">This dataTable.</param>
        /// <param name="columnName">The enum column name.</param>
        /// <param name="excludeEnumValues">A collection of enum values to exclude from validation.</param>
        /// <returns>The validation result.</returns>
        public static List<RawLogMessage> ValidateEnumValues<T>(this DataTable dataTable, string columnName, IEnumerable excludeEnumValues = null)
        {
            var validValues = Enum.GetValues(typeof(T)).Cast<T>()
                .Select(x => x.ToString()).ToList();

            // ReSharper disable once InvertIf
            if (excludeEnumValues != null)
            {
                foreach (object excludeValue in excludeEnumValues)
                {
                    validValues.Remove(excludeValue.ToString());
                }
            }

            return dataTable.Rows.Cast<DataRow>()
                .Where(row => !validValues.Contains(((string)row[columnName]).FirstCharToUpper()))
                .Select(row => new RawLogMessage
                {
                    Type = LogMessageType.DataTableIncorrectRowValue,
                    Parameters = new object[] { (string)row[columnName], (string)row["Id"], columnName }
                }).ToList();
        }
    }
}
