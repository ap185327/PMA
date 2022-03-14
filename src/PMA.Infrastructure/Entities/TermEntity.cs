// <copyright file="TermEntity.cs" company="Andrey Pospelov">
//     Copyright 2017-2021 Andrey Pospelov. All rights reserved.
// </copyright>

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PMA.Infrastructure.Entities
{
    /// <summary>
    /// Class for the TermEntity model.
    /// </summary>
    [Table("Terms")]
    public class TermEntity
    {
        /// <summary>
        /// Gets or set a TermEntity ID.
        /// </summary>
        [Key]
        [Column("Id", TypeName = "INTEGER")]
        public byte Id { get; set; }

        /// <summary>
        /// Gets or set a TermEntity entry.
        /// </summary>
        [Column("Entry", TypeName = "VARCHAR")]
        [Required(AllowEmptyStrings = false)]
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string Entry { get; set; }

        /// <summary>
        /// Gets or set a TermEntity alternative entry.
        /// </summary>
        [Column("AltEntry", TypeName = "VARCHAR")]
        [Required(AllowEmptyStrings = true)]
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string AltEntry { get; set; }

        /// <summary>
        /// Gets or set a TermEntity alternative property entry.
        /// </summary>
        [Column("AltPropEntry", TypeName = "VARCHAR")]
        [Required(AllowEmptyStrings = true)]
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string AltPropEntry { get; set; }
    }
}
