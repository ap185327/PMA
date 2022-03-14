// <copyright file="DataTableExtension.cs" company="Andrey Pospelov">
//     Copyright 2017-2021 Andrey Pospelov. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace PMA.Utils.Extensions
{
    public static class DataTableExtension
    {
        public static void ChangeColumnDataType(this DataTable dataTable, Type newType, IEnumerable<string> columnNames)
        {
            if (dataTable is null) return;

            using var tempDataTable = dataTable.Copy();
            dataTable.Rows.Clear();

            foreach (string columnName in columnNames)
            {
                // ReSharper disable once PossibleNullReferenceException
                dataTable.Columns[columnName].DataType = newType;
            }

            foreach (DataRow row in tempDataTable.Rows)
            {
                dataTable.ImportRow(row);
            }
        }

        public static void ChangeColumnDataType(this DataTable dataTable, Type newType, string columnName)
        {
            if (dataTable is null) return;

            using var tempDataTable = dataTable.Copy();
            dataTable.Rows.Clear();

            // ReSharper disable once PossibleNullReferenceException
            dataTable.Columns[columnName].DataType = newType;

            foreach (DataRow row in tempDataTable.Rows)
            {
                dataTable.ImportRow(row);
            }
        }

        public static void SplitRows(this DataTable dataTable, IEnumerable<string> columnNames)
        {
            if (dataTable is null) return;

            foreach (DataColumn column in dataTable.Columns)
            {
                column.Unique = false;
            }

            foreach (string columnName in columnNames)
            {
                for (int i = 0; i < dataTable.Rows.Count; i++)
                {
                    string value = dataTable.Rows[i][columnName].ToString();

                    // ReSharper disable once PossibleNullReferenceException
                    if (!value.Contains("|")) continue;

                    string[] list = value.Split('|');

                    dataTable.Rows[i][columnName] = list[0];

                    for (int j = 1; j < list.Length; j++)
                    {
                        var row = dataTable.NewRow();

                        foreach (DataColumn column in dataTable.Columns)
                        {
                            if (column.ColumnName == columnName)
                            {
                                row[column.ColumnName] = list[j];
                            }
                            else if (dataTable.Rows[i][column.ColumnName] != DBNull.Value)
                            {
                                row[column.ColumnName] = dataTable.Rows[i][column.ColumnName];
                            }
                        }

                        dataTable.Rows.Add(row);
                    }
                }
            }
        }

        public static void SplitRow(this DataTable dataTable, string columnName)
        {
            dataTable.SplitRows(new[] { columnName });
        }

        public static void MergeColumns(this DataTable dataTable, string[] columnNames, string newColumnName)
        {
            if (dataTable is null) return;

            foreach (DataRow row in dataTable.Rows)
            {
                var stringBuilder = new StringBuilder();
                foreach (string columnName in columnNames)
                {
                    stringBuilder.Append(row[columnName]);
                }
                row[columnNames[0]] = stringBuilder.ToString();
            }

            for (int i = 1; i < columnNames.Length; i++)
            {
                dataTable.Columns.Remove(columnNames[i]);
            }

            // ReSharper disable once PossibleNullReferenceException
            dataTable.Columns[columnNames[0]].ColumnName = newColumnName;
        }

        public static IList<string> GetDuplicates(this DataTable dataTable, IList<string> keyColumnNames, string idColumnName)
        {
            if (dataTable is null) return null;

            string newColumnName = Guid.NewGuid().ToString();

            dataTable.Columns.Add(newColumnName);

            for (int i = 0; i < dataTable.Rows.Count; i++)
            {
                var stringBuilder = new StringBuilder();
                foreach (string columnName in keyColumnNames)
                {
                    stringBuilder.Append((string)dataTable.Rows[i][columnName] + ";");
                }
                dataTable.Rows[i][newColumnName] = stringBuilder.ToString();
            }

            var duplicates = dataTable
                .AsEnumerable()
                .GroupBy(r => r[newColumnName])
                .Where(g => g.Count() > 1)
                .Select(g => string.Join(",", g.Select(x => (string)x[idColumnName]).Distinct().ToList()))
                .Distinct()
                .ToList();

            dataTable.Columns.Remove(newColumnName);

            return duplicates;
        }
    }
}
