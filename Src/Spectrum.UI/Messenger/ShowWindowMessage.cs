using System;
using System.Windows;
using Spectrum.InteractionMessenger;

namespace Spectrum.UI.Messenger
{
    /// <summary>
    /// Message parameter of the ShowWindowAction.
    /// </summary>
    public sealed class ShowWindowMessage : MessageParameter
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="showDialogFunc">Function to show the modal window.</param>
        public ShowWindowMessage(Func<Window> showDialogFunc)
        {
            if (showDialogFunc == null)
            {
                throw new ArgumentNullException(nameof(showDialogFunc));
            }

            this.ShowWindowFunc = showDialogFunc;
        }

        /// <summary>
        /// Gets the function to show the modal window.
        /// </summary>
        public Func<Window> ShowWindowFunc
        {
            get;
        }
    }
}