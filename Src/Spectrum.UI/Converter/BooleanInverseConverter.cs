using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Spectrum.UI.Converter
{
    /// <summary>
    /// Inverts a boolean value (true - false).
    /// </summary>
    public sealed class BooleanInverseConverter : IValueConverter
    {
        /// <summary>
        /// Converts a boolean value.
        /// </summary>
        /// <param name="value">The boolean value.</param>
        /// <param name="targetType">The target type (not used).</param>
        /// <param name="parameter">The parameter (not used).</param>
        /// <param name="culture">The culture (not used).</param>
        /// <returns>Negated boolean value.</returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                return DependencyProperty.UnsetValue;
            }

            return !(bool)value;
        }

        /// <summary>
        /// Converts a boolean value.
        /// </summary>
        /// <param name="value">The boolean value.</param>
        /// <param name="targetType">The target type (not used).</param>
        /// <param name="parameter">The parameter (not used).</param>
        /// <param name="culture">The culture (not used).</param>
        /// <returns>Negated boolean value.</returns>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                return DependencyProperty.UnsetValue;
            }

            return !(bool)value;
        }
    }
}