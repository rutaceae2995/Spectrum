using System.Windows;
using System.Windows.Controls;
using Spectrum.Mvvm;

namespace Spectrum.UI.Behavior
{
    /// <summary>
    /// This behavior supplies that enables command binding by double click event on datagrid row.
    /// </summary>
    public sealed class DataGridRowDoubleClickBehavior
    {
        /// <summary>
        /// DoubleClickCommand dependency property.
        /// </summary>
        public static DependencyProperty DoubleClickCommandProperty = DependencyProperty.RegisterAttached(
            "DoubleClickCommand",
            typeof(ICommandBase),
            typeof(DataGridRowDoubleClickBehavior),
            new PropertyMetadata(OnDoubleClickCommandPropertyChanged));

        /// <summary>
        /// Sets the value of DoubleClickCommand property.
        /// </summary>
        /// <param name="element">The target element.</param>
        /// <param name="value">The command value.</param>
        public static void SetDoubleClickCommand(UIElement element, ICommandBase value)
        {
            element.SetValue(DoubleClickCommandProperty, value);
        }

        /// <summary>
        /// Gets the value of DoubleClickCommand property.
        /// </summary>
        /// <param name="element">The target element.</param>
        /// <returns>The command value.</returns>
        public static ICommandBase GetDoubleClickCommand(UIElement element)
        {
            return element.GetValue(DoubleClickCommandProperty) as ICommandBase;
        }

        /// <summary>
        /// Occurs after DoubleClickCommand property changed.
        /// </summary>
        /// <param name="d">The target object.</param>
        /// <param name="e">Property changed event argument.</param>
        private static void OnDoubleClickCommandPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var dataGridRow = d as DataGridRow;
            if (dataGridRow == null)
            {
                return;
            }

            if (e.NewValue == null)
            {
                // remove event handler.
                dataGridRow.RemoveHandler(Control.MouseDoubleClickEvent, new RoutedEventHandler(OnDataGridRowMouseDoubleClick));
            }
            else
            {
                // add event handler.
                dataGridRow.AddHandler(Control.MouseDoubleClickEvent, new RoutedEventHandler(OnDataGridRowMouseDoubleClick));
            }
        }

        /// <summary>
        /// Occurs after double click on DataGridRow.
        /// </summary>
        /// <param name="sender">The event sender object.</param>
        /// <param name="e">The event argument.</param>
        private static void OnDataGridRowMouseDoubleClick(object sender, RoutedEventArgs e)
        {
            var dataGridRow = sender as DataGridRow;
            if (dataGridRow == null)
            {
                return;
            }

            var command = GetDoubleClickCommand(dataGridRow);
            if (command.CanExecute(dataGridRow.Item))
            {
                command.Execute(dataGridRow.Item);
            }
        }
    }
}