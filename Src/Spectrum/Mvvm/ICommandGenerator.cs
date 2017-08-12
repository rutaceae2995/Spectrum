using System;
using System.Runtime.CompilerServices;

namespace Spectrum.Mvvm
{
    /// <summary>
    /// This class supplies the interface that can generate ICommand.
    /// </summary>
    public interface ICommandGenerator
    {
        /// <summary>
        /// Creates a new ICommand instance.
        /// </summary>
        /// <param name="commandAction">The command action.</param>
        /// <param name="canExecuteFunc">Determines whether the command can execute in its current state.</param>
        /// <param name="callerMemberName">The caller member property name.</param>
        /// <returns>The command.</returns>
        ICommandBase CreateCommand(Action commandAction, Func<bool> canExecuteFunc = null, [CallerMemberName] string callerMemberName = "");

        /// <summary>
        /// Creates the ICommand instance.
        /// </summary>
        /// <typeparam name="T">The command argument type.</typeparam>
        /// <param name="commandAction">The command action.</param>
        /// <param name="canExecuteFunc">Determines whether the command can execute in its current state.</param>
        /// <param name="callerMemberName">The caller member property name.</param>
        /// <returns>The command.</returns>
        ICommandBase CreateCommand<T>(Action<T> commandAction, Func<T, bool> canExecuteFunc = null, [CallerMemberName] string callerMemberName = "");
    }
}