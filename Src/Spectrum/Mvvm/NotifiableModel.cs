using System;
using System.Dynamic;
using System.Linq.Expressions;
using Spectrum.Extension;

namespace Spectrum.Mvvm
{
    /// <summary>
    /// This class supplies that enables notification of properties of model class.
    /// </summary>
    /// <typeparam name="TModel">The target model class type.</typeparam>
    public class NotifiableModel<TModel> : NotifiableModelBase<TModel>
        where TModel : class
    {
        /// <summary>
        /// Initializes a new instance of the NotifiableModel class.
        /// </summary>
        /// <param name="model">The model class instance.</param>
        public NotifiableModel(TModel model)
            : base(model)
        {
        }

        /// <summary>
        /// Attempts to get the property value.
        /// </summary>
        /// <param name="binder">The member binder.</param>
        /// <param name="result">The property value.</param>
        /// <returns>Success or failure.</returns>
        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            var propertyName = binder.Name;
            return this.TryGetMember(propertyName, out result);
        }

        /// <summary>
        /// Attempts to get the property value.
        /// </summary>
        /// <param name="propertyName">The target property name.</param>
        /// <param name="result">The value.</param>
        /// <returns>Success or failure.</returns>
        public bool TryGetMember<TProperty>(string propertyName, out TProperty result)
        {
            var property = this.InternalModel.GetProperty(propertyName);
            if (property == null || !property.CanRead)
            {
                result = default(TProperty);
                return false;
            }

            result = (TProperty)property.GetValue(this.InternalModel);
            return true;
        }

        /// <summary>
        /// Attempts to set the property value.
        /// </summary>
        /// <typeparam name="TProperty">The target property type.</typeparam>
        /// <param name="propertyExpression">The target property expression.</param>
        /// <param name="value">The value.</param>
        /// <returns>Success or failure.</returns>
        public bool TryGetMember<TProperty>(Expression<Func<TModel, TProperty>> propertyExpression, out TProperty value)
        {
            var memberExpression = propertyExpression.Body as MemberExpression;
            if (memberExpression == null)
            {
                throw new ArgumentException(nameof(propertyExpression));
            }

            var propertyName = memberExpression.Member.Name;
            return this.TryGetMember(propertyName, out value);
        }

        /// <summary>
        /// Attempts to set the property value.
        /// </summary>
        /// <typeparam name="TProperty">The target property type.</typeparam>
        /// <param name="propertyExpression">The target property expression.</param>
        /// <param name="value">The value.</param>
        /// <returns>Success or failure.</returns>
        public bool TrySetMember<TProperty>(Expression<Func<TModel, TProperty>> propertyExpression, TProperty value)
        {
            var memberExpression = propertyExpression.Body as MemberExpression;
            if (memberExpression == null)
            {
                throw new ArgumentException(nameof(propertyExpression));
            }

            var propertyName = memberExpression.Member.Name;
            return this.TrySetMember(propertyName, value);
        }

        /// <summary>
        /// Attempts to set the property value.
        /// </summary>
        /// <param name="propertyName">The target property name.</param>
        /// <param name="value">The value.</param>
        /// <returns>Success or failure.</returns>
        protected override bool TrySetMember(string propertyName, object value)
        {
            var property = this.InternalModel.GetProperty(propertyName);
            if (property == null || !property.CanWrite)
            {
                return false;
            }

            var current = property.GetValue(this.InternalModel, null);
            if (Equals(current, value))
            {
                return false;
            }

            property.SetValue(this.InternalModel, value, null);
            this.RaisePropertyChanged(propertyName);
            return true;
        }
    }
}