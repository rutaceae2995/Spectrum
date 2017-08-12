using Microsoft.Practices.Prism.Interactivity.InteractionRequest;

namespace Spectrum.Triggers.InternalWrapper
{
    /// <summary>
    /// Prism InteractionRequestTrigger wrapper.
    /// </summary>
    internal sealed class InternalInteractionTriggerAction : InteractionRequestTrigger
    {
        /// <summary>
        /// Gets the event name.
        /// </summary>
        /// <returns>The event name.</returns>
        internal string GetEventNameInternal()
        {
            return this.GetEventName();
        }

        /// <summary>
        /// Occurs after trigger attached.
        /// </summary>
        internal void OnAttachedInternal()
        {
            this.OnAttached();
        }

        /// <summary>
        /// Occurs after trigger detached.
        /// </summary>
        internal void OnDetachingInternal()
        {
            this.OnDetaching();
        }
    }
}