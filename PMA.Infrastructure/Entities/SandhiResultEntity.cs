// <copyright file="SandhiResultEntity.cs" company="Andrey Pospelov">
//     Copyright 2017-2021 Andrey Pospelov. All rights reserved.
// </copyright>

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PMA.Infrastructure.Entities
{
    /// <summary>
    /// Class for the SandhiResultEntity model.
    /// </summary>
    [Table("SandhiResults")]
    public class SandhiResultEntity
    {
        /// <summary>
        /// Gets or sets a SandhiResultEntity ID.
        /// </summary>
        [Key]
        [Column("Id", TypeName = "INTEGER")]
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets a sandhi rule ID.
        /// </summary>
        [Column("RuleId", TypeName = "INT")]
        public int RuleId { get; set; }

        /// <summary>
        /// Gets or sets a sandhi rule regex result.
        /// </summary>
        [Column("RegexResult", TypeName = "VARCHAR")]
        [Required(AllowEmptyStrings = true)]
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string RegexResult { get; set; }
    }
}
