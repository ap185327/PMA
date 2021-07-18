// <copyright file="DynamicPropertyType.cs" company="Andrey Pospelov">
//     Copyright 2017-2021 Andrey Pospelov. All rights reserved.
// </copyright>

using PMA.WinForms.Components;
using System.ComponentModel;

namespace PMA.WinForms.Types
{
    /// <summary>
    /// The dynamic property type.
    /// </summary>
    [TypeConverter(typeof(DynamicPropertyConverter))]
    internal class DynamicPropertyType
    {
    }
}
