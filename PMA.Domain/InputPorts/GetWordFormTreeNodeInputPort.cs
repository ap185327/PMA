// <copyright file="GetWordFormTreeNodeInputPort.cs" company="Andrey Pospelov">
//     Copyright 2017-2021 Andrey Pospelov. All rights reserved.
// </copyright>

using PMA.Domain.Interfaces.ViewModels.Controls;
using PMA.Domain.Models;

namespace PMA.Domain.InputPorts
{
    /// <summary>
    /// The get wordform tree node input port class.
    /// </summary>
    public class GetWordFormTreeNodeInputPort
    {
        /// <summary>
        /// Gets or initializes a parent tree node.
        /// </summary>
        public ISolutionTreeNodeViewModel Parent { get; init; }

        /// <summary>
        /// Gets or initializes a wordform.
        /// </summary>
        public WordForm WordForm { get; init; }
    }
}
