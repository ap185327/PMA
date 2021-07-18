// <copyright file="ControlExtension.cs" company="Andrey Pospelov">
//     Copyright 2017-2021 Andrey Pospelov. All rights reserved.
// </copyright>

using System;
using System.Windows.Forms;

namespace PMA.WinForms.Extensions
{
    /// <summary>
    /// Extensions to facilitate work with controls in a multi threaded environment.
    /// </summary>
    internal static class ControlExtension
    {
        /// <summary>
        /// Calling the delegate through control. Invoke if needed.
        /// </summary>
        /// <param name="control">Control element.</param>
        /// <param name="action">Delegate with some action.</param>
        public static void InvokeIfNeeded(this Control control, Action action)
        {
            if (control != null)
            {
                if (control.InvokeRequired)
                {
                    control.Invoke(action);
                }
                else
                {
                    action();
                }
            }
        }

        /// <summary>
        /// Calling the delegate through control. Invoke if needed.
        /// </summary>
        /// <typeparam name="T">Delegate parameter type.</typeparam>
        /// <param name="control">Control element.</param>
        /// <param name="action">Delegate with some action.</param>
        /// <param name="arg">Delegate argument.</param>
        public static void InvokeIfNeeded<T>(this Control control, Action<T> action, T arg)
        {
            if (control.InvokeRequired)
            {
                control.Invoke(action, arg);
            }
            else
            {
                action(arg);
            }
        }
    }
}
