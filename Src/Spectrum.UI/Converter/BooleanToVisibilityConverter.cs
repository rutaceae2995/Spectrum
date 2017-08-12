using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Spectrum.UI.Converter
{
    /// <summary>
    /// Converts a boolean value to a Visibility value.
    /// true -> Visible
    /// false -> Collapsed
    /// </summary>
    public sealed class BooleanToVisibilityConverter : IValueConverter
    {
        /// <summary>
        /// Converts a boolean value to a Visibility value.
        /// </summary>
        /// <param name="value">The boolean value.</param>
        /// <param name="targetType">The target type (not used).</param>
        /// <param name="parameter">The parameter (not used).</param>
        /// <param name="culture">The culture (not used).</param>
        /// <returns>Visibility value.</returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is bool))
            {
                return DependencyProperty.UnsetValue;
            }

            return (bool)value ? Visibility.Visible : Visibility.Collapsed;
        }

        /// <summary>
        /// Converts a Visibility value to a boolean value.
        /// </summary>
        /// <param name="value">The Visibility value.</param>
        /// <param name="targetType">The target type (not used).</param>
        /// <param name="parameter">The parameter (not used).</param>
        /// <param name="culture">The culture (not used).</param>
        /// <returns>Boolean value.</returns>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is Visibility))
            {
                return DependencyProperty.UnsetValue;
            }

            return (Visibility)value == Visibility.Visible;
        }
    }
}