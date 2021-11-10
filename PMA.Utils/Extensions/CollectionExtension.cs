// <copyright file="CollectionExtension.cs" company="Andrey Pospelov">
//     Copyright 2017-2021 Andrey Pospelov. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Linq;

namespace PMA.Utils.Extensions
{
    public static class CollectionExtension
    {
        /// <summary>
        /// Gets the single array value by its type.
        /// </summary>
        /// <typeparam name="T">The value type.</typeparam>
        /// <param name="array">The array.</param>
        /// <returns>The single array value</returns>
        public static T GetSingleValue<T>(this IEnumerable<object> array)
        {
            return (T)array.Single(x => x.GetType() is T);
        }

        /// <summary>
        /// Fills the array with the same value.
        /// </summary>
        /// <typeparam name="T">The type of the value.</typeparam>
        /// <param name="array">This array.</param>
        /// <param name="value">The value.</param>
        public static void Fill<T>(this T[] array, T value)
        {
            for (int i = 0; i < array.Length; i++)
            {
                array[i] = value;
            }
        }

        /// <summary>
        /// Splits array into chunks.
        /// </summary>
        /// <typeparam name="T">The type of the value.</typeparam>
        /// <param name="array">This array.</param>
        /// <param name="chunkCount">The number of chunks.</param>
        /// <returns>Array chunks.</returns>
        public static IEnumerable<T[]> Split<T>(this T[] array, int chunkCount)
        {
            int len = array.Length;
            for (int i = 0; i < len; i += chunkCount)
            {
                yield return array.Skip(i).Take(Math.Min(chunkCount, len - i)).ToArray();
            }
        }

        /// <summary>
        /// Splits array into two chunks.
        /// </summary>
        /// <typeparam name="T">The type of the value.</typeparam>
        /// <param name="array">This array.</param>
        /// <returns>Array chunks.</returns>
        public static IEnumerable<T[]> SplitInTwo<T>(this T[] array)
        {
            int len = array.Length;
            int halfLen = len / 2;

            yield return array.Take(halfLen).ToArray();

            yield return array.Skip(halfLen).Take(len - halfLen).ToArray();
        }

        /// <summary>
        /// Gets an index of max value in the array.
        /// </summary>
        /// <typeparam name="T">The type of the value.</typeparam>
        /// <param name="array">This array.</param>
        /// <returns>An index of max value.</returns>
        public static int MaxIndex<T>(this T[] array) where T : IComparable<T>
        {
            int maxIndex = -1;
            var maxValue = default(T); // Immediately overwritten anyway

            int index = 0;
            for (int i = 0; i < array.Length; i++)
            {
                var value = array[i];

                if (value.CompareTo(maxValue) > 0 || maxIndex == -1)
                {
                    maxIndex = index;
                    maxValue = value;
                }
                index++;
            }

            return maxIndex;
        }

        public static void Replace<T>(this List<T> list, T oldItem, T newItem)
        {
            list.Remove(oldItem);
            list.Add(newItem);
        }
    }
}
