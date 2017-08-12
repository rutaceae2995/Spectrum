using System;
using Microsoft.Practices.Prism.Commands;
using Spectrum.Mvvm;

namespace Spectrum.Commands.InternalWrapper
{
    /// <summary>
    /// Prism DelegateCommand wrapper class.
    /// </summary>
    internal class InternalDelegateCommand : DelegateCommand, ICommandBase
    {
        /// <summary>
        /// Initializes a new instance of the InternalDelegateCommand class.
        /// </summary>
        /// <param name="executeMethod">The action.</param>
        internal InternalDelegateCommand(Action executeMethod)
            : base(executeMethod)
        {
        }

        /// <summary>
        /// Initializes a new instance of the InternalDelegateCommand class.
        /// </summary>
        /// <param name="executeMethod">The action.</param>
        /// <param name="canExecuteMethod">The method that determines whether the command can execute in its current state.</param>
        internal InternalDelegateCommand(Action executeMethod, Func<bool> canExecuteMethod)
            : base(executeMethod, canExecuteMethod)
        {
        }
    }
}