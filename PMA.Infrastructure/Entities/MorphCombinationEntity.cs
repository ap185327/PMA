// <copyright file="MorphCombinationEntity.cs" company="Andrey Pospelov">
//     Copyright 2017-2021 Andrey Pospelov. All rights reserved.
// </copyright>

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PMA.Infrastructure.Entities
{
    /// <summary>
    /// Class for the MorphCombinationEntity model.
    /// </summary>
    [Table("MorphCombinations")]
    public class MorphCombinationEntity
    {
        /// <summary>
        /// Gets or set a MorphCombinationEntity ID.
        /// </summary>
        [Key]
        [Column("Id", TypeName = "INTEGER")]
        public int Id { get; set; }

        /// <summary>
        /// Gets or set a parameters[0] value.
        /// </summary>
        [Column("0", TypeName = "INT")]
        public byte Parameter0 { get; set; }

        /// <summary>
        /// Gets or set a parameters[1] value.
        /// </summary>
        [Column("1", TypeName = "INT")]
        public byte Parameter1 { get; set; }

        /// <summary>
        /// Gets or set a parameters[2] value.
        /// </summary>
        [Column("2", TypeName = "INT")]
        public byte Parameter2 { get; set; }

        /// <summary>
        /// Gets or set a parameters[3] value.
        /// </summary>
        [Column("3", TypeName = "INT")]
        public byte Parameter3 { get; set; }

        /// <summary>
        /// Gets or set a parameters[4] value.
        /// </summary>
        [Column("4", TypeName = "INT")]
        public byte Parameter4 { get; set; }

        /// <summary>
        /// Gets or set a parameters[5] value.
        /// </summary>
        [Column("5", TypeName = "INT")]
        public byte Parameter5 { get; set; }

        /// <summary>
        /// Gets or set a parameters[6] value.
        /// </summary>
        [Column("6", TypeName = "INT")]
        public byte Parameter6 { get; set; }

        /// <summary>
        /// Gets or set a parameters[7] value.
        /// </summary>
        [Column("7", TypeName = "INT")]
        public byte Parameter7 { get; set; }

        /// <summary>
        /// Gets or set a parameters[8] value.
        /// </summary>
        [Column("8", TypeName = "INT")]
        public byte Parameter8 { get; set; }

        /// <summary>
        /// Gets or set a parameters[9] value.
        /// </summary>
        [Column("9", TypeName = "INT")]
        public byte Parameter9 { get; set; }

        /// <summary>
        /// Gets or set a parameters[10] value.
        /// </summary>
        [Column("10", TypeName = "INT")]
        public byte Parameter10 { get; set; }

        /// <summary>
        /// Gets or set a parameters[11] value.
        /// </summary>
        [Column("11", TypeName = "INT")]
        public byte Parameter11 { get; set; }

        /// <summary>
        /// Gets or set a parameters[12] value.
        /// </summary>
        [Column("12", TypeName = "INT")]
        public byte Parameter12 { get; set; }

        /// <summary>
        /// Gets or set a parameters[13] value.
        /// </summary>
        [Column("13", TypeName = "INT")]
        public byte Parameter13 { get; set; }

        /// <summary>
        /// Gets or set a parameters[14] value.
        /// </summary>
        [Column("14", TypeName = "INT")]
        public byte Parameter14 { get; set; }

        /// <summary>
        /// Gets or set a parameters[15] value.
        /// </summary>
        [Column("15", TypeName = "INT")]
        public byte Parameter15 { get; set; }

        /// <summary>
        /// Gets or set a parameters[16] value.
        /// </summary>
        [Column("16", TypeName = "INT")]
        public byte Parameter16 { get; set; }

        /// <summary>
        /// Gets or set a parameters[17] value.
        /// </summary>
        [Column("17", TypeName = "INT")]
        public byte Parameter17 { get; set; }

        /// <summary>
        /// Gets or set a parameters[18] value.
        /// </summary>
        [Column("18", TypeName = "INT")]
        public byte Parameter18 { get; set; }

        /// <summary>
        /// Gets or set a parameters[19] value.
        /// </summary>
        [Column("19", TypeName = "INT")]
        public byte Parameter19 { get; set; }

        /// <summary>
        /// Gets or set a parameters[20] value.
        /// </summary>
        [Column("20", TypeName = "INT")]
        public byte Parameter20 { get; set; }
    }
}
