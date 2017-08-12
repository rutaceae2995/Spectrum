using System;

namespace Spectrum.Attributes
{
    /// <summary>
    /// By specifying the property change notification property,
    /// RaisePropertyChanged will be called automatically when changing the property.
    /// * Target binding property can be set to protected or public.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class BindableCollectionPropertyAttribute : BindablePropertyAttribute
    {
        /// <summary>
        /// Initializes a new instance of the BindableCollectionPropertyAttribute class.
        /// </summary>
        public BindableCollectionPropertyAttribute()
            : this(null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the BindableCollectionPropertyAttribute class.
        /// </summary>
        /// <param name="propertyChangedAction">The method name to call when property changed.</param>
        /// <param name="proptetyName">Relevant property name.</param>
        public BindableCollectionPropertyAttribute(string propertyChangedAction, params string[] proptetyName)
            : base(0, propertyChangedAction, proptetyName)
        {
        }
    }
}