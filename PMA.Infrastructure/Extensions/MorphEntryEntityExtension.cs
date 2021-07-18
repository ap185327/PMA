// <copyright file="MorphEntryEntityExtension.cs" company="Andrey Pospelov">
//     Copyright 2017-2021 Andrey Pospelov. All rights reserved.
// </copyright>

using PMA.Domain.Models;
using PMA.Infrastructure.Entities;

namespace PMA.Infrastructure.Extensions
{
    /// <summary>
    /// Defines method extensions for morphological entry entity.
    /// </summary>
    internal static class MorphEntryEntityExtension
    {
        /// <summary>
        /// Overrides morphological entry entity properties
        /// </summary>
        /// <param name="entity">This morphological entry entity</param>
        /// <param name="entry">The morphological entry source.</param>
        public static void OverrideBy(this MorphEntryEntity entity, MorphEntry entry)
        {
            entity.Id = entry.Id;
            entity.Parameter0 = entry.Parameters[0];
            entity.Parameter1 = entry.Parameters[1];
            entity.Parameter2 = entry.Parameters[2];
            entity.Parameter3 = entry.Parameters[3];
            entity.Parameter4 = entry.Parameters[4];
            entity.Parameter5 = entry.Parameters[5];
            entity.Parameter6 = entry.Parameters[6];
            entity.Parameter7 = entry.Parameters[7];
            entity.Parameter8 = entry.Parameters[8];
            entity.Parameter9 = entry.Parameters[9];
            entity.Parameter10 = entry.Parameters[10];
            entity.Parameter11 = entry.Parameters[11];
            entity.Parameter12 = entry.Parameters[12];
            entity.Parameter13 = entry.Parameters[13];
            entity.Parameter14 = entry.Parameters[14];
            entity.Parameter15 = entry.Parameters[15];
            entity.Parameter16 = entry.Parameters[16];
            entity.Parameter17 = entry.Parameters[17];
            entity.Parameter18 = entry.Parameters[18];
            entity.Parameter19 = entry.Parameters[19];
            entity.Parameter20 = entry.Parameters[20];
            entity.Base = entry.Base;
            entity.IsVirtual = System.Convert.ToInt32(entry.IsVirtual);
            entity.Source = entry.Source;
        }
    }
}
