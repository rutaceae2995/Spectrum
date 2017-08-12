using System;
using System.Reflection;

namespace Spectrum.Extension
{
    /// <summary>
    /// Extension method definition class on Type.
    /// </summary>
    public static class TypeExtension
    {
        /// <summary>
        /// Returns property information.
        /// </summary>
        /// <param name="instance">The target instance.</param>
        /// <param name="propertyName">The property name.</param>
        /// <param name="flags">Binding flags.</param>
        /// <returns>Property information.</returns>
        public static PropertyInfo GetProperty<T>(this T instance, string propertyName, BindingFlags flags = BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance)
            where T : class
        {
            return instance?.GetType().GetProperty(propertyName, flags);
        }

        /// <summary>
        /// Returns property information.
        /// </summary>
        /// <param name="type">The target type.</param>
        /// <param name="propertyName">The target property name.</param>
        /// <param name="flags">Binding flags.</param>
        /// <returns>Property information.</returns>
        public static PropertyInfo GetProperty(this Type type, string propertyName, BindingFlags flags = BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance)
        {
            return type.GetProperty(propertyName, flags);
        }
    }
}