using System;
using System.Windows;
using System.Windows.Interactivity;
using Microsoft.Practices.Prism.Interactivity.InteractionRequest;
using Spectrum.InteractionMessenger.InternalWrapper;

namespace Spectrum.InteractionMessenger
{
    /// <summary>
    /// Base class of the TriggerAction.
    /// </summary>
    /// <typeparam name="T">Received message type.</typeparam>
    public abstract class MessageTriggerAction<T> : TriggerAction<DependencyObject>
        where T : MessageParameter
    {
        /// <summary>
        /// Invoke action.
        /// </summary>
        /// <param name="parameter">Parameter</param>
        protected sealed override void Invoke(object parameter)
        {
            var args = parameter as InteractionRequestedEventArgs;
            if (args == null)
            {
                throw new ArgumentException("Invalid implementation of TriggerAction.", nameof(parameter));
            }

            var notification = args.Context as InternalNotification;
            if (notification == null)
            {
                throw new ArgumentException("Invalid notification.", nameof(parameter));
            }

            var param = notification.MessageParameter as T;
            if (param == null)
            {
                // Ignore actions on other targets.
                return;
            }

            this.InvokeCore(param);
        }

        /// <summary>
        /// Invoke the target action.
        /// </summary>
        /// <param name="parameter">Parameter</param>
        protected abstract void InvokeCore(T parameter);
    }
}