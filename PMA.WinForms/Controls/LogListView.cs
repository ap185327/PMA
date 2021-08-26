// <copyright file="LogListView.cs" company="Andrey Pospelov">
//     Copyright 2017-2021 Andrey Pospelov. All rights reserved.
// </copyright>

using PMA.Domain.Enums;
using PMA.WinForms.Extensions;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace PMA.WinForms.Controls
{
    /// <summary>
    /// Represents a Windows list view control, which displays a collection of items that can be displayed using one of four different views.
    /// </summary>
    internal class LogListView : ListView
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LogListView"/> class.
        /// </summary>
        public LogListView()
        {
            MouseDoubleClick += LogListView_MouseDoubleClick;
        }

        /// <summary>
        /// Adds a new log item.
        /// </summary>
        /// <param name="level">The log message level.</param>
        /// <param name="eventType">The event type.</param>
        /// <param name="eventDescription">The event description.</param>
        public void AddItem(MessageLevel level, string eventType, string eventDescription)
        {
            Color eventColor;

            switch (level)
            {
                case MessageLevel.Error:
                    eventColor = Color.Red;
                    break;
                case MessageLevel.Warning:
                    eventColor = Color.DarkBlue;
                    break;
                default:
                    eventColor = Color.Black;
                    break;
            }
            var listViewItem = new ListViewItem(new[] { DateTime.Now.ToString("hh:mm:ss.fff"), eventType, eventDescription }, -1, eventColor, Color.Empty, null);

            Parent.InvokeIfNeeded(() => Items.Insert(0, listViewItem));
        }

        /// <summary>
        /// Event handler for the LogListView mouse double click.
        /// </summary>
        /// <param name="sender">Object sender.</param>
        /// <param name="e">Event arguments.</param>
        private void LogListView_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            ShowDialog();
        }

        /// <summary>
        /// Shows a dialog.
        /// </summary>
        private void ShowDialog()
        {
            if (SelectedItems.Count == 0) return;

            var item = SelectedItems[0];
            MessageBox.Show(item.SubItems[2].Text, item.SubItems[1].Text, MessageBoxButtons.OK);
        }
    }
}
