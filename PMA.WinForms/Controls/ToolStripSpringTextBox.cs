// <copyright file="ToolStripSpringTextBox.cs" company="Andrey Pospelov">
//     Copyright 2017-2021 Andrey Pospelov. All rights reserved.
// </copyright>

using System;
using System.Drawing;
using System.Windows.Forms;

namespace PMA.WinForms.Controls
{
    /// <summary>
    ///  Represents a text box that allows the user to enter text with watermark and auto symbol replacement.
    /// </summary>
    internal class ToolStripSpringTextBox : ToolStripTextBox
    {
        /// <summary>
        /// The default font.
        /// </summary>
        private readonly Font _defaultFont;

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
        /// Initializes a new instance of the <see cref="ToolStripSpringTextBox">ToolStripSpringTextBox</see> class.
        /// </summary>
        /// <param name="watermark">The watermark.</param>
        /// <param name="autoSymbolReplace">The auto symbol replace option.</param>
        public ToolStripSpringTextBox(string watermark, bool autoSymbolReplace)
        {
            Watermark = watermark;
            AutoSymbolReplace = autoSymbolReplace;

            _defaultFont = new Font("Segoe UI", 9F);
            _watermarkFont = new Font("Segoe UI", 9F, FontStyle.Italic);

            TextChanged += ToolStripSpringTextBox_TextChanged;
            GotFocus += ToolStripSpringTextBox_GotFocus;
            LostFocus += ToolStripSpringTextBox_LostFocus;

            base.Text = Watermark;
            base.ForeColor = SystemColors.GrayText;
            base.Font = _watermarkFont;
        }

        /// <summary>
        /// Event handler for lost focus.
        /// </summary>
        /// <param name="sender">Object sender.</param>
        /// <param name="e">Event arguments.</param>
        private void ToolStripSpringTextBox_LostFocus(object sender, EventArgs e)
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
        private void ToolStripSpringTextBox_GotFocus(object sender, EventArgs e)
        {
            if (Text != GetCasingWatermark()) return;

            Text = string.Empty;
            ForeColor = SystemColors.WindowText;
            Font = _defaultFont;
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
                Font = _defaultFont;
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
        /// Retrieves the size of a rectangular area into which a control can be fitted.
        /// </summary>
        /// <param name="constrainingSize">The custom-sized area for a control.</param>
        /// <returns>An ordered pair of type System.Drawing.Size representing the width and height of a rectangle.</returns>
        public override Size GetPreferredSize(Size constrainingSize)
        {
            // Use the default size if the text box is on the overflow menu
            // or is on a vertical ToolStrip.
            if (IsOnOverflow || Owner.Orientation == Orientation.Vertical)
            {
                return DefaultSize;
            }

            // Declare a variable to store the total available width as 
            // it is calculated, starting with the display width of the 
            // owning ToolStrip.
            int width = Owner.DisplayRectangle.Width;

            // Subtract the width of the overflow button if it is displayed. 
            if (Owner.OverflowButton.Visible)
            {
                width = width - Owner.OverflowButton.Width - Owner.OverflowButton.Margin.Horizontal;
            }

            // Declare a variable to maintain a count of ToolStripSpringTextBox 
            // items currently displayed in the owning ToolStrip. 
            int springBoxCount = 0;

            foreach (ToolStripItem item in Owner.Items)
            {
                // Ignore items on the overflow menu.
                if (item.IsOnOverflow)
                {
                    continue;
                }

                if (item is ToolStripSpringTextBox)
                {
                    // For ToolStripSpringTextBox items, increment the count and 
                    // subtract the margin width from the total available width.
                    springBoxCount++;
                    width -= item.Margin.Horizontal;
                }
                else
                {
                    // For all other items, subtract the full width from the total
                    // available width.
                    width = width - item.Width - item.Margin.Horizontal;
                }
            }

            // If there are multiple ToolStripSpringTextBox items in the owning
            // ToolStrip, divide the total available width between them. 
            if (springBoxCount > 1) width /= springBoxCount;

            // If the available width is less than the default width, use the
            // default width, forcing one or more items onto the overflow menu.
            if (width < DefaultSize.Width) width = DefaultSize.Width;

            // Retrieve the preferred size from the base class, but change the
            // width to the calculated width. 
            var size = base.GetPreferredSize(constrainingSize);
            size.Width = width;
            return size;
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