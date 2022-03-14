// <copyright file="ToolStripNumberControl.cs" company="Andrey Pospelov">
//     Copyright 2017-2021 Andrey Pospelov. All rights reserved.
// </copyright>

using System;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace PMA.WinForms.Controls
{
    /// <summary>
    /// Represents a number control that allows the user to enter numbers within a specified range.
    /// </summary>
    [ToolStripItemDesignerAvailability(ToolStripItemDesignerAvailability.ToolStrip)]
    internal class ToolStripNumberControl : ToolStripControlHost
    {
        /// <summary>
        /// Event handler when the Control value changed.
        /// </summary>
        public event EventHandler ValueChanged;

        /// <summary>
        /// Initializes a new instance of the ToolStripNumberControl class.
        /// </summary>
        public ToolStripNumberControl() : base(new NumericUpDown()) { }

        /// <summary>
        ///  Subscribes events from the hosted control.
        /// </summary>
        /// <param name="control">The control from which to subscribe events.</param>
        protected override void OnSubscribeControlEvents(Control control)
        {
            base.OnSubscribeControlEvents(control);
            ((NumericUpDown)control).ValueChanged += OnValueChanged;
        }

        /// <summary>
        /// Unsubscribes events from the hosted control.
        /// </summary>
        /// <param name="control">The control from which to unsubscribe events.</param>
        protected override void OnUnsubscribeControlEvents(Control control)
        {
            base.OnUnsubscribeControlEvents(control);
            ((NumericUpDown)control).ValueChanged -= OnValueChanged;
        }

        /// <summary>
        /// Gets Control that this System.Windows.Forms.ToolStripControlHost is hosting.
        /// </summary>
        public new NumericUpDown Control => base.Control as NumericUpDown;

        /// <summary>
        /// Event handler for value changed.
        /// </summary>
        /// <param name="sender">Object sender.</param>
        /// <param name="e">Event arguments.</param>
        public void OnValueChanged(object sender, EventArgs e)
        {
            ValueChanged?.Invoke(this, e);
        }
    }
}
