using System;
using System.Windows;
using Spectrum.InteractionMessenger;

namespace Spectrum.UI.Messenger
{
    /// <summary>
    /// Message parameter of the ShowDialogAction.
    /// </summary>
    public sealed class ShowDialogMessage : MessageParameter
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="showDialogFunc">Function to show a message dialog.</param>
        public ShowDialogMessage(Func<Window, MessageBoxResult> showDialogFunc)
        {
            if (showDialogFunc == null)
            {
                throw new ArgumentNullException(nameof(showDialogFunc));
            }

            this.ShowMessageBoxFunc = showDialogFunc;
        }

        /// <summary>
        /// Gets the function to show a message dialog.
        /// </summary>
        public Func<Window, MessageBoxResult> ShowMessageBoxFunc
        {
            get;
        }

        /// <summary>
        /// Gets the MessageBoxResult.
        /// </summary>
        public MessageBoxResult Result
        {
            get;
            internal set;
        }
    }
}