using System;
using System.Globalization;
using System.Windows.Data;

namespace Spectrum.UI.Converter
{
    /// <summary>
    /// Converts a null to a boolean value (=true).
    /// </summary>
    public sealed class NullToBooleanConverter : IValueConverter
    {
        /// <summary>
        /// Converts a null to a boolean value (=true).
        /// </summary>
        /// <param name="value">The target value.</param>
        /// <param name="targetType">The target type (not used).</param>
        /// <param name="parameter">The parameter (not used).</param>
        /// <param name="culture">The culture (not used).</param>
        /// <returns>A boolean value.</returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value == null;
        }

        /// <summary>
        /// Not supported.
        /// </summary>
        /// <param name="value">The target value (not used).</param>
        /// <param name="targetType">The target type (not used).</param>
        /// <param name="parameter">The parameter (not used).</param>
        /// <param name="culture">The culture (not used).</param>
        /// <returns>Nothing</returns>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}