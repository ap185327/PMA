// <copyright file="DynamicPropertyDescriptor.cs" company="Andrey Pospelov">
//     Copyright 2017-2021 Andrey Pospelov. All rights reserved.
// </copyright>

using PMA.WinForms.Models;
using System;
using System.ComponentModel;

namespace PMA.WinForms.Components
{
    /// <summary>
    /// Initializes a new instance of the DynamicPropertyDescriptor class.
    /// </summary>
    internal class DynamicPropertyDescriptor : PropertyDescriptor
    {
        /// <summary>
        /// The dynamic property.
        /// </summary>
        private readonly DynamicProperty _property;

        #region PropertyDescriptor Implementation

        /// <summary>
        /// When overridden in a derived class, gets a value indicating whether this property is read-only. Returns true if the property is read-only; otherwise, false.
        /// </summary>
        public override bool IsReadOnly => _property.ReadOnly;

        /// <summary>
        /// When overridden in a derived class, gets the type of the component this property is bound to. Returns null.
        /// </summary>
        public override Type ComponentType => null;

        /// <summary>
        ///  When overridden in a derived class, gets the type of the property. Returns a <see cref="Type">Type</see> that represents the type of the property.
        /// </summary>
        public override Type PropertyType => _property.Type;

        /// <summary>
        /// When overridden in a derived class, returns whether resetting an object changes its value.
        /// </summary>
        /// <param name="component">The component to test for reset capability.</param>
        /// <returns>false.</returns>
        public override bool CanResetValue(object component) => false;

        /// <summary>
        /// When overridden in a derived class, gets the current value of the property on a component.
        /// </summary>
        /// <param name="component">The component with the property for which to retrieve the value.</param>
        /// <returns>The value of a property for a given component.</returns>
        public override object GetValue(object component) => _property.Value;

        /// <summary>
        /// Gets the name of the member.
        /// </summary>
        public override string Name => _property.Name;

        /// <summary>
        /// Gets the description of the member, as specified in the <see cref="DescriptionAttribute">DescriptionAttribute</see>.
        /// </summary>
        public override string Description => _property.Description;

        /// <summary>
        ///  Gets the name of the category to which the member belongs, as specified in the <see cref="CategoryAttribute">CategoryAttribute</see>.
        /// </summary>
        public override string Category => _property.Category;

        /// <summary>
        /// Gets the name that can be displayed in a window, such as a Properties window.
        /// </summary>
        public override string DisplayName => _property.Name;

        /// <summary>
        ///  When overridden in a derived class, resets the value for this property of the component to the default value.
        /// </summary>
        /// <param name="component">The component with the property value that is to be reset to the default value.</param>
        public override void ResetValue(object component)
        {
            //Have to implement
        }

        /// <summary>
        /// When overridden in a derived class, sets the value of the component to a different value.
        /// </summary>
        /// <param name="component">The component with the property value that is to be set.</param>
        /// <param name="value">The new value.</param>
        public override void SetValue(object component, object value) => _property.Value = value;

        /// <summary>
        /// When overridden in a derived class, determines a value indicating whether the value of this property needs to be persisted.
        /// </summary>
        /// <param name="component">The component with the property to be examined for persistence.</param>
        /// <returns>False.</returns>
        public override bool ShouldSerializeValue(object component) => false;

        #endregion PropertyDescriptor Implementation

        /// <summary>
        /// Initializes a new instance of the <see cref="DynamicPropertyDescriptor">DynamicPropertyDescriptor</see>.
        /// </summary>
        /// <param name="property">The dynamic property.</param>
        /// <param name="attrs">Attributes.</param>
        public DynamicPropertyDescriptor(ref DynamicProperty property, Attribute[] attrs) : base(property.Name, attrs) => _property = property;
    }
}
