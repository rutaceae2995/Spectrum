using System;

namespace Spectrum.Extension
{
    /// <summary>
    /// Extension method definition class on Enum type.
    /// </summary>
    public static class EnumExtension
    {
        /// <summary>
        /// Returns an attribute of enum.
        /// </summary>
        /// <typeparam name="T">The attribute type.</typeparam>
        /// <param name="enumVal">A target enum value.</param>
        /// <returns>The attribute.</returns>
        public static T GetAttributeOfType<T>(this Enum enumVal) where T : Attribute
        {
            var type = enumVal.GetType();
            var memInfo = type.GetMember(enumVal.ToString());
            var attributes = memInfo[0].GetCustomAttributes(typeof(T), false);
            return attributes.Length > 0 ? (T)attributes[0] : null;
        }
    }
}