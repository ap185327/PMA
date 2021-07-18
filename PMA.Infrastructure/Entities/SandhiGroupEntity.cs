// <copyright file="SandhiGroupEntity.cs" company="Andrey Pospelov">
//     Copyright 2017-2021 Andrey Pospelov. All rights reserved.
// </copyright>

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PMA.Infrastructure.Entities
{
    /// <summary>
    /// Class for the SandhiGroupEntity model.
    /// </summary>
    [Table("SandhiGroups")]
    public class SandhiGroupEntity
    {
        /// <summary>
        /// Gets or sets a SandhiGroupEntity ID.
        /// </summary>
        [Key]
        [Column("Id", TypeName = "INTEGER")]
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets a sandhi group entry.
        /// </summary>
        [Column("Entry", TypeName = "VARCHAR")]
        [Required(AllowEmptyStrings = false)]
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string Entry { get; set; }
    }
}
