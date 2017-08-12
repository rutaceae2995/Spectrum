using System;
using System.Threading.Tasks;

namespace Spectrum.Thread
{
    /// <summary>
    /// This class supplies utility methods to simplify writing multithreading code.
    /// </summary>
    public static class ThreadUtility
    {
        /// <summary>
        /// Executes the specified action on the UI thread.
        /// If the caller is a UI thread, executes on the current thread.
        /// </summary>
        /// <param name="action">The execute action.</param>
        public static void InvokeUIThread(Action action)
        {
            var dispacher = System.Windows.Application.Current.Dispatcher;
            if (dispacher == null)
            {
                return;
            }

            if (dispacher.CheckAccess())
            {
                // UI Thread
                action();
            }
            else
            {
                // Invoke UI Thread
                dispacher.Invoke(action);
            }
        }

        /// <summary>
        /// Executes the specified delegate asynchronously on the UI thread.
        /// </summary>
        /// <param name="action">The execute action.</param>
        public static void BeginInvokeUIThread(Action action)
        {
            System.Windows.Application.Current.Dispatcher?.BeginInvoke(action);
        }

        /// <summary>
        /// Executes the specified action on a background thread.
        /// If createNewThread is true, creates new background thread.
        /// If createNewThread is false, execute on the current background thread.
        /// but if current thread is not background thread, creates new background thread.
        /// </summary>
        /// <param name="action">The execute action.</param>
        /// <param name="createNewThread">Whether to create a new thread.</param>
        public static void InvokeNonUIThread(Action action, bool createNewThread = false)
        {
            bool threadCreateFlag = createNewThread;
            if (!createNewThread)
            {
                var dispacher = System.Windows.Application.Current.Dispatcher;
                if (dispacher == null)
                {
                    return;
                }

                if (dispacher.CheckAccess())
                {
                    // UI Thread
                    threadCreateFlag = true;
                }
            }

            if (threadCreateFlag)
            {
                Task.Factory.StartNew(action);
            }
            else
            {
                action();
            }
        }
    }
}