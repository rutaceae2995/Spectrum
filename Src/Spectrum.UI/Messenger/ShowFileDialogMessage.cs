using System;
using Microsoft.Win32;
using Spectrum.InteractionMessenger;

namespace Spectrum.UI.Messenger
{
    /// <summary>
    /// Message parameter of the ShowFileDialogAction.
    /// </summary>
    public sealed class ShowFileDialogMessage : MessageParameter
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="showDialogFunc">Function to show a FileDialog.</param>
        /// <param name="resultAction">Action after the dialog closed. Item1: return value of the ShowDialog, Item2: File paths</param>
        public ShowFileDialogMessage(Func<FileDialog> showDialogFunc, Action<bool?, string[]> resultAction)
        {
            if (showDialogFunc == null)
            {
                throw new ArgumentNullException(nameof(showDialogFunc));
            }

            this.ShowDialogFunc = showDialogFunc;
            this.ResultAction = resultAction;
        }

        /// <summary>
        /// Gets the function to show a FileDialog.
        /// </summary>
        public Func<FileDialog> ShowDialogFunc
        {
            get;
        }

        /// <summary>
        /// Gets the action after the dialog closed.
        /// </summary>
        public Action<bool?, string[]> ResultAction
        {
            get;
        }
    }
}