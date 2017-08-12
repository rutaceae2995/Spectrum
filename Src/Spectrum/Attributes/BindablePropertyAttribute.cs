using System;

namespace Spectrum.Attributes
{
    /// <summary>
    /// By specifying the property change notification property,
    /// RaisePropertyChanged will be called automatically when changing the property.
    /// * Target binding property can be set to protected or public.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class BindablePropertyAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the BindablePropertyAttribute class.
        /// </summary>
        /// <param name="aggregateWaitTimeSecond">The wait time (second) to aggregate 'RaisePropertyChanged' execution.</param>
        /// <param name="propertyChangedAction">The method name to call when property changed.</param>
        /// <param name="proptetyName">Relevant property names.</param>
        public BindablePropertyAttribute(long aggregateWaitTimeSecond = 0, string propertyChangedAction = null, params string[] proptetyName)
        {
            this.RelevantProperties = proptetyName;
            this.PropertyChangedAction = propertyChangedAction;
            this.AggregateWaitTimeSecond = aggregateWaitTimeSecond;
        }

        /// <summary>
        /// Gets or sets the relevant property names.
        /// </summary>
        public string[] RelevantProperties
        {
            get;
            set;
        }

        /// <summary>
        /// Gets of sets the method name to call when property changed.
        /// </summary>
        public string PropertyChangedAction
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the wait time (second) to aggregate 'RaisePropertyChanged' execution.
        /// </summary>
        public long AggregateWaitTimeSecond
        {
            get;
            set;
        }
    }
}