// <copyright file="DynamicPropertyConverter.cs" company="Andrey Pospelov">
//     Copyright 2017-2021 Andrey Pospelov. All rights reserved.
// </copyright>

using PMA.WinForms.Controls;
using System.ComponentModel;

namespace PMA.WinForms.Components
{
    /// <summary>
    /// Initializes a new instance of the DynamicPropertyConverter class.
    /// </summary>
    internal class DynamicPropertyConverter : StringConverter
    {
        /// <summary>
        /// Returns a collection of standard values for the data type this type converter is designed for when provided with a format context.
        /// </summary>
        /// <param name="context">An <see cref="ITypeDescriptorContext">ITypeDescriptorContext</see> that provides a format context.</param>
        /// <returns>A <see cref="TypeConverter.StandardValuesCollection">StandardValuesCollection</see> that holds a standard set of valid values, or null if the data type does not support a standard set of values.</returns>
        public override StandardValuesCollection GetStandardValues(ITypeDescriptorContext context) => new StandardValuesCollection(DynamicPropertyGrid.SelectedPropertyValues);

        /// <summary>
        /// Returns whether the collection of standard values returned from <see cref="TypeConverter.GetStandardValues()"/> is an exclusive list of possible values, using the specified context.
        /// </summary>
        /// <param name="context">An <see cref="ITypeDescriptorContext">ITypeDescriptorContext</see> that provides a format context.</param>
        /// <returns>True.</returns>
        public override bool GetStandardValuesExclusive(ITypeDescriptorContext context) => true;

        /// <summary>
        /// Returns whether this object supports a standard set of values that can be picked from a list.
        /// </summary>
        /// <param name="context">An <see cref="ITypeDescriptorContext">ITypeDescriptorContext</see> that provides a format context.</param>
        /// <returns>True.</returns>
        public override bool GetStandardValuesSupported(ITypeDescriptorContext context) => true;
    }
}
