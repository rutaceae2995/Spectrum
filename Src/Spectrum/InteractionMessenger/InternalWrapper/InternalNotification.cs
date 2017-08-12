using System;
using Microsoft.Practices.Prism.Interactivity.InteractionRequest;

namespace Spectrum.InteractionMessenger.InternalWrapper
{
    /// <summary>
    /// This is a wrapper class of the Prism Notification class.
    /// </summary>
    internal sealed class InternalNotification : Notification
    {
        /// <summary>
        /// Initializes a new instance of the InternalNotification class.
        /// </summary>
        /// <param name="parentParameter">The parent message parameter instance.</param>
        internal InternalNotification(MessageParameter parentParameter)
        {
            if (parentParameter == null)
            {
                throw new ArgumentNullException(nameof(parentParameter));
            }

            this.MessageParameter = parentParameter;
        }

        /// <summary>
        /// Gets the message parameter.
        /// </summary>
        internal MessageParameter MessageParameter
        {
            get;
        }
    }
}