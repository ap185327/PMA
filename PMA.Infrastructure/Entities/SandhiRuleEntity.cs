// <copyright file="SandhiRuleEntity.cs" company="Andrey Pospelov">
//     Copyright 2017-2021 Andrey Pospelov. All rights reserved.
// </copyright>

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PMA.Infrastructure.Entities
{
    /// <summary>
    /// Class for the SandhiRuleEntity model.
    /// </summary>
    [Table("SandhiRules")]
    public class SandhiRuleEntity
    {
        /// <summary>
        /// Gets or sets a SandhiRuleEntity ID.
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
        /// Gets or sets a sandhi group ID.
        /// </summary>
        [Column("GroupId", TypeName = "INT")]
        public int GroupId { get; set; }

        /// <summary>
        /// Gets or sets a sandhi rule regular expression.
        /// </summary>
        [Column("Regex", TypeName = "VARCHAR")]
        [Required(AllowEmptyStrings = true)]
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string Regex { get; set; }

        /// <summary>
        /// Gets or sets a sandhi rule description.
        /// </summary>
        [Column("Description", TypeName = "VARCHAR")]
        [Required(AllowEmptyStrings = true)]
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string Description { get; set; }
    }
}
