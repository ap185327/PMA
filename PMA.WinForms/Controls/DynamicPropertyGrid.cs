// <copyright file="DynamicPropertyGrid.cs" company="Andrey Pospelov">
//     Copyright 2017-2021 Andrey Pospelov. All rights reserved.
// </copyright>

using PMA.WinForms.Components;
using PMA.WinForms.EventArguments;
using PMA.WinForms.Models;
using PMA.WinForms.Types;
using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace PMA.WinForms.Controls
{
    /// <summary>
    /// Provides a user interface for browsing the dynamic properties of an object.
    /// </summary>
    internal sealed class DynamicPropertyGrid : PropertyGrid
    {
        /// <summary>
        /// Gets or sets the collection of selected property values.
        /// </summary>
        public static string[] SelectedPropertyValues { get; private set; }

        /// <summary>
        /// The dynamic property collection.
        /// </summary>
        private readonly DynamicPropertyCollection _dynamicPropertyCollection = new();

        /// <summary>
        /// Occurs when a dynamic property value changes.
        /// </summary>
        public event EventHandler<DynamicPropertyEventArgs> DynamicPropertyValueChanged;

        /// <summary>
        /// Initializes a new instance of the <see cref="DynamicPropertyGrid">DynamicPropertyGrid</see> class.
        /// </summary>
        public DynamicPropertyGrid()
        {
            BackColor = Color.LightYellow;

            SelectedObject = _dynamicPropertyCollection;

            ActiveControl.Controls[1].GotFocus += DynamicPropertyGrid_GotFocus;
            SelectedGridItemChanged += DynamicPropertyGrid_SelectedGridItemChanged;
            PropertyValueChanged += DynamicPropertyGrid_PropertyValueChanged;
        }

        #region Overrides of PropertyGrid

        /// <summary>
        /// Forces the control to invalidate its client area and immediately redraw itself and any child controls.
        /// </summary>
        public override void Refresh()
        {
            BackColor = _dynamicPropertyCollection.Cast<DynamicProperty>().Any(x => x.Values.Length > 1 && (string)x.Value == x.Values[0])
                ? Color.LightYellow
                : DefaultBackColor;

            base.Refresh();
        }

        #endregion

        /// <summary>
        /// Adds a new dynamic property.
        /// </summary>
        /// <param name="name">The property name.</param>
        /// <param name="category">The property category.</param>
        /// <param name="description">The property description.</param>
        /// <param name="values">The collection of property values.</param>
        /// <param name="selectedValueIndex">The selected property value index.</param>
        public void AddProperty(string name, string category, string description, string[] values, int selectedValueIndex)
        {
            _dynamicPropertyCollection.Add(new DynamicProperty
            {
                Name = name,
                Value = values[selectedValueIndex],
                Type = typeof(DynamicPropertyType),
                ReadOnly = false,
                Visible = true,
                Category = category,
                Description = description,
                Values = values
            });
        }

        /// <summary>
        /// Updates a new collection of property values.
        /// </summary>
        /// <param name="index">The dynamic property index.</param>
        /// <param name="newValues">The collection of ne property values.</param>
        public void UpdatePropertyValues(int index, string[] newValues)
        {
            _dynamicPropertyCollection[index].Values = newValues;
        }

        /// <summary>
        /// Updates a new property value.
        /// </summary>
        /// <param name="index">The dynamic property index.</param>
        /// <param name="valueIndex">The dynamic property value index.</param>
        public void UpdatePropertyValue(int index, int valueIndex)
        {
            var dynamicProperty = _dynamicPropertyCollection[index];

            if ((string)dynamicProperty.Value == dynamicProperty.Values[valueIndex])
            {
                return;
            }

            dynamicProperty.Value = dynamicProperty.Values[valueIndex];

            Refresh();
        }

        /// <summary>
        /// Event handler for the <see cref="DynamicPropertyGrid">DynamicPropertyGrid</see> property value changed.
        /// </summary>
        /// <param name="sender">Object sender.</param>
        /// <param name="e">Event arguments.</param>
        private void DynamicPropertyGrid_PropertyValueChanged(object sender, PropertyValueChangedEventArgs e)
        {
            string value = (string)SelectedGridItem.Value;

            if ((string)e.OldValue == value)
            {
                return;
            }

            string name = SelectedGridItem.Label;

            for (int i = 0; i < _dynamicPropertyCollection.Count; i++)
            {
                var property = _dynamicPropertyCollection[i];

                if (property.Name != name) continue;

                for (int j = 0; j < property.Values.Length; j++)
                {
                    if (property.Values[j] != value) continue;

                    DynamicPropertyValueChanged?.Invoke(this, new DynamicPropertyEventArgs
                    {
                        PropertyIndex = i,
                        NewValueIndex = j
                    });

                    return;
                }
            }
        }

        /// <summary>
        /// Event handler for the <see cref="DynamicPropertyGrid">DynamicPropertyGrid</see> selected GridItem changed.
        /// </summary>
        /// <param name="sender">Object sender.</param>
        /// <param name="e">Event arguments.</param>
        private void DynamicPropertyGrid_SelectedGridItemChanged(object sender, SelectedGridItemChangedEventArgs e)
        {
            var item = e.NewSelection;
            if (item.GridItemType.ToString() == "Property")
            {
                SelectedPropertyValues = GetProperty(item.Label).Values;
            }
        }

        /// <summary>
        /// Event handler for the <see cref="DynamicPropertyGrid">DynamicPropertyGrid</see> got focus.
        /// </summary>
        /// <param name="sender">Object sender.</param>
        /// <param name="e">Event arguments.</param>
        private void DynamicPropertyGrid_GotFocus(object sender, EventArgs e)
        {
            var property = GetProperty(SelectedGridItem.Label);

            if (property is null) return;

            SelectedPropertyValues = property.Values;

            Refresh();
        }

        /// <summary>
        /// Gets a dynamic property.
        /// </summary>
        /// <param name="name">The property name.</param>
        /// <returns>A dynamic property.</returns>
        private DynamicProperty GetProperty(string name)
        {
            for (int i = 0; i < _dynamicPropertyCollection.Count; i++)
            {
                var property = _dynamicPropertyCollection[i];

                if (property.Name == name)
                {
                    return property;
                }
            }

            return null;
        }
    }
}
