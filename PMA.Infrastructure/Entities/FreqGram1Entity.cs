// <copyright file="FreqGram1Entity.cs" company="Andrey Pospelov">
//     Copyright 2017-2021 Andrey Pospelov. All rights reserved.
// </copyright>

using System.ComponentModel.DataAnnotations.Schema;

namespace PMA.Infrastructure.Entities
{
    /// <summary>
    /// Class for the FreqGram1Entity model.
    /// </summary>
    [Table("FreqGram1")]
    public class FreqGram1Entity
    {
        /// <summary>
        /// Gets or set a word count sum.
        /// </summary>
        [Column("Sum", TypeName = "INT")]
        public int Sum { get; set; }
    }
}
