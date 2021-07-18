// <copyright file="OpenFileDialogComboBox.cs" company="Andrey Pospelov">
//     Copyright 2017-2021 Andrey Pospelov. All rights reserved.
// </copyright>

using System.Windows.Forms;

namespace PMA.WinForms.Controls
{
    /// <summary>
    /// Represents a Windows combo box control with open file dialog feature.
    /// </summary>
    internal class OpenFileDialogComboBox : ComboBox
    {
        /// <summary>
        /// Opens file dialog.
        /// </summary>
        /// <param name="title">The file dialog box title.</param>
        /// <param name="filter">The current file name filter string, which determines the choices that appear in the "Save as file type" or "Files of type" box in the dialog box.</param>
        public void OpenFile(string title, string filter)
        {
            using var openFileDialog = new OpenFileDialog
            {
                Title = title,
                Filter = filter
            };

            if (openFileDialog.ShowDialog() != DialogResult.OK) return;

            int index = Items.IndexOf(openFileDialog.FileName);

            if (index >= 0)
            {
                Items.RemoveAt(index);
            }

            Items.Insert(0, openFileDialog.FileName);

            Text = openFileDialog.FileName;
        }
    }
}
