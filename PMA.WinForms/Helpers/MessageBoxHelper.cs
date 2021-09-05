// <copyright file="MessageBoxHelper.cs" company="Andrey Pospelov">
//     Copyright 2017-2021 Andrey Pospelov. All rights reserved.
// </copyright>

using PMA.Domain.Enums;
using PMA.Utils.Exceptions;
using System.Windows.Forms;

namespace PMA.WinForms.Helpers
{
    /// <summary>
    /// The message box helper class.
    /// </summary>
    internal static class MessageBoxHelper
    {
        /// <summary>
        /// Gets a message box icon flag.
        /// </summary>
        /// <param name="type">The modal dialog type.</param>
        /// <returns>A message box icon flag.</returns>
        public static MessageBoxIcon GetIcon(ModalDialogType type)
        {
            return type switch
            {
                ModalDialogType.Information => MessageBoxIcon.Information,
                ModalDialogType.Warning => MessageBoxIcon.Warning,
                ModalDialogType.Error => MessageBoxIcon.Error,
                ModalDialogType.Question => MessageBoxIcon.Question,
                ModalDialogType.Exclamation => MessageBoxIcon.Exclamation,
                _ => MessageBoxIcon.None
            };
        }

        /// <summary>
        /// Gets message box buttons flag.
        /// </summary>
        /// <param name="buttons">Button texts.</param>
        /// <returns>Message box buttons flag.</returns>
        public static MessageBoxButtons GetButtons(ModalButtonType[] buttons)
        {
            var firstButtonText = buttons[0];
            var secondButtonText = buttons.Length > 1 ? buttons[1] : ModalButtonType.None;
            var thirdButtonText = buttons.Length > 2 ? buttons[2] : ModalButtonType.None;

            return firstButtonText switch
            {
                ModalButtonType.Ok when secondButtonText == ModalButtonType.None && thirdButtonText == ModalButtonType.None => MessageBoxButtons.OK,
                ModalButtonType.Ok when secondButtonText == ModalButtonType.Cancel && thirdButtonText == ModalButtonType.None => MessageBoxButtons.OKCancel,
                ModalButtonType.Abort when secondButtonText == ModalButtonType.Retry && thirdButtonText == ModalButtonType.Ignore => MessageBoxButtons.AbortRetryIgnore,
                ModalButtonType.Yes when secondButtonText == ModalButtonType.No && thirdButtonText == ModalButtonType.Cancel => MessageBoxButtons.YesNoCancel,
                ModalButtonType.Yes when secondButtonText == ModalButtonType.No && thirdButtonText == ModalButtonType.None => MessageBoxButtons.YesNo,
                ModalButtonType.Retry when secondButtonText == ModalButtonType.Cancel && thirdButtonText == ModalButtonType.None => MessageBoxButtons.RetryCancel,
                _ => throw new CustomException("This combination of arguments is not supported")
            };
        }

        /// <summary>
        /// Gets a button type.
        /// </summary>
        /// <param name="dialogResult">The dialog result.</param>
        /// <returns>A button type.</returns>
        public static ModalButtonType GetButtonType(DialogResult dialogResult)
        {
            return dialogResult switch
            {
                DialogResult.Abort => ModalButtonType.Abort,
                DialogResult.Yes => ModalButtonType.Yes,
                DialogResult.No => ModalButtonType.No,
                DialogResult.Ignore => ModalButtonType.Ignore,
                DialogResult.Cancel => ModalButtonType.Cancel,
                DialogResult.Retry => ModalButtonType.Retry,
                _ => ModalButtonType.Ok
            };
        }
    }
}
