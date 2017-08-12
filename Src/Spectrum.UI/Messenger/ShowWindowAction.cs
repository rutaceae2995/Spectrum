using System.Windows;
using Spectrum.InteractionMessenger;
using Spectrum.UI.Extension;

namespace Spectrum.UI.Messenger
{
    /// <summary>
    /// Trigger action to show a modal window.
    /// </summary>
    public sealed class ShowWindowAction : MessageTriggerAction<ShowWindowMessage>
    {
        /// <summary>
        /// Executes the action.
        /// </summary>
        /// <param name="parameter">Parameter.</param>
        protected override void InvokeCore(ShowWindowMessage parameter)
        {
            Window parentWindow;
            var element = this.AssociatedObject as FrameworkElement;
            if (element != null)
            {
                parentWindow = element.FindParentWindow();
            }
            else
            {
                parentWindow = Application.Current.MainWindow;
            }

            var window = parameter.ShowWindowFunc();
            if (window == null)
            {
                return;
            }

            window.Owner = parentWindow;
            window.ShowDialog();
        }
    }
}