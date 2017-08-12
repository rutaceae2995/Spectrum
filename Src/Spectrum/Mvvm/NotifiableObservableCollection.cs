using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using Spectrum.WeakEventListener;

namespace Spectrum.Mvvm
{
    /// <summary>
    /// This class supplies that enables to notify collection changes when the property of the item changes.
    /// </summary>
    /// <typeparam name="T">The type of item.</typeparam>
    public class NotifiableObservableCollection<T> : ObservableCollection<T>
        where T : INotifyPropertyChanged
    {
        /// <summary>
        /// PropertyChanged weak event listener.
        /// </summary>
        private readonly PropertyChangedWeakEventListener propertyChangedListener;

        /// <summary>
        /// Initializes a new instance of the NotifiableObservableCollection class.
        /// </summary>
        /// <param name="collection">Item collection.</param>
        public NotifiableObservableCollection(IEnumerable<T> collection)
            : base(collection)
        {
            this.propertyChangedListener = new PropertyChangedWeakEventListener(string.Empty, this.OnItemPropertyChanged);
            if (this.Items != null)
            {
                foreach (var i in this.Items)
                {
                    PropertyChangedEventManager.AddListener(i, this.propertyChangedListener, string.Empty);
                }
            }
        }

        /// <summary>
        /// Initializes a new instance of the NotifiableObservableCollection class.
        /// </summary>
        public NotifiableObservableCollection()
        {
            this.propertyChangedListener = new PropertyChangedWeakEventListener(string.Empty, this.OnItemPropertyChanged);
        }

        /// <summary>
        /// Occurs after collection changed.
        /// </summary>
        /// <param name="e">The collection changed event argument.</param>
        protected override void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
        {
            base.OnCollectionChanged(e);

            if (e.OldItems != null)
            {
                foreach (T i in e.OldItems)
                {
                    PropertyChangedEventManager.RemoveListener(i, this.propertyChangedListener, string.Empty);
                }
            }

            if (e.NewItems != null)
            {
                foreach (T i in e.NewItems)
                {
                    PropertyChangedEventManager.AddListener(i, this.propertyChangedListener, string.Empty);
                }
            }
        }

        /// <summary>
        /// Occurs after item property changed.
        /// </summary>
        /// <param name="empty">Always string.Empty.</param>
        /// <param name="propertyName">The changed propety name.</param>
        private void OnItemPropertyChanged(string empty, string propertyName)
        {
            this.OnPropertyChanged(new PropertyChangedEventArgs(propertyName));
        }
    }
}