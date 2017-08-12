using System.Windows.Interactivity;
using Spectrum.Triggers.InternalWrapper;

namespace Spectrum.Triggers
{
    /// <summary>
    /// Event trigger that notificate a action from ViewModel to View.
    /// </summary>
    public sealed class MessengerActionTrigger : EventTrigger
    {
        /// <summary>
        /// InteractionTrigger instance.
        /// </summary>
        private readonly InternalInteractionTriggerAction triggerActionCore = new InternalInteractionTriggerAction();

        /// <summary>
        /// Gets the event name.
        /// </summary>
        /// <returns>The event name.</returns>
        protected override string GetEventName()
        {
            return this.triggerActionCore.GetEventNameInternal();
        }

        /// <summary>
        /// Occurs after trigger attached.
        /// </summary>
        protected override void OnAttached()
        {
            this.triggerActionCore.OnAttachedInternal();
        }

        /// <summary>
        /// Occurs after trigger detached.
        /// </summary>
        protected override void OnDetaching()
        {
            this.triggerActionCore.OnDetachingInternal();
        }
    }
}