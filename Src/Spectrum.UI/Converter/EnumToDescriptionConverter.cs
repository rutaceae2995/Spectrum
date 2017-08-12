using System;
using System.ComponentModel;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using Spectrum.Extension;

namespace Spectrum.UI.Converter
{
    /// <summary>
    /// Converts the Enum value to the Description attribute value.
    /// </summary>
    public sealed class EnumToDescriptionConverter : IValueConverter
    {
        /// <summary>
        /// Converts the Enum value to the Description attribute value.
        /// </summary>
        /// <param name="value">The target Enum value.</param>
        /// <param name="targetType">The target type (not used).</param>
        /// <param name="parameter">The parameter (not used).</param>
        /// <param name="culture">The culture (not used).</param>
        /// <returns>The description value.</returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var displayNameAttribute = (value as Enum)?.GetAttributeOfType<DescriptionAttribute>();
            if (displayNameAttribute == null)
            {
                return DependencyProperty.UnsetValue;
            }

            return displayNameAttribute.Description;
        }

        /// <summary>
        /// Converts the Description attribute value to the Enum value.
        /// The converted value is the first matching Enum value if exists same Desctiption attribute values.
        /// </summary>
        /// <param name="value">The target Enum value.</param>
        /// <param name="targetType">The target type (not used).</param>
        /// <param name="parameter">The parameter (not used).</param>
        /// <param name="culture">The culture (not used).</param>
        /// <returns>The Enum value.</returns>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var targetValue = value as string;
            if (string.IsNullOrEmpty(targetValue))
            {
                return DependencyProperty.UnsetValue;
            }

            foreach (var v in Enum.GetValues(targetType))
            {
                var descriptionAttribute = (v as Enum)?.GetAttributeOfType<DescriptionAttribute>();
                if (descriptionAttribute?.Description == targetValue)
                {
                    return v;
                }
            }

            return DependencyProperty.UnsetValue;
        }
    }
}