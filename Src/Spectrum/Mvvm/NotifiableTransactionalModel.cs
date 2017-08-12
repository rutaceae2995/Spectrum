using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Linq.Expressions;
using Spectrum.Extension;

namespace Spectrum.Mvvm
{
    /// <summary>
    /// This class is a DynamicObject class that enables to notify property change from model (non INotifyPropertyChanged) class.
    /// The properties of this class are separate from the property values of the target model instance.
    /// Uses the SaveToModel or Rollback method to change the property value of the target model instance.
    /// </summary>
    /// <typeparam name="T">Type of model class.</typeparam>
    public class NotifiableTransactionalModel<T> : NotifiableModelBase<T>
        where T : class
    {
        /// <summary>
        /// Temporary property values of the target model.
        /// </summary>
        private readonly Dictionary<string, object> modelProperties = new Dictionary<string, object>();

        /// <summary>
        /// Initializes a new instance of the NotifiableTransactionalModel class.
        /// </summary>
        /// <param name="model">Model class instance.</param>
        public NotifiableTransactionalModel(T model)
            : base(model)
        {
        }

        /// <summary>
        /// Attempts to get the value of the property.
        /// </summary>
        /// <param name="binder">GetMemberBinder instance.</param>
        /// <param name="result">The result.</param>
        /// <returns>Success or failure.</returns>
        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            var propertyName = binder.Name;
            if (this.modelProperties.ContainsKey(propertyName))
            {
                result = this.modelProperties[propertyName];
                return true;
            }

            var property = this.InternalModel.GetProperty(propertyName);
            if (property == null || !property.CanRead)
            {
                result = null;
                return false;
            }

            result = property.GetValue(this.InternalModel, null);
            return true;
        }

        /// <summary>
        /// Attempts to set the value of the property.
        /// The model property is not changed.
        /// </summary>
        /// <param name="propertyName">The target property name.</param>
        /// <param name="value">the value.</param>
        /// <returns>Changed or not.</returns>
        protected override bool TrySetMember(string propertyName, object value)
        {
            if (!this.modelProperties.ContainsKey(propertyName))
            {
                this.modelProperties.Add(propertyName, value);
                this.RaisePropertyChanged(propertyName);
                return true;
            }

            if (this.modelProperties[propertyName] != value)
            {
                this.modelProperties[propertyName] = value;
                this.RaisePropertyChanged(propertyName);
                return true;
            }

            return false;
        }

        /// <summary>
        /// Saves property values to the model and gets the model.
        /// </summary>
        /// <returns>The model.</returns>
        public override T GetModel()
        {
            this.SaveToModel();
            return this.InternalModel;
        }

        /// <summary>
        /// Saves property values to the model.
        /// </summary>
        public void SaveToModel()
        {
            foreach (var kvp in this.modelProperties)
            {
                var property = this.InternalModel.GetProperty(kvp.Key);
                if (property == null || !property.CanWrite)
                {
                    continue;
                }

                property.SetValue(this.InternalModel, kvp.Value, null);
            }
        }

        /// <summary>
        /// Rollbacks property values from the last saved property values.
        /// </summary>
        public void Rollback()
        {
            var propertyNames = this.modelProperties.Keys.ToArray();
            this.modelProperties.Clear();
            foreach (var name in propertyNames)
            {
                this.RaisePropertyChanged(name);
            }
        }

        /// <summary>
        /// Sets the value of the property.
        /// </summary>
        /// <typeparam name="TProperty">Type of the property.</typeparam>
        /// <param name="propertyExpression">Property access expression.</param>
        /// <param name="value">The value.</param>
        /// <returns>Changed or not.</returns>
        public bool SetProperty<TProperty>(Expression<Func<T, TProperty>> propertyExpression, TProperty value)
        {
            var memberExpression = propertyExpression.Body as MemberExpression;
            if (memberExpression == null)
            {
                throw new ArgumentException(nameof(propertyExpression));
            }

            var propertyName = memberExpression.Member.Name;
            return this.TrySetMember(propertyName, value);
        }
    }
}