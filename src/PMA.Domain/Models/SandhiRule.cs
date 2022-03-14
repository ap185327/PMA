// <copyright file="SandhiRule.cs" company="Andrey Pospelov">
//     Copyright 2017-2021 Andrey Pospelov. All rights reserved.
// </copyright>

using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace PMA.Domain.Models
{
    /// <summary>
    /// Class for the SandhiRule rule model.
    /// </summary>
    public class SandhiRule
    {
        /// <summary>
        /// Gets or sets a SandhiRule ID.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets a sandhi regex.
        /// </summary>
        public Regex Regex { get; set; }

        /// <summary>
        /// Gets or sets a rule description.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets a collection of replacement patterns.
        /// </summary>
        public IList<string> RegexResults { get; set; }
    }
}
