// <copyright file="StopwatchExtension.cs" company="Andrey Pospelov">
//     Copyright 2017-2021 Andrey Pospelov. All rights reserved.
// </copyright>

using System;
using System.Diagnostics;

namespace PMA.Utils.Extensions
{
    public static class StopwatchExtension
    {
        /// <summary>
        /// Gets a time interval in convenient format (HH:mm:ss or mm:ss).
        /// </summary>
        /// <param name="timer">This timer.</param>
        /// <returns>A time.</returns>
        public static string GetTime(this Stopwatch timer)
        {
            var time = TimeSpan.FromMilliseconds(timer.ElapsedMilliseconds);

            return string.Format(time.Hours > 0 ? $"{time.Hours:D2}:{time.Minutes:D2}:{time.Seconds:D2}" : $"{time.Minutes:D2}:{time.Seconds:D2}");
        }
    }
}
