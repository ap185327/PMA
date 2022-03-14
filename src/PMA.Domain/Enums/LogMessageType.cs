// <copyright file="LogMessageType.cs" company="Andrey Pospelov">
//     Copyright 2017-2021 Andrey Pospelov. All rights reserved.
// </copyright>

namespace PMA.Domain.Enums
{
    /// <summary>
    /// Enumerates different types of log messages.
    /// </summary>
    public enum LogMessageType
    {
        ImportStart,
        ImportEnd,
        ImportCompleted,
        ImportCanceling,
        ImportCanceled,

        DbTableDataRemoving,
        DbTableDataAdding,
        DbTableDataUpdating,
        DbTableError,

        DataFileOpening,
        DataFileClosing,
        DataFileReading,
        DataFileDoesNotExist,
        DataFileOpenError,
        DataFileCloseError,
        DataFileTableNotFoundByName,

        DataTableValidating,
        DataTableIncorrectColumnNumber,
        DataTableIncorrectColumnName,
        DataTableUniqueValueValidationFailed,
        DataTableNonEmptyValueValidationFailed,
        DataTableIntegerValueValidationFailed,
        DataTableDoubleValueValidationFailed,
        DataTableBooleanValueValidationFailed,
        DataTableRegExpValueValidationFailed,
        DataTableIncorrectRowValue,
        DataTableMorphCombinationValidationFailed,
        DataTableDuplicateFound,

        MorphEntryUpdated,
        MorphEntriesUpdated,
        MorphEntryInserted,
        MorphEntryNotFound,
        MorphEntryError
    }
}
