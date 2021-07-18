// <copyright file="MorphParameterEntity.cs" company="Andrey Pospelov">
//     Copyright 2017-2021 Andrey Pospelov. All rights reserved.
// </copyright>

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PMA.Infrastructure.Entities
{
    /// <summary>
    /// Class for the SettingEntity model.
    /// </summary>
    [Table("MorphParameters")]
    public class MorphParameterEntity
    {
        /// <summary>
        /// Gets or set a parameter ID.
        /// </summary>
        [Key]
        [Column("Id", TypeName = "INTEGER")]
        public int Id { get; set; }

        /// <summary>
        /// Gets or set a parameter category ID.
        /// </summary>
        [Column("CategoryId", TypeName = "INT")]
        public int CategoryId { get; set; }

        /// <summary>
        /// Gets or sets whether an alternative property name is used.
        /// </summary>
        [Column("UseAltPropertyEntry", TypeName = "INT")]
        public int UseAltPropertyEntry { get; set; }
    }
}
