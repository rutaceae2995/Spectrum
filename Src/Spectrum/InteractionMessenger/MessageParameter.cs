using Microsoft.Practices.Prism.Interactivity.InteractionRequest;
using Spectrum.InteractionMessenger.InternalWrapper;

namespace Spectrum.InteractionMessenger
{
    /// <summary>
    /// The MessageParameter class for using the messender parameter.
    /// </summary>
    public abstract class MessageParameter
    {
        /// <summary>
        /// Notification instance.
        /// </summary>
        private readonly InternalNotification notificationCore;

        /// <summary>
        /// Initializes a new instance of the MessageParameter class. 
        /// </summary>
        protected MessageParameter()
        {
            this.notificationCore = new InternalNotification(this);
        }

        /// <summary>
        /// Gets or sets the content of the MessageParameter.
        /// </summary>
        public object Content
        {
            get => this.notificationCore.Content;
            set => this.notificationCore.Content = value;
        }

        /// <summary>
        /// Gets or sets the title to use for the MessageParameter.
        /// </summary>
        public string Title
        {
            get => this.notificationCore.Title;
            set => this.notificationCore.Title = value;
        }

        /// <summary>
        /// Gets the Notification instance.
        /// </summary>
        internal Notification Notification => this.notificationCore;
    }
}