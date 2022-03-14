// <copyright file="UpdateMorphPropertyInputPort.cs" company="Andrey Pospelov">
//     Copyright 2017-2021 Andrey Pospelov. All rights reserved.
// </copyright>

using PMA.Domain.Interfaces.ViewModels.Controls;
using System.Collections.Generic;

namespace PMA.Domain.InputPorts
{
    /// <summary>
    /// The update morphological property input port class.
    /// </summary>
    public class UpdateMorphPropertyInputPort
    {
        /// <summary>
        /// Gets or sets start morphological property index for updating.
        /// </summary>
        public int StartIndex { get; init; }

        /// <summary>
        ///  Gets or sets a collection of morphological property view models.
        /// </summary>
        public IList<IMorphPropertyControlViewModel> Properties { get; init; }
    }
}
