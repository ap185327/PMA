// <copyright file="ModalDialogEventArgs.cs" company="Andrey Pospelov">
//     Copyright 2017-2021 Andrey Pospelov. All rights reserved.
// </copyright>

namespace PMA.Domain.Enums
{
    /// <summary>
    /// Enumerates different types of the modal dialog.
    /// </summary>
    public enum ModalDialogType
    {
        /// <summary>
        /// Modal dialog doesn't have a specific type.
        /// </summary>
        None = 0,

        /// <summary>
        /// The information type.
        /// </summary>
        Information,

        /// <summary>
        /// The warning type.
        /// </summary>
        Warning,

        /// <summary>
        /// The error type.
        /// </summary>
        Error,

        /// <summary>
        /// The question type.
        /// </summary>
        Question,

        /// <summary>
        /// The exclamation type. 
        /// </summary>
        Exclamation
    }
}
