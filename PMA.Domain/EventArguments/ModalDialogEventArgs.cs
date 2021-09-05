// <copyright file="ModalDialogEventArgs.cs" company="Andrey Pospelov">
//     Copyright 2017-2021 Andrey Pospelov. All rights reserved.
// </copyright>

using PMA.Domain.Enums;
using System;

namespace PMA.Domain.EventArguments
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ModalDialogEventArgs"/> class.
    /// </summary>
    public class ModalDialogEventArgs : EventArgs
    {
        /// <summary>
        /// Gets or initialize a modal dialog title.
        /// </summary>
        public string Title { get; init; }

        /// <summary>
        /// Gets or initialize a modal dialog message.
        /// </summary>
        public string Message { get; init; }

        /// <summary>
        /// Gets or initialize a modal dialog type.
        /// </summary>
        public ModalDialogType Type { get; init; }

        /// <summary>
        /// Gets or initialize a modal dialog buttons.
        /// </summary>
        public ModalButtonType[] Buttons { get; init; }
    }
}
