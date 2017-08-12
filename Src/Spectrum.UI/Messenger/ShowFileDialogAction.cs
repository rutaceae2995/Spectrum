using System.Windows;
using Spectrum.InteractionMessenger;
using Spectrum.UI.Extension;

namespace Spectrum.UI.Messenger
{
    /// <summary>
    /// Trigger action to show a FileDialog.
    /// </summary>
    public sealed class ShowFileDialogAction : MessageTriggerAction<ShowFileDialogMessage>
    {
        /// <summary>
        /// Executes the action.
        /// </summary>
        /// <param name="parameter">Parameter.</param>
        protected override void InvokeCore(ShowFileDialogMessage parameter)
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

           var window = parameter.ShowDialogFunc();
            if (window == null)
            {
                return;
            }

            var result = window.ShowDialog(parentWindow);
            parameter.ResultAction(result, window.FileNames);
        }
    }
}