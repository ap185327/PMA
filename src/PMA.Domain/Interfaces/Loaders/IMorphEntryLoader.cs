// <copyright file="IMorphEntryLoader.cs" company="Andrey Pospelov">
//     Copyright 2017-2021 Andrey Pospelov. All rights reserved.
// </copyright>

using PMA.Domain.Interfaces.Loaders.Base;
using PMA.Domain.Models;
using System.Collections.Generic;

namespace PMA.Domain.Interfaces.Loaders
{
    /// <summary>
    /// Initializes a new instance of the <see cref="IMorphEntryLoader"/> interfacing class.
    /// </summary>
    public interface IMorphEntryLoader : IMemoryLoader<IList<MorphEntry>>
    {
    }
}
