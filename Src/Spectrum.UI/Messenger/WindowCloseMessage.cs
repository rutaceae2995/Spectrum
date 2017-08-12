using Spectrum.InteractionMessenger;

namespace Spectrum.UI.Messenger
{
    /// <summary>
    /// Message parameter of the WindowCloseAction.
    /// </summary>
    public sealed class WindowCloseMessage : MessageParameter
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="dialogResult">The return value of the window.</param>
        public WindowCloseMessage(bool? dialogResult)
        {
            this.DialogResult = dialogResult;
        }

        /// <summary>
        /// Gets the return value.
        /// </summary>
        public bool? DialogResult
        {
            get;
            private set;
        }
    }
}