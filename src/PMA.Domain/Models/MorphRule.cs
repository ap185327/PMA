// <copyright file="MorphRule.cs" company="Andrey Pospelov">
//     Copyright 2017-2021 Andrey Pospelov. All rights reserved.
// </copyright>

using PMA.Domain.Enums;
using System.Collections.Generic;

namespace PMA.Domain.Models
{
    /// <summary>
    /// Class for the MorphRule model.
    /// </summary>
    public class MorphRule
    {
        /// <summary>
        /// Gets or sets a database entity ID.
        /// </summary>
        public int EntityId { get; set; }

        /// <summary>
        /// Gets or sets an MorphRule ID.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets whether collapsing a morphological rule is required.
        /// </summary>
        public bool IsCollapsed { get; set; }

        /// <summary>
        /// Gets or sets a label.
        /// </summary>
        public string Label { get; set; }

        /// <summary>
        /// Gets or sets whether to check result solution by the morphological combinations.
        /// </summary>
        public bool NeedToCheck { get; set; }

        /// <summary>
        /// Gets or sets a sandhi group ID.
        /// </summary>
        public int SandhiGroup { get; set; }

        /// <summary>
        /// Gets or sets a collection of sandhi rules.
        /// </summary>
        public IList<SandhiRule> SandhiRules { get; set; }

        /// <summary>
        /// Gets or sets an entry expression.
        /// </summary>
        public string Entry { get; set; }

        /// <summary>
        /// Gets or sets morphological parameters.
        /// </summary>
        public byte[] Parameters { get; set; }

        /// <summary>
        /// Gets or sets a morphological base.
        /// </summary>
        public MorphBase Base { get; set; }

        /// <summary>
        /// Gets or sets inheritance type for the left wordform.
        /// </summary>
        public MorphRuleType LeftType { get; set; }

        /// <summary>
        /// Gets or sets a label for the left wordform.
        /// </summary>
        public string LeftLabel { get; set; }

        /// <summary>
        /// Gets or sets an entry expression for the left wordform.
        /// </summary>
        public string Left { get; set; }

        /// <summary>
        /// Gets or sets morphological parameters for the left wordform.
        /// </summary>
        public byte[] LeftParameters { get; set; }

        /// <summary>
        /// Gets or sets inheritance type for the right wordform.
        /// </summary>
        public MorphRuleType RightType { get; set; }

        /// <summary>
        /// Gets or sets a label for the right wordform.
        /// </summary>
        public string RightLabel { get; set; }

        /// <summary>
        /// Gets or sets an entry expression for the right wordform.
        /// </summary>
        public string Right { get; set; }

        /// <summary>
        /// Gets or sets morphological parameters for the right wordform.
        /// </summary>
        public byte[] RightParameters { get; set; }

        /// <summary>
        /// Gets or sets a rule rating.
        /// </summary>
        public double Rating { get; set; }

        /// <summary>
        /// Gets or sets a rule description.
        /// </summary>
        public string Description { get; set; }
    }
}
