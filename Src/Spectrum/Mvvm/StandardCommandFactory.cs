using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using Spectrum.Attributes;
using Spectrum.Commands;
using Spectrum.Commands.InternalWrapper;

namespace Spectrum.Mvvm
{
    /// <summary>
    /// The standard command factory class.
    /// </summary>
    public sealed class StandardCommandFactory : ICommandGenerator
    {
        /// <summary>
        /// The cache dictionary of command property name and instance.
        /// </summary>
        private Dictionary<string, ICommandBase> commandCache;

        /// <summary>
        /// Initializes a new instance of the ICommandBase instance.
        /// </summary>
        /// <param name="commandAction">The command action.</param>
        /// <param name="canExecuteFunc">Determines whether the command can execute in its current state.</param>
        /// <param name="callerMemberName">The caller member property name.</param>
        /// <returns>The command.</returns>
        public ICommandBase CreateCommand(Action commandAction, Func<bool> canExecuteFunc = null, [CallerMemberName] string callerMemberName = "")
        {
            if (string.IsNullOrEmpty(callerMemberName))
            {
                throw new ArgumentNullException(nameof(callerMemberName));
            }

            var cachedCommand = this.GetCachedCommand(callerMemberName);
            if (cachedCommand != null)
            {
                return cachedCommand;
            }

            var attribute = commandAction.Method.GetCustomAttributes<BindingMethodInvokeModeAttribute>().FirstOrDefault();
            ICommandBase newCommand;
            if (attribute == null)
            {
                if (canExecuteFunc == null)
                {
                    newCommand = new InternalDelegateCommand(commandAction);
                }
                else
                {
                    newCommand = new InternalDelegateCommand(commandAction, canExecuteFunc);
                }
            }
            else
            {
                newCommand = new TimeShiftDelegateCommand(commandAction, canExecuteFunc, attribute.FirstDelayWaitTime, attribute.CoolingTime, attribute.InvokeAfterCoolingTime);
            }

            return this.RegistCommandCache(callerMemberName, newCommand);
        }

        /// <summary>
        /// Initializes a new instance of the ICommandBase instance.
        /// </summary>
        /// <typeparam name="T">The argument type.</typeparam>
        /// <param name="commandAction">The command action.</param>
        /// <param name="canExecuteFunc">Determines whether the command can execute in its current state.</param>
        /// <param name="callerMemberName">The caller member property name.</param>
        /// <returns>The command.</returns>
        public ICommandBase CreateCommand<T>(Action<T> commandAction, Func<T, bool> canExecuteFunc = null, [CallerMemberName] string callerMemberName = "")
        {
            if (string.IsNullOrEmpty(callerMemberName))
            {
                throw new ArgumentNullException(nameof(callerMemberName));
            }

            var cachedCommand = this.GetCachedCommand(callerMemberName);
            if (cachedCommand != null)
            {
                return cachedCommand;
            }

            var attribute = commandAction.Method.GetCustomAttributes<BindingMethodInvokeModeAttribute>().FirstOrDefault();
            ICommandBase newCommand;
            if (attribute == null)
            {
                if (canExecuteFunc == null)
                {
                    newCommand = new InternalDelegateCommand<T>(commandAction);
                }
                else
                {
                    newCommand = new InternalDelegateCommand<T>(commandAction, canExecuteFunc);
                }
            }
            else
            {
                newCommand = new TimeShiftDelegateCommand<T>(commandAction, canExecuteFunc, attribute.FirstDelayWaitTime, attribute.CoolingTime, attribute.InvokeAfterCoolingTime);
            }

            return this.RegistCommandCache(callerMemberName, newCommand);
        }

        /// <summary>
        /// Gets the command instance from a property name.
        /// </summary>
        /// <param name="propertyName">The target property name.</param>
        /// <returns>The cached command instance. If not exists, returns null.</returns>
        private ICommandBase GetCachedCommand(string propertyName)
        {
            if (this.commandCache == null)
            {
                return null;
            }

            if (!this.commandCache.ContainsKey(propertyName))
            {
                return null;
            }

            return this.commandCache[propertyName];
        }

        /// <summary>
        /// Adds a command instance to the cache.
        /// </summary>
        /// <param name="propertyName">A property name.</param>
        /// <param name="command">A command instance.</param>
        /// <returns>The added command instance.</returns>
        private ICommandBase RegistCommandCache(string propertyName, ICommandBase command)
        {
            if (this.commandCache == null)
            {
                this.commandCache = new Dictionary<string, ICommandBase>();
            }

            if (!this.commandCache.ContainsKey(propertyName))
            {
                this.commandCache.Add(propertyName, command);
            }

            return command;
        }
    }
}