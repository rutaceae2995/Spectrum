using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using Spectrum.Extension;

namespace Spectrum.UI.Converter
{
    /// <summary>
    /// Converts a Enum value to a Description attribute list.
    /// </summary>
    public sealed class EnumToDescriptionListConverter : IValueConverter
    {
        /// <summary>
        /// Converts a Enum value to a Description attribute list.
        /// </summary>
        /// <param name="value">The target Enum value.</param>
        /// <param name="targetType">The target type (not used).</param>
        /// <param name="parameter">The parameter (not used).</param>
        /// <param name="culture">The culture (not used).</param>
        /// <returns>The description values.</returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var type = value as Type;
            if (type == null)
            {
                return DependencyProperty.UnsetValue;
            }

            var result = new List<string>();
            foreach (var v in Enum.GetValues(type))
            {
                var descriptionAttribute = (v as Enum)?.GetAttributeOfType<DescriptionAttribute>();
                result.Add(descriptionAttribute == null ? string.Empty : descriptionAttribute.Description);
            }

            return result;
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