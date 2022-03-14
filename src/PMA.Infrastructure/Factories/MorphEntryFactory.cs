// <copyright file="MorphEntryFactory.cs" company="Andrey Pospelov">
//     Copyright 2017-2021 Andrey Pospelov. All rights reserved.
// </copyright>

using PMA.Domain.Enums;
using PMA.Domain.Models;
using PMA.Infrastructure.Entities;
using PMA.Utils.Extensions;
using System;
using System.Data;

namespace PMA.Infrastructure.Factories
{
    /// <summary>
    /// The morphological entry factory class.
    /// </summary>
    public static class MorphEntryFactory
    {
        /// <summary>
        /// Create a new instance of the <see cref="MorphEntry"/> class.
        /// </summary>
        /// <param name="entity">The morphological entry entity.</param>
        /// <returns>A new instance of the <see cref="MorphEntry"/> class.</returns>
        public static MorphEntry Create(MorphEntryEntity entity)
        {
            return new()
            {
                Id = entity.Id,
                Entry = entity.Entry,
                Parameters = ParameterFactory.Create(entity),
                Base = entity.Base,
                IsVirtual = Convert.ToBoolean(entity.IsVirtual),
                Source = entity.Source
            };
        }

        /// <summary>
        /// Create a new instance of the <see cref="MorphEntry"/> class.
        /// </summary>
        /// <param name="row">The data row.</param>
        /// <returns>A new instance of the <see cref="MorphEntry"/> class.</returns>
        public static MorphEntry Create(DataRow row)
        {
            static MorphEntry GetMorphEntry(string value)
            {
                if (string.IsNullOrEmpty(value) || value == "0") return null;

                return int.TryParse(value, out int id)
                    ? Application.Factories.MorphEntryFactory.Create(id)
                    : Application.Factories.MorphEntryFactory.Create(value);
            }

            return new MorphEntry
            {
                Entry = (string)row["Entry"],
                Parameters = Application.Factories.ParameterFactory.Create(row),
                Base = (MorphBase)Enum.Parse(typeof(MorphBase), ((string)row["Base"]).FirstCharToUpper()),
                IsVirtual = Convert.ToBoolean(row["IsVirtual"]),
                Left = GetMorphEntry((string)row["LeftEntry"]),
                Right = GetMorphEntry((string)row["RightEntry"])
            };
        }
    }
}
