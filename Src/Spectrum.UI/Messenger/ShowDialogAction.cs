using System.Windows;
using Spectrum.InteractionMessenger;
using Spectrum.UI.Extension;

namespace Spectrum.UI.Messenger
{
    /// <summary>
    /// Trigger action to show a dialog.
    /// </summary>
    public sealed class ShowDialogAction : MessageTriggerAction<ShowDialogMessage>
    {
        /// <summary>
        /// Executes the action.
        /// </summary>
        /// <param name="parameter">Parameter.</param>
        protected override void InvokeCore(ShowDialogMessage parameter)
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

            parameter.Result = parameter.ShowMessageBoxFunc(parentWindow);
        }
    }
}