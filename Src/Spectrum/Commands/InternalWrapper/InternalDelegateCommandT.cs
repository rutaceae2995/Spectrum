using System;
using Microsoft.Practices.Prism.Commands;
using Spectrum.Mvvm;

namespace Spectrum.Commands.InternalWrapper
{
    /// <summary>
    /// Prism DelegateCommand wrapper generic class.
    /// </summary>
    internal class InternalDelegateCommand<T>: DelegateCommand<T>, ICommandBase
    {
        /// <summary>
        /// Initializes a new instance of the InternalDelegateCommand class.
        /// </summary>
        /// <param name="executeMethod">The action.</param>
        internal InternalDelegateCommand(Action<T> executeMethod)
            : base(executeMethod)
        {
        }

        /// <summary>
        /// Initializes a new instance of the InternalDelegateCommand class.
        /// </summary>
        /// <param name="executeMethod">The action.</param>
        /// <param name="canExecuteMethod">The method that determines whether the command can execute in its current state.</param>
        internal InternalDelegateCommand(Action<T> executeMethod, Func<T, bool> canExecuteMethod)
            : base(executeMethod, canExecuteMethod)
        {
        }
    }
}