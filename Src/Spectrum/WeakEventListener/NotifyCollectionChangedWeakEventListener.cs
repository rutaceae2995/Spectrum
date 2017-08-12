using System;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Windows;

namespace Spectrum.WeakEventListener
{
    /// <summary>
    /// WeakEventパターンを実装したコレクション変更イベントリスナークラス。
    /// </summary>
    public sealed class NotifyCollectionChangedWeakEventListener : IWeakEventListener
    {
        /// <summary>
        /// CollectionChangedの処理。
        /// </summary>
        private readonly Action<NotifyCollectionChangedEventArgs> raiseCollectionChangedAction;

        public NotifyCollectionChangedWeakEventListener(Action<NotifyCollectionChangedEventArgs> action)
        {
            this.raiseCollectionChangedAction = action;
        }

        public bool ReceiveWeakEvent(Type managerType, object sender, EventArgs e)
        {
            if (typeof(PropertyChangedEventManager) != managerType)
            {
                return false;
            }

            var eventArgs = e as NotifyCollectionChangedEventArgs;
            if (eventArgs == null)
            {
                return false;
            }

            this.raiseCollectionChangedAction(eventArgs);
            return true;
        }
    }
}
