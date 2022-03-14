// <copyright file="StringEntity.cs" company="Andrey Pospelov">
//     Copyright 2017-2021 Andrey Pospelov. All rights reserved.
// </copyright>

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PMA.Infrastructure.Entities
{
    /// <summary>
    /// Class for the StringEntity model.
    /// </summary>
    [Table("Strings")]
    public class StringEntity
    {
        /// <summary>
        /// Gets or set a StringEntity ID.
        /// </summary>
        [Key]
        [Column("Id", TypeName = "INTEGER")]
        public int Id { get; set; }

        /// <summary>
        /// Gets or set a StringEntity Name.
        /// </summary>
        [Column("Name", TypeName = "VARCHAR")]
        [Required(AllowEmptyStrings = false)]
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string Name { get; set; }

        /// <summary>
        /// Gets or set a StringEntity value.
        /// </summary>
        [Column("EN", TypeName = "VARCHAR")]
        [Required(AllowEmptyStrings = false)]
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string Value { get; set; }
    }
}
