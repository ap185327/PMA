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
            Primary = new MudColor("#b73801"),
            Error = Colors.Red.Default,
            Warning = Colors.Orange.Default,

            // Buttons
            ActionDefault = new MudColor(Color.Black.ToHex()).SetAlpha(0.75),
            ActionDisabled = new MudColor(Color.Black.ToHex()).SetAlpha(0.25),

            // Lines
            Divider = new MudColor(Color.Black.ToHex()).SetAlpha(0.1),
            TableLines = new MudColor(Color.Black.ToHex()).SetAlpha(0.25),
            LinesInputs = new MudColor(Color.Black.ToHex()).SetAlpha(0.5),
            LinesDefault = new MudColor(Color.Black.ToHex()).SetAlpha(0.12),

            // Hovers
            TableHover = new MudColor(Color.Black.ToHex()).SetAlpha(0.04),

            // Texts
            AppbarText = new MudColor(Color.Black.ToHex()).SetAlpha(0.75),
            TextPrimary = new MudColor(Color.Black.ToHex()).SetAlpha(0.75),
            TextSecondary = new MudColor(Color.Black.ToHex()).SetAlpha(0.5),
            TextDisabled = new MudColor(Color.Black.ToHex()).SetAlpha(0.25),
            DrawerText = new MudColor(Color.Black.ToHex()).SetAlpha(0.5),

            // Backgrounds
            Surface = new MudColor(Color.White.ToHex()),
            Background = new MudColor(Color.FromArgb(245, 243, 237).ToHex()),
            AppbarBackground = new MudColor(Color.White.ToHex()),
            DrawerBackground = new MudColor(Color.White.ToHex()),

            //Tooltip
            DarkContrastText = new MudColor(Color.White.ToHex()).SetAlpha(0.85)
        };

        public static readonly Palette Dark = new()
        {
            Primary = new MudColor("#5898e1"),
            Error = new MudColor(Colors.Red.Default).SetAlpha(0.75),
            Warning = new MudColor(Colors.Orange.Default).SetAlpha(0.75),

            // Buttons
            ActionDefault = new MudColor(Color.White.ToHex()).SetAlpha(0.75),
            ActionDisabled = new MudColor(Color.White.ToHex()).SetAlpha(0.25),

            // Lines
            Divider = new MudColor(Color.White.ToHex()).SetAlpha(0.1),
            TableLines = new MudColor(Color.White.ToHex()).SetAlpha(0.25),
            LinesInputs = new MudColor(Color.White.ToHex()).SetAlpha(0.5),
            LinesDefault = new MudColor(Color.White.ToHex()).SetAlpha(0.12),

            // Hovers
            TableHover = new MudColor(Color.White.ToHex()).SetAlpha(0.04),

            // Texts
            AppbarText = new MudColor(Color.White.ToHex()).SetAlpha(0.75),
            TextPrimary = new MudColor(Color.White.ToHex()).SetAlpha(0.75),
            TextSecondary = new MudColor(Color.White.ToHex()).SetAlpha(0.5),
            TextDisabled = new MudColor(Color.White.ToHex()).SetAlpha(0.25),
            DrawerText = new MudColor(Color.White.ToHex()).SetAlpha(0.5),


            // Backgrounds
            Surface = new MudColor("#172a46"),
            Background = new MudColor("#0a192f"),
            AppbarBackground = new MudColor("#172a46"),
            DrawerBackground = new MudColor("#172a46"),

            // Tooltip
            DarkContrastText = new MudColor(Color.White.ToHex()).SetAlpha(0.85),
            GrayDarker = "#0a192f"
        };
    }
}
