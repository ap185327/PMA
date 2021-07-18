// <copyright file="MessageBoxHelper.cs" company="Andrey Pospelov">
//     Copyright 2017-2021 Andrey Pospelov. All rights reserved.
// </copyright>

using PMA.Domain.Enums;
using PMA.Domain.Exceptions;
using System.Windows.Forms;

namespace PMA.WinForms.Helpers
{
    /// <summary>
    /// The message box helper.
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
            switch (type)
            {
                case ModalDialogType.Information:
                    {
                        return MessageBoxIcon.Information;
                    }
                case ModalDialogType.Warning:
                    {
                        return MessageBoxIcon.Warning;
                    }
                case ModalDialogType.Error:
                    {
                        return MessageBoxIcon.Error;
                    }
                case ModalDialogType.Question:
                    {
                        return MessageBoxIcon.Question;
                    }
                case ModalDialogType.Exclamation:
                    {
                        return MessageBoxIcon.Exclamation;
                    }
                default:
                    {
                        return MessageBoxIcon.None;
                    }
            }
        }

        /// <summary>
        /// Gets message box buttons flag.
        /// </summary>
        /// <param name="buttons">Button texts.</param>
        /// <returns>Message box buttons flag.</returns>
        public static MessageBoxButtons GetButtons(string[] buttons)
        {
            string firstButtonText = buttons[0]?.ToLower();
            string secondButtonText = buttons.Length > 1 ? buttons[1]?.ToLower() : null;
            string thirdButtonText = buttons.Length > 2 ? buttons[2]?.ToLower() : null;

            if (firstButtonText == "ok" && string.IsNullOrEmpty(secondButtonText) && string.IsNullOrEmpty(thirdButtonText))
            {
                return MessageBoxButtons.OK;
            }

            if (firstButtonText == "ok" && secondButtonText == "cancel" && string.IsNullOrEmpty(thirdButtonText))
            {
                return MessageBoxButtons.OKCancel;
            }

            if (firstButtonText == "abort" && secondButtonText == "retry" && thirdButtonText == "ignore")
            {
                return MessageBoxButtons.AbortRetryIgnore;
            }

            if (firstButtonText == "yes" && secondButtonText == "no" && thirdButtonText == "cancel")
            {
                return MessageBoxButtons.YesNoCancel;
            }

            if (firstButtonText == "yes" && secondButtonText == "no" && string.IsNullOrEmpty(thirdButtonText))
            {
                return MessageBoxButtons.YesNo;
            }

            if (firstButtonText == "retry" && secondButtonText == "cancel" && string.IsNullOrEmpty(thirdButtonText))
            {
                return MessageBoxButtons.RetryCancel;
            }

            throw new CustomException("This combination of arguments is not supported");
        }

        /// <summary>
        /// Gets a button index.
        /// </summary>
        /// <param name="dialogResult">The dialog result.</param>
        /// <param name="buttons">The buttons.</param>
        /// <returns>A button index.</returns>
        public static int GetButtonIndex(DialogResult dialogResult, MessageBoxButtons buttons)
        {
            switch (dialogResult)
            {
                case DialogResult.OK:
                case DialogResult.Abort:
                case DialogResult.Yes:
                    {
                        return 0;
                    }
                case DialogResult.No:
                    {
                        return 1;
                    }
                case DialogResult.Ignore:
                    {
                        return 2;
                    }
                case DialogResult.Cancel:
                    {
                        return buttons == MessageBoxButtons.YesNoCancel ? 2 : 1;
                    }
                case DialogResult.Retry:
                    {
                        return buttons == MessageBoxButtons.AbortRetryIgnore ? 1 : 0;
                    }
                default:
                    {
                        return 0;
                    }
            }
        }
    }
}
