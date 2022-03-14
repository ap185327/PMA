// <copyright file="UpdateDbInputPort.cs" company="Andrey Pospelov">
//     Copyright 2017-2021 Andrey Pospelov. All rights reserved.
// </copyright>

namespace PMA.Domain.InputPorts
{
    /// <summary>
    /// The update database input port class.
    /// </summary>
    public class UpdateDbInputPort
    {
        /// <summary>
        /// Gets or sets a file path with PMA tables.
        /// </summary>
        public string DataFilePath { get; init; }

        /// <summary>
        /// Gets or sets whether the sandhi group database table is checked or not.
        /// </summary>
        public bool IsSandhiGroupDbTableChecked { get; init; }

        /// <summary>
        /// Gets or sets whether the sandhi rule database table is checked or not.
        /// </summary>
        public bool IsSandhiRuleDbTableChecked { get; init; }

        /// <summary>
        /// Gets or sets whether the morphological rule database table is checked or not.
        /// </summary>
        public bool IsMorphRuleDbTableChecked { get; init; }

        /// <summary>
        /// Gets or sets whether the morphological combination database table is checked or not.
        /// </summary>
        public bool IsMorphCombinationDbTableChecked { get; init; }
    }
}
