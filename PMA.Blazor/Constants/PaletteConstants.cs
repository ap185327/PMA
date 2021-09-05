// <copyright file="PaletteConstants.cs" company="Andrey Pospelov">
//     Copyright 2017-2021 Andrey Pospelov. All rights reserved.
// </copyright>

using MudBlazor;
using MudBlazor.Utilities;
using PMA.Utils.Extensions;
using Color = System.Drawing.Color;

namespace PMA.Blazor.Constants
{
    public static class PaletteConstants
    {
        public static readonly Palette Default = new()
        {
            Primary = "#b73801",
            Error = Colors.Red.Default,
            Warning = Colors.Orange.Default,

            // Buttons
            ActionDefault = Color.Black.ToRgba(0.75),
            ActionDisabled = Color.Black.ToRgba(0.25),

            // Lines
            Divider = Color.Black.ToRgba(0.1),
            TableLines = Color.Black.ToRgba(0.25),
            LinesInputs = Color.Black.ToRgba(0.5),
            LinesDefault = Color.Black.ToRgba(0.12),

            // Hovers
            TableHover = Color.Black.ToRgba(0.04),

            // Texts
            AppbarText = Color.Black.ToRgba(0.75),
            TextPrimary = Color.Black.ToRgba(0.75),
            TextSecondary = Color.Black.ToRgba(0.5),
            TextDisabled = Color.Black.ToRgba(0.25),
            DrawerText = Color.Black.ToRgba(0.5),

            // Backgrounds
            Surface = Color.White.ToHex(),
            Background = Color.FromArgb(245, 243, 237).ToHex(),
            AppbarBackground = Color.White.ToHex(),
            DrawerBackground = Color.White.ToHex(),

            //Tooltip
            DarkContrastText = Color.White.ToRgba(0.85)
        };

        public static readonly Palette Dark = new()
        {
            Primary = "#5898e1",
            Error = new MudColor(Colors.Red.Default).SetAlpha(0.75).ToString(MudColorOutputFormats.RGBA),
            Warning = new MudColor(Colors.Orange.Default).SetAlpha(0.75).ToString(MudColorOutputFormats.RGBA),

            // Buttons
            ActionDefault = Color.White.ToRgba(0.75),
            ActionDisabled = Color.White.ToRgba(0.25),

            // Lines
            Divider = Color.White.ToRgba(0.1),
            TableLines = Color.White.ToRgba(0.25),
            LinesInputs = Color.White.ToRgba(0.5),
            LinesDefault = Color.White.ToRgba(0.12),

            // Hovers
            TableHover = Color.White.ToRgba(0.04),

            // Texts
            AppbarText = Color.White.ToRgba(0.75),
            TextPrimary = Color.White.ToRgba(0.75),
            TextSecondary = Color.White.ToRgba(0.5),
            TextDisabled = Color.White.ToRgba(0.25),
            DrawerText = Color.White.ToRgba(0.5),


            // Backgrounds
            Surface = "#172a46",
            Background = "#0a192f",
            AppbarBackground = "#172a46",
            DrawerBackground = "#172a46",

            // Tooltip
            DarkContrastText = Color.White.ToRgba(0.85),
            GrayDarker = "#0a192f"
        };
    }
}
