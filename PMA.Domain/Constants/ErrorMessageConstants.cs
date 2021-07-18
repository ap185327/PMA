// <copyright file="ErrorMessageConstants.cs" company="Andrey Pospelov">
//     Copyright 2017-2021 Andrey Pospelov. All rights reserved.
// </copyright>

namespace PMA.Domain.Constants
{
    /// <summary>
    /// The error message constant class.
    /// </summary>
    public class ErrorMessageConstants
    {
        // Common

        // Loaders
        public const string DataTableStructureIsNotValid = "The data table structure is invalid";
        public const string DataTableContainsDuplicates = "The data table contains duplicates";
        public const string DataTableValuesAreNotValid = "The data table values are invalid";

        // UseCases
        public const string ValueIsNull = "{0} is null";
        public const string CancellationIsAlreadyRequested = "Cancellation is already requested";
        public const string FilePathIsEmpty = "File path is empty";
        public const string FileNotFound = "File '{0}' not found";
        public const string MorphParameterIndexOutOfRange = "Morphological parameter index: {0}, out of range";

        //Managers
        public const string LayerIsNotSet = "A chronological layer isn't set";
    }
}
