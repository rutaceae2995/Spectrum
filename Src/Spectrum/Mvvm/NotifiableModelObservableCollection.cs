using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Spectrum.Mvvm
{
    /// <summary>
    /// Notifiable ObservableCollection model class.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class NotifiableModelObservableCollection<T> : NotifiableObservableCollection<NotifiableModel<T>>
        where T : class
    {
        /// <summary>
        /// Initializes a new instance of the NotifiableModelObservableCollection class.
        /// </summary>
        /// <param name="collection">The items of collection.</param>
        public NotifiableModelObservableCollection(IEnumerable<NotifiableModel<T>> collection)
            : base(collection)
        {
        }

        /// <summary>
        /// Initializes a new instance of the NotifiableModelObservableCollection class.
        /// </summary>
        public NotifiableModelObservableCollection()
        {
        }

        /// <summary>
        /// Remove model items.
        /// </summary>
        /// <param name="removeTarget">Remove items that match this Predicate.</param>
        public void RemoveModelItems(Predicate<T> removeTarget)
        {
            var removes = this.Items.Where(e => removeTarget(e.GetModel())).ToArray();
            foreach (var item in removes)
            {
                this.Remove(item);
            }
        }

        /// <summary>
        /// Gets the property name of item.
        /// </summary>
        /// <typeparam name="TProperty">the property type of item.</typeparam>
        /// <param name="propertyExpression">The target property expression.</param>
        /// <returns>Property name.</returns>
        public string ItemNameOf<TProperty>(Expression<Func<T, TProperty>> propertyExpression)
        {
            var memberExpression = propertyExpression.Body as MemberExpression;
            if (memberExpression == null)
            {
                throw new ArgumentException(nameof(propertyExpression));
            }

            return memberExpression.Member.Name;
        }
    }
}