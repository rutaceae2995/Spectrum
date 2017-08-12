using System.Windows;

namespace Spectrum.UI.Extension
{
    /// <summary>
    /// Extension method definition class on FrameworkElement.
    /// </summary>
    public static class FrameworkElementExtension
    {
        /// <summary>
        /// Returns a window to which the target element belongs.
        /// </summary>
        /// <param name="element">The element.</param>
        /// <returns>The parent window.</returns>
        public static Window FindParentWindow(this FrameworkElement element)
        {
            while (true)
            {
                if (element == null)
                {
                    return null;
                }

                var window = element as Window;
                if (window != null)
                {
                    return window;
                }

                element = element.Parent as FrameworkElement;
            }
        }
    }
}