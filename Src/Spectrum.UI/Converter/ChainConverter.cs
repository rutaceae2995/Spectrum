using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Windows.Data;
using System.Windows.Markup;

namespace Spectrum.UI.Converter
{
    /// <summary>
    /// This class support supplies the converter that aggregate multiple IValueConverter.
    /// </summary>
    [ContentProperty(nameof(Converters))]
    public sealed class ChainConverter : IValueConverter
    {
        /// <summary>
        /// Gets the converters.
        /// </summary>
        public Collection<IValueConverter> Converters
        {
            get;
        } = new Collection<IValueConverter>();

        /// <summary>
        /// Converts a value by the converters.
        /// </summary>
        /// <param name="value">The target value.</param>
        /// <param name="targetType">The target type.</param>
        /// <param name="parameter">The parameter.</param>
        /// <param name="culture">The culture.</param>
        /// <returns>A converted value.</returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return this.Converters.Aggregate(value, (s, e) => e.Convert(s, targetType, parameter, culture));
        }

        /// <summary>
        /// Converts the specified value back.
        /// </summary>
        /// <param name="value">The target value.</param>
        /// <param name="targetType">The target type.</param>
        /// <param name="parameter">The target parameter.</param>
        /// <param name="culture">The target culture.</param>
        /// <returns>A converted value.</returns>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return this.Converters.Reverse().Aggregate(value, (s, e) => e.Convert(s, targetType, parameter, culture));
        }
    }
}