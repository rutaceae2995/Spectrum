using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Dynamic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using Microsoft.Practices.Prism.Interactivity.InteractionRequest;
using Spectrum.Attributes;
using Spectrum.Commands;
using Spectrum.Extension;
using Spectrum.InteractionMessenger;
using Spectrum.WeakEventListener;

namespace Spectrum.Mvvm
{
    /// <summary>
    /// The base class of ViewModel which implements INotifyPropertyChanged, ICommandGenerator, and ISendMessage.
    /// </summary>
    public abstract class ViewModelBase : DynamicObject, INotifyPropertyChanged, ICommandGenerator, ISendMessage
    {
        /// <summary>
        /// ICommandGenerator instance.
        /// </summary>
        private readonly ICommandGenerator commandGenerator;

        /// <summary>
        /// Default InteractionRequest instance.
        /// </summary>
        private readonly Lazy<InteractionRequest<Notification>> defaultInteractionRequest = new Lazy<InteractionRequest<Notification>>(() => new InteractionRequest<Notification>());

        /// <summary>
        /// The command dictionary for aggregating property change notification events.
        /// Key: Property name,
        /// Value: Command instance.
        /// </summary>
        private readonly Dictionary<string, TimeShiftDelegateCommand> propertyChangedCommands = new Dictionary<string, TimeShiftDelegateCommand>();

        /// <summary>
        /// The listener dictionary for property changed events.
        /// Key: Property name,
        /// Value: PropertyChangedWeakEventListener instance.
        /// </summary>
        private readonly Dictionary<string, PropertyChangedWeakEventListener> propertyChangedListeners = new Dictionary<string, PropertyChangedWeakEventListener>();
        
        /// <summary>
        /// PropertyChanged event handler.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Initializes a new instance of the ViewModelBase class.
        /// </summary>
        /// <param name="command">ICommandGenerator instance.</param>
        protected ViewModelBase(ICommandGenerator command)
        {
            if (command == null)
            {
                throw new ArgumentNullException(nameof(command));
            }

            this.commandGenerator = command;
            this.CheckImplementation();
        }

        /// <summary>
        /// Initializes a new instance of the ViewModelBase class.
        /// </summary>
        protected ViewModelBase()
            : this(new StandardCommandFactory())
        {
        }

        /// <summary>
        /// The default trigger property of InteractionTriggerAction.
        /// </summary>
        [Obsolete("This property is not available. This property is for internal use only.", true)]
        public object DefaultTriggerAction => this.defaultInteractionRequest.Value;

        /// <summary>
        /// Gets the value of the property.
        /// </summary>
        /// <param name="binder">GetMemberBinder instance.</param>
        /// <param name="result">The result.</param>
        /// <returns>Success or failure.</returns>
        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            var propertyName = binder.Name;
            var property = this.GetProperty(propertyName);
            if (property == null)
            {
                result = null;
                return false;
            }

            result = property.GetValue(this);
            return true;
        }

        /// <summary>
        /// Sets the value of the property.
        /// </summary>
        /// <param name="binder">SetMemberBinder instance.</param>
        /// <param name="value">The value.</param>
        /// <returns>Changed or not.</returns>
        public override bool TrySetMember(SetMemberBinder binder, object value)
        {
            var propertyName = binder.Name;
            var property = this.GetProperty(propertyName);
            if (property == null)
            {
                return false;
            }

            var current = property.GetValue(this);
            if (current != value)
            {
                property.SetValue(this, value);
            }

            var bindableAttribute = property.GetCustomAttributes<BindablePropertyAttribute>().FirstOrDefault();
            if (bindableAttribute == null)
            {
                return true;
            }

            if (bindableAttribute.AggregateWaitTimeSecond <= 0)
            {
                this.RaisePropertyChanged(propertyName);
            }
            else
            {
                if (!this.propertyChangedCommands.ContainsKey(propertyName))
                {
                    this.propertyChangedCommands.Add(
                        propertyName,
                        new TimeShiftDelegateCommand(
                            () => this.RaisePropertyChanged(propertyName),
                            0,
                            bindableAttribute.AggregateWaitTimeSecond,
                            true));
                }

                this.propertyChangedCommands[propertyName].Execute();
            }

            this.RaiseRelationPropertyChanged(property);
            this.ExecutePropertyChangedAction(property);
            return true;
        }

        /// <summary>
        /// Creates a new command instance.
        /// </summary>
        /// <param name="commandAction">The command action.</param>
        /// <param name="canExecuteFunc">Determines whether the command can execute in its current state.</param>
        /// <param name="callerMemberName">The caller member property name.</param>
        /// <returns>The command.</returns>
        public ICommandBase CreateCommand(Action commandAction, Func<bool> canExecuteFunc = null, [CallerMemberName] string callerMemberName = "")
        {
            return this.commandGenerator.CreateCommand(commandAction, canExecuteFunc, callerMemberName);
        }

        /// <summary>
        /// Creates a new command instance.
        /// </summary>
        /// <typeparam name="T">The type of command argument.</typeparam>
        /// <param name="commandAction">The command action.</param>
        /// <param name="canExecuteFunc">Determines whether the command can execute in its current state.</param>
        /// <param name="callerMemberName">The caller member property name.</param>
        /// <returns>The command.</returns>
        public ICommandBase CreateCommand<T>(Action<T> commandAction, Func<T, bool> canExecuteFunc = null, [CallerMemberName] string callerMemberName = "")
        {
            return this.commandGenerator.CreateCommand(commandAction, canExecuteFunc, callerMemberName);
        }

        /// <summary>
        /// Sends an action message using the messenger pattern.
        /// </summary>
        /// <typeparam name="TParameter">The type of MessageParameter.</typeparam>
        /// <param name="parameter">A parameter</param>
        public void SendMessage<TParameter>(TParameter parameter)
            where TParameter : MessageParameter
        {
            this.defaultInteractionRequest.Value.Raise(parameter.Notification);
        }

        /// <summary>
        /// Raise property changed event.
        /// Typcally, instead of calling this method, use the BindablePropertyAttribute.
        /// </summary>
        /// <param name="propertyName">The target property name.</param>
        protected void RaisePropertyChanged(string propertyName)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// Raise CanExecuteChanged event.
        /// Typcally, instead of calling this method, use the BindablePropertyAttribute.
        /// </summary>
        /// <param name="commandName">The command property name.</param>
        protected void RaiseCanExecuteChanged(string commandName)
        {
            var property = this.GetProperty(commandName);
            var command = property?.GetValue(this) as ICommandBase;
            command?.RaiseCanExecuteChanged();
        }

        /// <summary>
        /// Sets the value of the property.
        /// </summary>
        /// <typeparam name="T">The type of the target property.</typeparam>
        /// <param name="propertyName">The property name.</param>
        /// <param name="value">The value.</param>
        protected void SetProperty<T>(string propertyName, T value)
        {
            var property = this.GetProperty(propertyName);
            if (property == null)
            {
                return;
            }

            var bindableCollectionAttribute = property.GetCustomAttributes<BindableCollectionPropertyAttribute>().FirstOrDefault();
            var current = property.GetValue(this);
            if (Equals(current, value))
            {
                return;
            }

            if (bindableCollectionAttribute != null)
            {
                var collection = current as INotifyPropertyChanged;
                if (collection != null)
                {
                    PropertyChangedEventManager.RemoveListener(collection, this.GetListener(propertyName), string.Empty);
                }
            }

            property.SetValue(this, value);

            if (bindableCollectionAttribute != null && value != null)
            {
                var collection = value as INotifyPropertyChanged;
                if (collection != null)
                {
                    PropertyChangedEventManager.AddListener(collection, this.GetListener(propertyName), string.Empty);
                }
            }

            this.RaisePropertyChanged(propertyName);
            this.RaiseRelationPropertyChanged(property);
            this.ExecutePropertyChangedAction(property);
        }

        /// <summary>
        /// Gets the property changed event listener.
        /// </summary>
        /// <param name="propertyName">The target property name.</param>
        /// <returns>Property changed event listener.</returns>
        private PropertyChangedWeakEventListener GetListener(string propertyName)
        {
            PropertyChangedWeakEventListener targetListener;
            if (!this.propertyChangedListeners.ContainsKey(propertyName))
            {
                targetListener = new PropertyChangedWeakEventListener(propertyName, this.OnCollectionPropertyChanged);
                this.propertyChangedListeners.Add(propertyName, targetListener);
            }
            else
            {
                targetListener = this.propertyChangedListeners[propertyName];
            }

            return targetListener;
        }

        /// <summary>
        /// Occurs after property changed.
        /// </summary>
        /// <param name="propertyName">The property name.</param>
        /// <param name="collectionPropertyName">The target changed property name. (not used)</param>
        private void OnCollectionPropertyChanged(string propertyName, string collectionPropertyName)
        {
            var property = this.GetProperty(propertyName);
            if (property == null)
            {
                return;
            }

            this.RaisePropertyChanged(propertyName);
            this.RaiseRelationPropertyChanged(property);
            this.ExecutePropertyChangedAction(property);
        }

        /// <summary>
        /// Raise PropertyChanged or CanExecuteChanged event of the relevant properties.
        /// </summary>
        /// <param name="property">The property information.</param>
        private void RaiseRelationPropertyChanged(PropertyInfo property)
        {
            var bindableAttribute = property?.GetCustomAttributes<BindablePropertyAttribute>().FirstOrDefault();
            if (bindableAttribute?.RelevantProperties == null)
            {
                return;
            }

            foreach (var relationPropertyName in bindableAttribute.RelevantProperties)
            {
                var prop = this.GetProperty(relationPropertyName);
                var command = prop?.GetValue(this) as ICommandBase;
                if (command != null)
                {
                    this.RaiseCanExecuteChanged(relationPropertyName);
                }
                else
                {
                    this.RaisePropertyChanged(relationPropertyName);
                }
            }
        }

        /// <summary>
        /// Occurs after property changed.
        /// </summary>
        /// <param name="property">The property information.</param>
        private void ExecutePropertyChangedAction(PropertyInfo property)
        {
            var bindableAttribute = property?.GetCustomAttributes<BindablePropertyAttribute>().FirstOrDefault();
            if (bindableAttribute?.PropertyChangedAction == null)
            {
                return;
            }

            var targetType = this.GetType();
            while (targetType != null)
            {
                var method = targetType.GetMethod(bindableAttribute.PropertyChangedAction, BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
                if (method != null)
                {
                    var attribute = method.GetCustomAttributes<BindingMethodInvokeModeAttribute>().FirstOrDefault();
                    if (attribute == null)
                    {
                        method.Invoke(this, null);
                    }
                    else
                    {
                        var command = new TimeShiftDelegateCommand(() => method.Invoke(this, null), null, attribute.FirstDelayWaitTime, attribute.CoolingTime, attribute.InvokeAfterCoolingTime);
                        command.Execute();
                    }

                    break;
                }

                targetType = targetType.BaseType;
            }
        }

        /// <summary>
        /// For developper (Debug check method).
        /// </summary>
        [Conditional("DEBUG")]
        private void CheckImplementation()
        {
            var properties = this.GetType().GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

            // BindableProperty attribute check
            foreach (var property in properties)
            {
                var bindableAttribute = property.GetCustomAttributes<BindablePropertyAttribute>().FirstOrDefault();
                if (bindableAttribute == null)
                {
                    continue;
                }

                if (property.CanRead && !property.GetGetMethod(true).IsFamily)
                {
                    throw new InvalidProgramException($"Property [{property.Name}] using BindablePropertyAttribute must be protected.");
                }

                if (property.CanWrite && !property.GetSetMethod(true).IsFamily)
                {
                    throw new InvalidProgramException($"Property [{property.Name}] using BindablePropertyAttribute must be protected.");
                }
            }

            // BindableCollectionProperty attribute check
            foreach (var property in properties)
            {
                var bindableCollectionAttribute = property.GetCustomAttributes<BindableCollectionPropertyAttribute>().FirstOrDefault();
                if (bindableCollectionAttribute == null)
                {
                    continue;
                }

                var getter = property.GetGetMethod(true);
                if (property.CanRead && !getter.IsFamily)
                {
                    throw new InvalidProgramException($"Property [{property.Name}] using BindableCollectionPropertyAttribute must be protected.");
                }

                if (!getter.ReturnType.IsGenericType
                    || getter.ReturnType.GetGenericTypeDefinition() != typeof(NotifiableObservableCollection<>) && getter.ReturnType.GetGenericTypeDefinition() != typeof(NotifiableModelObservableCollection<>))
                {
                    throw new InvalidProgramException($"Property [{property.Name}] using BindableCollectionPropertyAttribute must be NotifiableObservableCollection<T> or NotifiableModelObservableCollection<T>.");
                }
            }
        }
    }
}