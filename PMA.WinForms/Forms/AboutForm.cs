// <copyright file="AboutForm.cs" company="Andrey Pospelov">
//     Copyright 2017-2021 Andrey Pospelov. All rights reserved.
// </copyright>

using System;
using System.Windows.Forms;

namespace PMA.WinForms.Forms
{
    public partial class AboutForm : Form
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AboutForm"/> class.
        /// </summary>
        public AboutForm()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Event handler for the AboutForm click.
        /// </summary>
        /// <param name="sender">Object sender.</param>
        /// <param name="e">Event arguments.</param>
        private void AboutForm_Click(object sender, EventArgs e)
        {
            Close();
        }

        /// <summary>
        /// Event handler for the AboutForm mouse click.
        /// </summary>
        /// <param name="sender">Object sender.</param>
        /// <param name="e">Event arguments.</param>
        private void AboutForm_MouseClick(object sender, MouseEventArgs e)
        {
            Close();
        }

        /// <summary>
        /// Event handler for the AboutForm double click.
        /// </summary>
        /// <param name="sender">Object sender.</param>
        /// <param name="e">Event arguments.</param>
        private void AboutForm_DoubleClick(object sender, EventArgs e)
        {
            Close();
        }

        /// <summary>
        /// Event handler for the AboutForm mouse double click.
        /// </summary>
        /// <param name="sender">Object sender.</param>
        /// <param name="e">Event arguments.</param>
        private void AboutForm_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            Close();
        }
    }
}
