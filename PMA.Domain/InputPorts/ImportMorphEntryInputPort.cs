// <copyright file="ImportMorphEntryInputPort.cs" company="Andrey Pospelov">
//     Copyright 2017-2021 Andrey Pospelov. All rights reserved.
// </copyright>

namespace PMA.Domain.InputPorts
{
    /// <summary>
    /// The import morphological entry input port class.
    /// </summary>
    public class ImportMorphEntryInputPort
    {
        /// <summary>
        /// Gets or sets a file path with morphological entries.
        /// </summary>
        public string DataFilePath { get; init; }

        /// <summary>
        /// Gets or sets whether the analyze before import is checked or not.
        /// </summary>
        public bool IsAnalyzeBeforeImportChecked { get; init; }
    }
}
