using System.ComponentModel;
using System.Windows;
using System.Windows.Input;

namespace Spectrum.UI.AttachedProperty
{
    /// <summary>
    /// This class supplies to attach a ICommand to Window events.
    /// </summary>
    public sealed class WindowCommand
    {
        /// <summary>
        /// WindowClosingCommand property.
        /// </summary>
        public static readonly DependencyProperty WindowClosingCommandProperty =
            DependencyProperty.RegisterAttached(
                "WindowClosingCommand",
                typeof(ICommand),
                typeof(WindowCommand),
                new FrameworkPropertyMetadata(
                    null,
                    OnWindowClosingCommandChanged));

        /// <summary>
        /// Gets the value of WindowClosingCommand property.
        /// </summary>
        /// <param name="obj">The target object.</param>
        /// <returns>the WindowClosingCommand.</returns>
        public static ICommand GetWindowClosingCommand(DependencyObject obj)
        {
            return obj.GetValue(WindowClosingCommandProperty) as ICommand;
        }

        /// <summary>
        /// Sets a WindowClosingCommand.
        /// </summary>
        /// <param name="obj">The target object.</param>
        /// <param name="value">The WindowClosingCommand.</param>
        public static void SetWindowClosingCommand(DependencyObject obj, ICommand value)
        {
            obj.SetValue(WindowClosingCommandProperty, value);
        }

        /// <summary>
        /// Occurs after WindowClosingCommand property changed.
        /// </summary>
        /// <param name="d">The target window object.</param>
        /// <param name="e">The property changed event argument.</param>
        private static void OnWindowClosingCommandChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var window = d as Window;
            if (window == null)
            {
                return;
            }

            if (e.NewValue == null)
            {
                window.Closing -= WindowOnClosing;
            }
            else
            {
                window.Closing += WindowOnClosing;
            }
        }

        /// <summary>
        /// Occurs after WindowClosing event.
        /// </summary>
        /// <param name="sender">The event sender object.</param>
        /// <param name="cancelEventArgs">The cancel event argument.</param>
        private static void WindowOnClosing(object sender, CancelEventArgs cancelEventArgs)
        {
            var command = GetWindowClosingCommand((DependencyObject)sender);
            if (command == null)
            {
                return;
            }

            if (command.CanExecute(cancelEventArgs))
            {
                command.Execute(cancelEventArgs);
            }
        }
    }
}