using System;
using System.ComponentModel;
using System.Dynamic;

namespace Spectrum.Mvvm
{
    /// <summary>
    /// This class is the base class that enables notification of properties of model class.
    /// </summary>
    /// <typeparam name="TModel">The type of model class</typeparam>
    public abstract class NotifiableModelBase<TModel> : DynamicObject, INotifyPropertyChanged
        where TModel : class
    {
        /// <summary>
        /// Initializes a new instance of the NotifiableModelBase class.
        /// </summary>
        /// <param name="model">The model class instance.</param>
        protected NotifiableModelBase(TModel model)
        {
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            this.InternalModel = model;
        }

        /// <summary>
        /// PropertyChanged event handler.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Gets the model instance.
        /// </summary>
        /// <remarks>Do not directly change the model acquired while binding with the view.</remarks>
        protected TModel InternalModel
        {
            get;
        }

        /// <summary>
        /// Gets the model instance.
        /// </summary>
        /// <remarks>Do not directly change the model acquired while binding with the view.</remarks>
        /// <returns>The model instance.</returns>
        public virtual TModel GetModel()
        {
            return this.InternalModel;
        }

        /// <summary>
        /// Sets a property value to ViewModel without change the model instance.
        /// </summary>
        /// <param name="binder">Member binder.</param>
        /// <param name="value">The value.</param>
        /// <returns>Success or failure.</returns>
        public override bool TrySetMember(SetMemberBinder binder, object value)
        {
            return this.TrySetMember(binder.Name, value);
        }

        /// <summary>
        /// Sets a property value to ViewModel without change the model instance.
        /// </summary>
        /// <param name="propertyName">The property name.</param>
        /// <param name="value">The value.</param>
        /// <returns>Success or failure.</returns>
        protected abstract bool TrySetMember(string propertyName, object value);

        /// <summary>
        /// Raises the PropertyChanged event.
        /// </summary>
        /// <param name="propertyName">The target property name.</param>
        protected virtual void RaisePropertyChanged(string propertyName)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}