using System;
using System.ComponentModel;
using System.Windows;

namespace Spectrum.WeakEventListener
{
    /// <summary>
    /// Implements a weak event listener for PropertyChanged event.
    /// </summary>
    public sealed class PropertyChangedWeakEventListener : IWeakEventListener
    {
        /// <summary>
        /// Action after listening the PropertyChanged event.
        /// arg1: The original property name
        /// arg2: The property name by the PropertyChanged event.
        /// * arg1,2 used only NotifiableObservableCollection.
        /// </summary>
        private readonly Action<string, string> raisePropertyChangedAction;

        /// <summary>
        /// The original property name.
        /// </summary>
        private readonly string sourcePropertyName;

        /// <summary>
        /// Initializes a new instance for the PropertyChangedWeakEventListener class.
        /// </summary>
        /// <param name="sourcePropertyName">The original property name.</param>
        /// <param name="raisePropertyChangedAction">The action after listenning the PropertyChanged event.</param>
        public PropertyChangedWeakEventListener(string sourcePropertyName, Action<string, string> raisePropertyChangedAction)
        {
            this.sourcePropertyName = sourcePropertyName;
            this.raisePropertyChangedAction = raisePropertyChangedAction;
        }

        /// <summary>
        /// Handle events from the centralized event table.
        /// </summary>
        /// <param name="managerType">The manager type.</param>
        /// <param name="sender">The event sender object.</param>
        /// <param name="e">The event argument.</param>
        /// <returns>
        /// True if the listener handled the event.
        /// It is an error to register a listener for an event that it does not handle.
        /// </returns>
        public bool ReceiveWeakEvent(Type managerType, object sender, EventArgs e)
        {
            if (typeof(PropertyChangedEventManager) != managerType)
            {
                return false;
            }

            var eventArgs = e as PropertyChangedEventArgs;
            if (eventArgs == null)
            {
                return false;
            }

            this.raisePropertyChangedAction(this.sourcePropertyName, eventArgs.PropertyName);
            return true;
        }
    }
}