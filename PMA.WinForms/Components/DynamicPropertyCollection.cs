// <copyright file="DynamicPropertyCollection.cs" company="Andrey Pospelov">
//     Copyright 2017-2021 Andrey Pospelov. All rights reserved.
// </copyright>

using PMA.WinForms.Models;
using System;
using System.Collections;
using System.ComponentModel;

namespace PMA.WinForms.Components
{
    /// <summary>
    /// Initializes a new instance of the DynamicPropertyCollection class.
    /// </summary>
    internal class DynamicPropertyCollection : CollectionBase, ICustomTypeDescriptor
    {
        #region ICustomTypeDescriptor Implementation

        /// <summary>
        /// Returns a collection of custom attributes for this instance of a component.
        /// </summary>
        /// <returns>An <see cref="AttributeCollection">AttributeCollection</see> containing the attributes for this object.</returns>
        public AttributeCollection GetAttributes()
        {
            return TypeDescriptor.GetAttributes(this, true);
        }

        /// <summary>
        /// Returns the class name of this instance of a component.
        /// </summary>
        /// <returns>The class name of the object, or null if the class does not have a name.</returns>
        public string GetClassName()
        {
            return TypeDescriptor.GetClassName(this, true);
        }

        /// <summary>
        /// Returns the name of this instance of a component.
        /// </summary>
        /// <returns>The name of the object, or null if the object does not have a name.</returns>
        public string GetComponentName()
        {
            return TypeDescriptor.GetComponentName(this, true);
        }

        /// <summary>
        /// Returns a type converter for this instance of a component.
        /// </summary>
        /// <returns>A <see cref="TypeConverter">TypeConverter</see> that is the converter for this object, or null if there is no <see cref="TypeConverter">TypeConverter</see> for this object.</returns>
        public TypeConverter GetConverter()
        {
            return TypeDescriptor.GetConverter(this, true);
        }

        /// <summary>
        /// Returns the default event for this instance of a component.
        /// </summary>
        /// <returns>An <see cref="EventDescriptor">EventDescriptor</see> that represents the default event for this object, or null if this object does not have events.</returns>
        public EventDescriptor GetDefaultEvent()
        {
            return TypeDescriptor.GetDefaultEvent(this, true);
        }

        /// <summary>
        /// Returns the default property for this instance of a component.
        /// </summary>
        /// <returns>A <see cref="PropertyDescriptor">PropertyDescriptor</see> that represents the default property for this object, or null if this object does not have properties.</returns>
        public PropertyDescriptor GetDefaultProperty()
        {
            return TypeDescriptor.GetDefaultProperty(this, true);
        }

        /// <summary>
        /// Returns an editor of the specified type for this instance of a component.
        /// </summary>
        /// <param name="editorBaseType">A System.Type that represents the editor for this object.</param>
        /// <returns>An <see cref="object">object</see> of the specified type that is the editor for this object, or null if the editor cannot be found.</returns>
        public object GetEditor(Type editorBaseType)
        {
            return TypeDescriptor.GetEditor(this, editorBaseType, true);
        }

        /// <summary>
        /// Returns the events for this instance of a component.
        /// </summary>
        /// <returns>An <see cref="EventDescriptorCollection">EventDescriptorCollection</see> that represents the events for this component instance.</returns>
        public EventDescriptorCollection GetEvents()
        {
            return TypeDescriptor.GetEvents(this, true);
        }

        /// <summary>
        /// Returns the events for this instance of a component using the specified attribute array as a filter.
        /// </summary>
        /// <param name="attributes">An array of type <see cref="Attribute">Attribute</see> that is used as a filter.</param>
        /// <returns>An <see cref="EventDescriptorCollection">EventDescriptorCollection</see> that represents the filtered events for this component instance.</returns>
        public EventDescriptorCollection GetEvents(Attribute[] attributes)
        {
            return TypeDescriptor.GetEvents(this, attributes, true);
        }

        /// <summary>
        /// Returns the properties for this instance of a component.
        /// </summary>
        /// <returns>A <see cref="PropertyDescriptorCollection">PropertyDescriptorCollection</see> that represents the properties for this component instance.</returns>
        public PropertyDescriptorCollection GetProperties()
        {
            return TypeDescriptor.GetProperties(this, true);
        }

        /// <summary>
        /// Returns the properties for this instance of a component using the attribute array as a filter.
        /// </summary>
        /// <param name="attributes">An array of type <see cref="Attribute">Attribute</see> that is used as a filter.</param>
        /// <returns>A <see cref="PropertyDescriptorCollection">PropertyDescriptorCollection</see> that represents the filtered properties for this component instance.</returns>
        public PropertyDescriptorCollection GetProperties(Attribute[] attributes)
        {
            var newProps = new PropertyDescriptor[Count];
            for (int i = 0; i < Count; i++)
            {
                var prop = this[i];
                newProps[i] = new DynamicPropertyDescriptor(ref prop, attributes);
            }
            return new PropertyDescriptorCollection(newProps);
        }

        /// <summary>
        /// Returns an object that contains the property described by the specified property descriptor.
        /// </summary>
        /// <param name="pd">A System.ComponentModel.PropertyDescriptor that represents the property whose owner is to be found.</param>
        /// <returns>An <see cref="object">object</see> that represents the owner of the specified property.</returns>
        public object GetPropertyOwner(PropertyDescriptor pd)
        {
            return this;
        }

        #endregion ICustomTypeDescriptor Implementation

        /// <summary>
        /// Add the dynamic property to the list.
        /// </summary>
        /// <param name="value">The dynamic property.</param>
        public void Add(DynamicProperty value)
        {
            List.Add(value);
        }

        /// <summary>
        /// Remove dynamic property from the list.
        /// </summary>
        /// <param name="name">The dynamic property name.</param>
        public void Remove(string name)
        {
            foreach (DynamicProperty prop in List)
            {
                if (prop.Name != name) continue;

                List.Remove(prop);
                return;
            }
        }

        /// <summary>
        /// Indexer.
        /// </summary>
        public DynamicProperty this[int index]
        {
            get => (DynamicProperty)List[index];
            set => List[index] = value;
        }
    }
}
