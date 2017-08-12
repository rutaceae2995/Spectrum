using System;
using System.Collections;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace Spectrum.UI.AttachedProperty
{
    /// <summary>
    /// This class supplies the attached property that can be used for multi selection items property.
    /// </summary>
    public sealed class MultiSelection
    {
        /// <summary>
        /// SelectedItems property of a MultiSelector for data binding.
        /// </summary>
        public static readonly DependencyProperty SelectedItemsProperty =
            DependencyProperty.RegisterAttached(
                "SelectedItems",
                typeof(IList),
                typeof(MultiSelection),
                new FrameworkPropertyMetadata(
                    null,
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                    OnSelectedItemsChanged));

        /// <summary>
        /// SelectionChanged event handler.
        /// </summary>
       private static readonly DependencyProperty SelectionChangedHandlerProperty =
            DependencyProperty.RegisterAttached(
                "SelectionChangedHandler",
                typeof(SelectionChangedEventHandler),
                typeof(MultiSelection),
                new UIPropertyMetadata(null));

        /// <summary>
        /// Gets the selected items.
        /// </summary>
        /// <param name="obj">The target object.</param>
        /// <returns>The selected items.</returns>
        public static IList GetSelectedItems(DependencyObject obj)
        {
            return obj.GetValue(SelectedItemsProperty) as IList;
        }

        /// <summary>
        /// Sets the selected items.
        /// </summary>
        /// <param name="obj">The target object.</param>
        /// <param name="value">The selected items.</param>
        public static void SetSelectedItems(DependencyObject obj, IList value)
        {
            obj.SetValue(SelectedItemsProperty, value);
        }

        /// <summary>
        /// Get the SelectionChanged event handler.
        /// </summary>
        /// <param name="obj">The target object.</param>
        /// <returns>The SelectionChanged event handler.</returns>
        private static SelectionChangedEventHandler GetSelectionChangedHandler(DependencyObject obj)
        {
            return (SelectionChangedEventHandler)obj.GetValue(SelectionChangedHandlerProperty);
        }

        /// <summary>
        /// Set a SelectionChanged event handler.
        /// </summary>
        /// <param name="obj">The target object.</param>
        /// <param name="value">The SelectionChanged event handler.</param>
        private static void SetSelectionChangedHandler(DependencyObject obj, SelectionChangedEventHandler value)
        {
            obj.SetValue(SelectionChangedHandlerProperty, value);
        }

        /// <summary>
        /// Occurs after the value of the SelectedItems property.
        /// </summary>
        /// <param name="d">The target object.</param>
        /// <param name="e">The dependency property changed event argument.</param>
        private static void OnSelectedItemsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (GetSelectionChangedHandler(d) != null)
            {
                return;
            }

            var multiselector = d as MultiSelector;
            if (multiselector != null)
            {
                if (e.NewValue != null)
                {
                    foreach (var selected in GetSelectedItems(d))
                    {
                        multiselector.SelectedItems.Add(selected);
                    }

                    SetSelectionChangedHandler(d, OnSelectionChanged);
                    multiselector.SelectionChanged += GetSelectionChangedHandler(d);
                }
                else
                {
                    foreach (var selected in GetSelectedItems(d))
                    {
                        multiselector.SelectedItems.Remove(selected);
                    }

                    SetSelectionChangedHandler(d, null);
                    multiselector.SelectionChanged -= GetSelectionChangedHandler(d);
                }

                return;
            }

            var listbox = d as ListBox;
            if (listbox != null)
            {
                if (e.NewValue != null)
                {
                    SetSelectionChangedHandler(d, OnSelectionChanged);
                    listbox.SelectionChanged += GetSelectionChangedHandler(d);
                }
                else
                {
                    SetSelectionChangedHandler(d, null);
                    listbox.SelectionChanged -= GetSelectionChangedHandler(d);
                }
            }
        }

        /// <summary>
        /// Occurs after the selection changed.
        /// </summary>
        /// <param name="sender">The sender object.</param>
        /// <param name="e">The SelectionChanged event argument.</param>
        private static void OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var dp = (DependencyObject)sender;
            IEnumerable selectedItems = null;
            var multiSelector = sender as MultiSelector;
            if (multiSelector != null)
            {
                selectedItems = multiSelector.SelectedItems;
            }
            else
            {
                var listBox = sender as ListBox;
                if (listBox != null)
                {
                    selectedItems = listBox.SelectedItems;
                }
            }

            if (selectedItems == null)
            {
                throw new NotSupportedException();
            }

            var selectedItemsModel = GetSelectedItems(dp);
            selectedItemsModel.Clear();

            foreach (var item in selectedItems)
            {
                selectedItemsModel.Add(item);
            }

            SetSelectedItems(dp, selectedItemsModel);
        }
    }
}