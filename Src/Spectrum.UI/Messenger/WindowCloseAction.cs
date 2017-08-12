using System.Windows;
using Spectrum.InteractionMessenger;
using Spectrum.UI.Extension;

namespace Spectrum.UI.Messenger
{
    /// <summary>
    /// Trigger action to close a window.
    /// </summary>
    public sealed class WindowCloseAction : MessageTriggerAction<WindowCloseMessage>
    {
        /// <summary>
        /// Executes the action.
        /// </summary>
        /// <param name="parameter">Parameter (unused) </param>
        protected override void InvokeCore(WindowCloseMessage parameter)
        {
            Window parentWindow = null;
            var element = this.AssociatedObject as FrameworkElement;
            if (element != null)
            {
                parentWindow = element.FindParentWindow();
            }

            if (parentWindow != null)
            {
                parentWindow.DialogResult = parameter.DialogResult;
            }
        }
    }
}