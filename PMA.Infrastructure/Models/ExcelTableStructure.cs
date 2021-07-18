// <copyright file="ExcelTableStructure.cs" company="Andrey Pospelov">
//     Copyright 2017-2021 Andrey Pospelov. All rights reserved.
// </copyright>

namespace PMA.Infrastructure.Models
{
    /// <summary>
    /// Class for the excel table structure model.
    /// </summary>
    internal class ExcelTableStructure
    {
        /// <summary>
        /// Gets or sets a collection of all column names.
        /// </summary>
        public string[] AllColumns { get; set; }

        /// <summary>
        /// Gets or sets a collection of unique column names.
        /// </summary>
        public string[] UniqueColumns { get; set; }

        /// <summary>
        /// Gets or sets a collection of non empty column names.
        /// </summary>
        public string[] NonEmptyColumns { get; set; }

        /// <summary>
        /// Gets or sets a collection of integer column names.
        /// </summary>
        public string[] IntegerColumns { get; set; }

        /// <summary>
        /// Gets or sets a collection of double column names.
        /// </summary>
        public string[] DoubleColumns { get; set; }

        /// <summary>
        /// Gets or sets a collection of boolean column names.
        /// </summary>
        public string[] BooleanColumns { get; set; }

        /// <summary>
        /// Gets or sets a collection of regular expression column names.
        /// </summary>
        public string[] RegExpColumns { get; set; }
    }
}
