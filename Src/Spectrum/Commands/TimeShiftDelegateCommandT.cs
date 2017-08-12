using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Spectrum.Mvvm;

namespace Spectrum.Commands
{
    /// <summary>
    /// Generic Command class with time control.
    /// </summary>
    public sealed class TimeShiftDelegateCommand<T> : ICommandBase
    {
        /// <summary>
        /// command function.
        /// </summary>
        private readonly Func<T, Task> execute;

        /// <summary>
        /// The function that determines whether the command can execute in its current state.
        /// </summary>
        private readonly Func<T, bool> canExecute;

        /// <summary>
        /// The wait time (millisecond) before execute command.
        /// </summary>
        private readonly long firstDelayTime;

        /// <summary>
        /// The cooling time (millisecond) to suppress execution.
        /// </summary>
        private readonly long coolingTime;

        /// <summary>
        /// (only coolingTime > 0) Whether to execute the command when the suppression period is completed.
        /// </summary>
        private readonly bool invokeAfterCoolingTime;

        /// <summary>
        /// last execute time (Ticks value).
        /// </summary>
        private long lastExecuteTime;

        /// <summary>
        /// Is preventing command execute.
        /// </summary>
        private bool blockAllInvoke;

        /// <summary>
        /// CanExecuteChanged event handler.
        /// </summary>
        private event EventHandler canExecuteChanged;

        /// <summary>
        /// Initializes a new instance of the TimeShiftDelegateCommand class.
        /// </summary>
        /// <param name="executeMethod">command function.</param>
        /// <param name="firstDelayTime">The wait time (millisecond) before execute command.</param>
        /// <param name="coolingTime">The cooling time (millisecond) to suppress execution.</param>
        /// <param name="invokeAfterCoolingTime">(only coolingTime > 0) Whether to execute the command when the suppression period is completed.</param>
        public TimeShiftDelegateCommand(Action<T> executeMethod, long firstDelayTime, long coolingTime, bool invokeAfterCoolingTime)
            : this(executeMethod, null, firstDelayTime, coolingTime, invokeAfterCoolingTime)
        {
        }

        /// <summary>
        /// Initializes a new instance of the TimeShiftDelegateCommand class.
        /// </summary>
        /// <param name="executeMethod">command function.</param>
        /// <param name="canExecuteMethod">The function that determines whether the command can execute in its current state.</param>
        /// <param name="firstDelayTime">The wait time (millisecond) before execute command.</param>
        /// <param name="coolingTime">The cooling time (millisecond) to suppress execution.</param>
        /// <param name="invokeAfterCoolingTime">(only coolingTime > 0) Whether to execute the command when the suppression period is completed.</param>
        public TimeShiftDelegateCommand(Action<T> executeMethod, Func<T, bool> canExecuteMethod, long firstDelayTime, long coolingTime, bool invokeAfterCoolingTime)
        {
            if (executeMethod == null)
            {
                throw new ArgumentNullException(nameof(executeMethod));
            }

            this.execute =
                args =>
                {
                    executeMethod(args);
                    return Task.CompletedTask;
                };
            this.canExecute = canExecuteMethod;
            this.firstDelayTime = firstDelayTime;
            this.coolingTime = coolingTime;
            this.invokeAfterCoolingTime = invokeAfterCoolingTime;
        }

        /// <summary>
        /// Adds or removes the event handler of The CanExecuteChanged.
        /// </summary>
        public event EventHandler CanExecuteChanged
        {
            add
            {
                this.canExecuteChanged += value;
                CommandManager.RequerySuggested += value;
            }

            remove
            {
                this.canExecuteChanged -= value;
                CommandManager.RequerySuggested -= value;
            }
        }

        /// <summary>
        /// Execute the command.
        /// </summary>
        /// <param name="parameter">The parameter</param>
        public async void Execute(T parameter)
        {
            await this.ExecuteInternal(parameter);
        }

        /// <summary>
        /// Execute the command.
        /// </summary>
        /// <param name="parameter">The parameter</param>
        /// <returns>Task.</returns>
        public Task ExecuteAsync(T parameter)
        {
            return this.ExecuteInternal(parameter);
        }

        /// <summary>
        /// Execute the command.
        /// </summary>
        /// <param name="parameter">The parameter</param>
        async void ICommand.Execute(object parameter)
        {
            await this.ExecuteInternal((T)parameter);
        }

        /// <summary>
        /// Determines whether the command can execute in its current state.
        /// </summary>
        /// <param name="parameter">The parameter</param>
        /// <returns>Can execute or not.</returns>
        public bool CanExecute(T parameter)
        {
            if (this.blockAllInvoke)
            {
                return false;
            }

            if (this.canExecute == null)
            {
                return true;
            }

            return this.canExecute(parameter);
        }

        /// <summary>
        /// Determines whether the command can execute in its current state.
        /// </summary>
        /// <param name="parameter">The parameter.</param>
        /// <returns>Can execute or not.</returns>
        bool ICommand.CanExecute(object parameter)
        {
            return this.CanExecute((T)parameter);
        }

        /// <summary>
        /// Execute the command.
        /// </summary>
        /// <param name="parameter">The parameter.</param>
        /// <returns>Task.</returns>
        internal async Task ExecuteInternal(T parameter)
        {
            if (this.blockAllInvoke)
            {
                return;
            }

            if (this.firstDelayTime > 0)
            {
                this.SetInvokeBlock(true);
                await Task.Delay(TimeSpan.FromMilliseconds(this.firstDelayTime));
            }

            if (this.coolingTime > 0)
            {
                var past = TimeSpan.FromTicks(DateTime.Now.Ticks - this.lastExecuteTime).TotalMilliseconds;
                if (past < this.coolingTime)
                {
                    this.SetInvokeBlock(true);
                    await Task.Delay(TimeSpan.FromMilliseconds(this.coolingTime - (long)past));

                    if (this.invokeAfterCoolingTime)
                    {
                        await this.execute(parameter);
                    }

                    this.lastExecuteTime = DateTime.Now.Ticks;
                    this.SetInvokeBlock(false);
                    return;
                }
            }

            await this.execute(parameter);
            this.lastExecuteTime = DateTime.Now.Ticks;
            this.SetInvokeBlock(false);
        }

        /// <summary>
        /// Sets the flag to prevent command execute.
        /// </summary>
        /// <param name="block">Enables prevent flag.</param>
        private void SetInvokeBlock(bool block)
        {
            this.blockAllInvoke = block;
            this.RaiseCanExecuteChanged();
        }

        /// <summary>
        /// Raise the CanExecuteChanged event.
        /// </summary>
        public void RaiseCanExecuteChanged()
        {
            var eventHandler = this.canExecuteChanged;
            eventHandler?.Invoke(this, EventArgs.Empty);
        }
    }
}