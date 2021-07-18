// <copyright file="GetMorphEntryRequest.cs" company="Andrey Pospelov">
//     Copyright 2017-2021 Andrey Pospelov. All rights reserved.
// </copyright>

using MediatR;
using PMA.Domain.Models;

namespace PMA.Domain.Requests
{
    /// <summary>
    /// The get morphological entry request class.
    /// </summary>
    public class GetMorphEntryRequest : IRequest<MorphEntry>
    {
    }
}
