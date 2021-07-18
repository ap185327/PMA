// <copyright file="FormExtension.cs" company="Andrey Pospelov">
//     Copyright 2017-2021 Andrey Pospelov. All rights reserved.
// </copyright>

using System.Windows.Forms;

namespace PMA.WinForms.Extensions
{
    internal static class FormExtension
    {
        private const int Limit = 10;
        public static void Sticking(this Form form)
        {
            int x1 = form.Location.X;
            int y1 = form.Location.Y;
            int x2 = form.Parent.Width - form.Width - x1 - 4;
            int y2 = form.Parent.Height - form.Height - y1 - 4;

            if (x1 < Limit)
            {
                form.Left = 0;
            }
            else if (x2 < Limit && x2 > -Limit)
            {
                form.Left = x1 + x2;
            }

            if (y1 < Limit)
            {
                form.Top = 0;
            }
            else if (y2 < Limit && y2 > -Limit)
            {
                form.Top = y1 + y2;
            }
        }
    }
}
