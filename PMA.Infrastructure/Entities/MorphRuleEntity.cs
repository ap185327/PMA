// <copyright file="MorphRuleEntity.cs" company="Andrey Pospelov">
//     Copyright 2017-2021 Andrey Pospelov. All rights reserved.
// </copyright>

using PMA.Domain.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PMA.Infrastructure.Entities
{
    /// <summary>
    /// Class for the MorphEntryEntity model.
    /// </summary>
    [Table("MorphRules")]
    public class MorphRuleEntity
    {
        /// <summary>
        /// Gets or sets a MorphRuleEntity ID.
        /// </summary>
        [Key]
        [Column("Id", TypeName = "INTEGER")]
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets a morphological rule ID.
        /// </summary>
        [Column("RuleId", TypeName = "INT")]
        public int RuleId { get; set; }

        /// <summary>
        /// Gets or sets whether collapsing the morphological rule is required.
        /// </summary>
        [Column("IsCollapsed", TypeName = "INT")]
        public int IsCollapsed { get; set; }

        /// <summary>
        /// Gets or sets a rule label.
        /// </summary>
        [Column("Label", TypeName = "VARCHAR")]
        [Required(AllowEmptyStrings = true)]
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string Label { get; set; }

        /// <summary>
        /// Gets or sets the sandhi group ID.
        /// </summary>
        [Column("SandhiGroupId", TypeName = "INT")]
        public int SandhiGroupId { get; set; }

        /// <summary>
        /// Gets or sets a rule entry expression.
        /// </summary>
        [Column("Entry", TypeName = "VARCHAR")]
        [Required(AllowEmptyStrings = true)]
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string Entry { get; set; }

        /// <summary>
        /// Gets or sets a parameters[0] value.
        /// </summary>
        [Column("0", TypeName = "INT")]
        public byte Parameter0 { get; set; }

        /// <summary>
        /// Gets or sets a parameters[1] value.
        /// </summary>
        [Column("1", TypeName = "INT")]
        public byte Parameter1 { get; set; }

        /// <summary>
        /// Gets or sets a parameters[2] value.
        /// </summary>
        [Column("2", TypeName = "INT")]
        public byte Parameter2 { get; set; }

        /// <summary>
        /// Gets or sets a parameters[3] value.
        /// </summary>
        [Column("3", TypeName = "INT")]
        public byte Parameter3 { get; set; }

        /// <summary>
        /// Gets or sets a parameters[4] value.
        /// </summary>
        [Column("4", TypeName = "INT")]
        public byte Parameter4 { get; set; }

        /// <summary>
        /// Gets or sets a parameters[5] value.
        /// </summary>
        [Column("5", TypeName = "INT")]
        public byte Parameter5 { get; set; }

        /// <summary>
        /// Gets or sets a parameters[6] value.
        /// </summary>
        [Column("6", TypeName = "INT")]
        public byte Parameter6 { get; set; }

        /// <summary>
        /// Gets or sets a parameters[7] value.
        /// </summary>
        [Column("7", TypeName = "INT")]
        public byte Parameter7 { get; set; }

        /// <summary>
        /// Gets or sets a parameters[8] value.
        /// </summary>
        [Column("8", TypeName = "INT")]
        public byte Parameter8 { get; set; }

        /// <summary>
        /// Gets or sets a parameters[9] value.
        /// </summary>
        [Column("9", TypeName = "INT")]
        public byte Parameter9 { get; set; }

        /// <summary>
        /// Gets or sets a parameters[10] value.
        /// </summary>
        [Column("10", TypeName = "INT")]
        public byte Parameter10 { get; set; }

        /// <summary>
        /// Gets or sets a parameters[11] value.
        /// </summary>
        [Column("11", TypeName = "INT")]
        public byte Parameter11 { get; set; }

        /// <summary>
        /// Gets or sets a parameters[12] value.
        /// </summary>
        [Column("12", TypeName = "INT")]
        public byte Parameter12 { get; set; }

        /// <summary>
        /// Gets or sets a parameters[13] value.
        /// </summary>
        [Column("13", TypeName = "INT")]
        public byte Parameter13 { get; set; }

        /// <summary>
        /// Gets or sets a parameters[14] value.
        /// </summary>
        [Column("14", TypeName = "INT")]
        public byte Parameter14 { get; set; }

        /// <summary>
        /// Gets or sets a parameters[15] value.
        /// </summary>
        [Column("15", TypeName = "INT")]
        public byte Parameter15 { get; set; }

        /// <summary>
        /// Gets or sets a parameters[16] value.
        /// </summary>
        [Column("16", TypeName = "INT")]
        public byte Parameter16 { get; set; }

        /// <summary>
        /// Gets or sets a parameters[17] value.
        /// </summary>
        [Column("17", TypeName = "INT")]
        public byte Parameter17 { get; set; }

        /// <summary>
        /// Gets or sets a parameters[18] value.
        /// </summary>
        [Column("18", TypeName = "INT")]
        public byte Parameter18 { get; set; }

        /// <summary>
        /// Gets or sets a parameters[19] value.
        /// </summary>
        [Column("19", TypeName = "INT")]
        public byte Parameter19 { get; set; }

        /// <summary>
        /// Gets or sets a parameters[20] value.
        /// </summary>
        [Column("20", TypeName = "INT")]
        public byte Parameter20 { get; set; }

        /// <summary>
        /// Gets or sets a morphological base.
        /// </summary>
        [Column("Base", TypeName = "INT")]
        public MorphBase Base { get; set; }

        /// <summary>
        /// Gets or sets an inheritance type for the left wordform.
        /// </summary>
        [Column("LeftType", TypeName = "INT")]
        public MorphRuleType LeftType { get; set; }

        /// <summary>
        /// Gets or sets a label for the left wordform.
        /// </summary>
        [Column("LeftLabel", TypeName = "VARCHAR")]
        [Required(AllowEmptyStrings = true)]
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string LeftLabel { get; set; }

        /// <summary>
        /// Gets or sets an entry expression for the left wordform.
        /// </summary>
        [Column("LeftEntry", TypeName = "VARCHAR")]
        [Required(AllowEmptyStrings = true)]
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string LeftEntry { get; set; }

        /// <summary>
        /// Gets or sets a parameters[0] value for the left wordform.
        /// </summary>
        [Column("Left0", TypeName = "INT")]
        public byte LeftParameter0 { get; set; }

        /// <summary>
        /// Gets or sets a parameters[1] value for the left wordform.
        /// </summary>
        [Column("Left1", TypeName = "INT")]
        public byte LeftParameter1 { get; set; }

        /// <summary>
        /// Gets or sets a parameters[2] value for the left wordform.
        /// </summary>
        [Column("Left2", TypeName = "INT")]
        public byte LeftParameter2 { get; set; }

        /// <summary>
        /// Gets or sets a parameters[3] value for the left wordform.
        /// </summary>
        [Column("Left3", TypeName = "INT")]
        public byte LeftParameter3 { get; set; }

        /// <summary>
        /// Gets or sets a parameters[4] value for the left wordform.
        /// </summary>
        [Column("Left4", TypeName = "INT")]
        public byte LeftParameter4 { get; set; }

        /// <summary>
        /// Gets or sets a parameters[5] value for the left wordform.
        /// </summary>
        [Column("Left5", TypeName = "INT")]
        public byte LeftParameter5 { get; set; }

        /// <summary>
        /// Gets or sets a parameters[6] value for the left wordform.
        /// </summary>
        [Column("Left6", TypeName = "INT")]
        public byte LeftParameter6 { get; set; }

        /// <summary>
        /// Gets or sets a parameters[7] value for the left wordform.
        /// </summary>
        [Column("Left7", TypeName = "INT")]
        public byte LeftParameter7 { get; set; }

        /// <summary>
        /// Gets or sets a parameters[8] value for the left wordform.
        /// </summary>
        [Column("Left8", TypeName = "INT")]
        public byte LeftParameter8 { get; set; }

        /// <summary>
        /// Gets or sets a parameters[9] value for the left wordform.
        /// </summary>
        [Column("Left9", TypeName = "INT")]
        public byte LeftParameter9 { get; set; }

        /// <summary>
        /// Gets or sets a parameters[10] value for the left wordform.
        /// </summary>
        [Column("Left10", TypeName = "INT")]
        public byte LeftParameter10 { get; set; }

        /// <summary>
        /// Gets or sets a parameters[11] value for the left wordform.
        /// </summary>
        [Column("Left11", TypeName = "INT")]
        public byte LeftParameter11 { get; set; }

        /// <summary>
        /// Gets or sets a parameters[12] value for the left wordform.
        /// </summary>
        [Column("Left12", TypeName = "INT")]
        public byte LeftParameter12 { get; set; }

        /// <summary>
        /// Gets or sets a parameters[13] value for the left wordform.
        /// </summary>
        [Column("Left13", TypeName = "INT")]
        public byte LeftParameter13 { get; set; }

        /// <summary>
        /// Gets or sets a parameters[14] value for the left wordform.
        /// </summary>
        [Column("Left14", TypeName = "INT")]
        public byte LeftParameter14 { get; set; }

        /// <summary>
        /// Gets or sets a parameters[15] value for the left wordform.
        /// </summary>
        [Column("Left15", TypeName = "INT")]
        public byte LeftParameter15 { get; set; }

        /// <summary>
        /// Gets or sets a parameters[16] value for the left wordform.
        /// </summary>
        [Column("Left16", TypeName = "INT")]
        public byte LeftParameter16 { get; set; }

        /// <summary>
        /// Gets or sets a parameters[17] value for the left wordform.
        /// </summary>
        [Column("Left17", TypeName = "INT")]
        public byte LeftParameter17 { get; set; }

        /// <summary>
        /// Gets or sets a parameters[18] value for the left wordform.
        /// </summary>
        [Column("Left18", TypeName = "INT")]
        public byte LeftParameter18 { get; set; }

        /// <summary>
        /// Gets or sets a parameters[19] value for the left wordform.
        /// </summary>
        [Column("Left19", TypeName = "INT")]
        public byte LeftParameter19 { get; set; }

        /// <summary>
        /// Gets or sets a parameters[20] value for the left wordform.
        /// </summary>
        [Column("Left20", TypeName = "INT")]
        public byte LeftParameter20 { get; set; }

        /// <summary>
        /// Gets or sets an inheritance type for the right wordform.
        /// </summary>
        [Column("RightType", TypeName = "INT")]
        public MorphRuleType RightType { get; set; }

        /// <summary>
        /// Gets or sets a label for the right wordform.
        /// </summary>
        [Column("RightLabel", TypeName = "VARCHAR")]
        [Required(AllowEmptyStrings = true)]
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string RightLabel { get; set; }

        /// <summary>
        /// Gets or sets an entry expression for the right wordform.
        /// </summary>
        [Column("RightEntry", TypeName = "VARCHAR")]
        [Required(AllowEmptyStrings = true)]
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string RightEntry { get; set; }

        /// <summary>
        /// Gets or sets a parameters[0] value for the right wordform.
        /// </summary>
        [Column("Right0", TypeName = "INT")]
        public byte RightParameter0 { get; set; }

        /// <summary>
        /// Gets or sets a parameters[1] value for the right wordform.
        /// </summary>
        [Column("Right1", TypeName = "INT")]
        public byte RightParameter1 { get; set; }

        /// <summary>
        /// Gets or sets a parameters[2] value for the right wordform.
        /// </summary>
        [Column("Right2", TypeName = "INT")]
        public byte RightParameter2 { get; set; }

        /// <summary>
        /// Gets or sets a parameters[3] value for the right wordform.
        /// </summary>
        [Column("Right3", TypeName = "INT")]
        public byte RightParameter3 { get; set; }

        /// <summary>
        /// Gets or sets a parameters[4] value for the right wordform.
        /// </summary>
        [Column("Right4", TypeName = "INT")]
        public byte RightParameter4 { get; set; }

        /// <summary>
        /// Gets or sets a parameters[5] value for the right wordform.
        /// </summary>
        [Column("Right5", TypeName = "INT")]
        public byte RightParameter5 { get; set; }

        /// <summary>
        /// Gets or sets a parameters[6] value for the right wordform.
        /// </summary>
        [Column("Right6", TypeName = "INT")]
        public byte RightParameter6 { get; set; }

        /// <summary>
        /// Gets or sets a parameters[7] value for the right wordform.
        /// </summary>
        [Column("Right7", TypeName = "INT")]
        public byte RightParameter7 { get; set; }

        /// <summary>
        /// Gets or sets a parameters[8] value for the right wordform.
        /// </summary>
        [Column("Right8", TypeName = "INT")]
        public byte RightParameter8 { get; set; }

        /// <summary>
        /// Gets or sets a parameters[9] value for the right wordform.
        /// </summary>
        [Column("Right9", TypeName = "INT")]
        public byte RightParameter9 { get; set; }

        /// <summary>
        /// Gets or sets a parameters[10] value for the right wordform.
        /// </summary>
        [Column("Right10", TypeName = "INT")]
        public byte RightParameter10 { get; set; }

        /// <summary>
        /// Gets or sets a parameters[11] value for the right wordform.
        /// </summary>
        [Column("Right11", TypeName = "INT")]
        public byte RightParameter11 { get; set; }

        /// <summary>
        /// Gets or sets a parameters[12] value for the right wordform.
        /// </summary>
        [Column("Right12", TypeName = "INT")]
        public byte RightParameter12 { get; set; }

        /// <summary>
        /// Gets or sets a parameters[13] value for the right wordform.
        /// </summary>
        [Column("Right13", TypeName = "INT")]
        public byte RightParameter13 { get; set; }

        /// <summary>
        /// Gets or sets a parameters[14] value for the right wordform.
        /// </summary>
        [Column("Right14", TypeName = "INT")]
        public byte RightParameter14 { get; set; }

        /// <summary>
        /// Gets or sets a parameters[15] value for the right wordform.
        /// </summary>
        [Column("Right15", TypeName = "INT")]
        public byte RightParameter15 { get; set; }

        /// <summary>
        /// Gets or sets a parameters[16] value for the right wordform.
        /// </summary>
        [Column("Right16", TypeName = "INT")]
        public byte RightParameter16 { get; set; }

        /// <summary>
        /// Gets or sets a parameters[17] value for the right wordform.
        /// </summary>
        [Column("Right17", TypeName = "INT")]
        public byte RightParameter17 { get; set; }

        /// <summary>
        /// Gets or sets a parameters[18] value for the right wordform.
        /// </summary>
        [Column("Right18", TypeName = "INT")]
        public byte RightParameter18 { get; set; }

        /// <summary>
        /// Gets or sets a parameters[19] value for the right wordform.
        /// </summary>
        [Column("Right19", TypeName = "INT")]
        public byte RightParameter19 { get; set; }

        /// <summary>
        /// Gets or sets a parameters[20] value for the right wordform.
        /// </summary>
        [Column("Right20", TypeName = "INT")]
        public byte RightParameter20 { get; set; }

        /// <summary>
        /// Gets or sets the rule rating.
        /// </summary>
        [Column("Rating", TypeName = "REAL")]
        public double Rating { get; set; }

        /// <summary>
        /// Gets or sets the rule description.
        /// </summary>
        [Column("Description", TypeName = "VARCHAR")]
        [Required(AllowEmptyStrings = true)]
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string Description { get; set; }
    }
}
