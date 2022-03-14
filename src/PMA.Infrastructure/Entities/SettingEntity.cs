// <copyright file="SettingEntity.cs" company="Andrey Pospelov">
//     Copyright 2017-2021 Andrey Pospelov. All rights reserved.
// </copyright>

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PMA.Infrastructure.Entities
{
    /// <summary>
    /// Class for the SettingEntity model.
    /// </summary>
    [Table("Settings")]
    public class SettingEntity
    {
        /// <summary>
        /// Gets or set a SettingEntity ID.
        /// </summary>
        [Key]
        [Column("Id", TypeName = "INTEGER")]
        public int Id { get; set; }

        /// <summary>
        /// Gets or set a SettingEntity Name.
        /// </summary>
        [Column("Name", TypeName = "VARCHAR")]
        [Required(AllowEmptyStrings = false)]
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string Name { get; set; }

        /// <summary>
        /// Gets or set a SettingEntity value.
        /// </summary>
        [Column("Value", TypeName = "VARCHAR")]
        [Required(AllowEmptyStrings = true)]
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string Value { get; set; }
    }
}
