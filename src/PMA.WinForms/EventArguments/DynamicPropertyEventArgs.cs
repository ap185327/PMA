// <copyright file="DynamicPropertyEventArgs.cs" company="Andrey Pospelov">
//     Copyright 2017-2021 Andrey Pospelov. All rights reserved.
// </copyright>

using System;

namespace PMA.WinForms.EventArguments
{
    /// <summary>
    /// Initializes a new instance of the DynamicPropertyEventArgs class.
    /// </summary>
    internal class DynamicPropertyEventArgs : EventArgs
    {
        /// <summary>
        /// Gets or sets the property index.
        /// </summary>
        public int PropertyIndex { get; set; }

        /// <summary>
        /// Gets or sets the new property value index.
        /// </summary>
        public int NewValueIndex { get; set; }
    }
}
