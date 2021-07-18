﻿// <copyright file="WatermarkTextBox.cs" company="Andrey Pospelov">
//     Copyright 2017-2021 Andrey Pospelov. All rights reserved.
// </copyright>

using System;
using System.Drawing;
using System.Windows.Forms;

namespace PMA.WinForms.Controls
{
    /// <summary>
    /// Represents a Windows text box control with watermark and auto symbol replacement.
    /// </summary>
    internal class WatermarkTextBox : TextBox
    {
        /// <summary>
        /// The watermark font.
        /// </summary>
        private readonly Font _watermarkFont;

        /// <summary>
        /// The watermark.
        /// </summary>
        public string Watermark { get; set; }

        /// <summary>
        /// Gets or sets whether the watermark is shown or not.
        /// </summary>
        public bool IsWatermarkShown => Text == GetCasingWatermark();

        /// <summary>
        /// Gets or sets the text associated with this control.
        /// </summary>
        public override string Text
        {
            get => base.Text;
            set
            {
                if (!Focused && string.IsNullOrEmpty(value))
                {
                    base.Text = Watermark;
                    ForeColor = SystemColors.GrayText;
                    Font = _watermarkFont;
                }
                else
                {
                    base.Text = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets auto symbol replace option: aa -> ā, ii -> ī, etc.
        /// </summary>
        public bool AutoSymbolReplace { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="WatermarkTextBox">WatermarkTextBox</see> class.
        /// </summary>
        /// <param name="watermark">The watermark.</param>
        /// <param name="autoSymbolReplace">The auto symbol replace option.</param>
        public WatermarkTextBox(string watermark, bool autoSymbolReplace)
        {
            Watermark = watermark;
            AutoSymbolReplace = autoSymbolReplace;

            _watermarkFont = new Font("Segoe UI", 9F, FontStyle.Italic);

            TextChanged += ToolStripSpringTextBox_TextChanged;
            GotFocus += WatermarkTextBox_GotFocus;
            LostFocus += WatermarkTextBox_LostFocus;

            base.Text = Watermark;
            base.ForeColor = SystemColors.GrayText;
            base.Font = _watermarkFont;
        }

        /// <summary>
        /// Event handler for lost focus.
        /// </summary>
        /// <param name="sender">Object sender.</param>
        /// <param name="e">Event arguments.</param>
        private void WatermarkTextBox_LostFocus(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(Text)) return;

            Text = Watermark;
            ForeColor = SystemColors.GrayText;
            Font = _watermarkFont;
        }

        /// <summary>
        /// Event handler for got focus.
        /// </summary>
        /// <param name="sender">Object sender.</param>
        /// <param name="e">Event arguments.</param>
        private void WatermarkTextBox_GotFocus(object sender, EventArgs e)
        {
            if (Text != GetCasingWatermark()) return;

            Text = string.Empty;
            ForeColor = SystemColors.WindowText;
            Font = DefaultFont;
        }

        /// <summary>
        /// Event handler for text changed.
        /// </summary>
        /// <param name="sender">Object sender.</param>
        /// <param name="e">Event arguments.</param>
        private void ToolStripSpringTextBox_TextChanged(object sender, EventArgs e)
        {
            if (Text != GetCasingWatermark())
            {
                ForeColor = SystemColors.WindowText;
                Font = DefaultFont;
            }
            else
            {
                ForeColor = SystemColors.GrayText;
                Font = _watermarkFont;
            }

            if (!AutoSymbolReplace) return;

            string text = Text
                .Replace("aa", "ā")
                .Replace("ii", "ī")
                .Replace("uu", "ū")
                .Replace("~n", "ñ")
                .Replace(".n", "ṇ")
                .Replace(".d", "ḍ")
                .Replace(".l", "ḷ")
                .Replace(".m", "ṃ")
                .Replace(".t", "ṭ")
                .Replace("\"n", "ṅ");

            if (text == Text) return;

            int start = SelectionStart;
            int length = SelectionLength;
            Text = text;
            Select(start - 1, length);
        }

        /// <summary>
        /// Gets a casing watermark value.
        /// </summary>
        /// <returns>A casing watermark value.</returns>
        private string GetCasingWatermark()
        {
            return CharacterCasing switch
            {
                CharacterCasing.Lower => Watermark.ToLower(),
                CharacterCasing.Upper => Watermark.ToUpper(),
                _ => Watermark
            };
        }
    }
}
