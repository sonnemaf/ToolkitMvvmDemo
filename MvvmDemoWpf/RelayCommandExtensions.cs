using System;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;

namespace CommunityToolkit.Mvvm.Wpf.Input {
    /// <summary>
    /// A helper class with attached properties to interact with <see cref="IRelayCommand"/> instances on WPF.
    /// The attached properties in <see cref="RelayCommandExtensions"/> allow to enable support for the
    /// <see cref="CommandManager"/> class for commands bound to buttons, to support change notifications.
    /// </summary>
    public static class RelayCommandExtensions {
        /// <summary>
        /// A mapping of registered buttons to their change notifiers for the <see cref="ButtonBase.Command"/> property.
        /// </summary>
        private static readonly ConditionalWeakTable<DependencyObject, PropertyChangeNotifier> _notifiers = new ConditionalWeakTable<DependencyObject, PropertyChangeNotifier>();

        /// <summary>
        /// A mapping of registered buttons to their subscribed handlers.
        /// </summary>
        private static readonly ConditionalWeakTable<DependencyObject, SubscriptionData> _handlers = new ConditionalWeakTable<DependencyObject, SubscriptionData>();

        /// <summary>
        /// Gets the value of <see cref="IsCommandUpdateEnabled"/> for a given <see cref="DependencyObject"/> instance.
        /// </summary>
        /// <param name="element">The input <see cref="DependencyObject"/> for which to get the property value.</param>
        /// <returns>The value of the <see cref="IsCommandUpdateEnabled"/> property for the input <see cref="DependencyObject"/> instance.</returns>
        public static bool GetIsCommandUpdateEnabled(DependencyObject element) {
            return (bool)element.GetValue(IsCommandUpdateEnabled);
        }

        /// <summary>
        /// Sets the value of <see cref="IsCommandUpdateEnabled"/> for a given <see cref="DependencyObject"/> instance.
        /// </summary>
        /// <param name="element">The input <see cref="DependencyObject"/> for which to set the property value.</param>
        /// <param name="value">The value to set for the <see cref="INotifyCollectionChanged"/> property.</param>
        public static void SetIsCommandUpdateEnabled(DependencyObject element, bool value) {
            element.SetValue(IsCommandUpdateEnabled, value);
        }

        /// <summary>
        /// An attached property that indicates whether or not to enable <see cref="CommandManager"/> support.
        /// </summary>
        public static readonly DependencyProperty IsCommandUpdateEnabled = DependencyProperty.RegisterAttached(
              "IsCommandUpdateEnabled",
              typeof(bool),
              typeof(RelayCommandExtensions),
              new FrameworkPropertyMetadata(false, OnIsCommandUpdateEnabledChanged));

        /// <summary>
        /// Handler for <see cref="IsCommandUpdateEnabled"/> that enables notifications and command updates.
        /// </summary>
        /// <param name="d">The current target instance.</param>
        /// <param name="e">The <see cref="DependencyPropertyChangedEventArgs"/> value for the current change.</param>
        private static void OnIsCommandUpdateEnabledChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) {
            if (d is not DependencyObject obj) {
                return;
            }

            if (obj is not ICommandSource) {
                return;
            }

            // Ensure a notifier is enabled so that we can monitor for changes to the command
            // property in the future. This is also needed in case this attached property is
            // set before the binding engine has actually resolved the target command instance.
            if (!_notifiers.TryGetValue(obj, out _)) {
                PropertyChangeNotifier notifier = new (obj, nameof(ButtonBase.Command));

                notifier.PropertyChanged += (_, _) => ToggleIsCommandUpdateEnabled(obj);
            }

            ToggleIsCommandUpdateEnabled(obj);
        }

        /// <summary>
        /// Toggles the <see cref="IsCommandUpdateEnabled"/> logic for a target <see cref="DependencyObject"/> instance.
        /// </summary>
        /// <param name="obj">The target <see cref="DependencyObject"/> to toggle the property for.</param>
        private static void ToggleIsCommandUpdateEnabled(DependencyObject obj) {
            // Before doing anything, ensure previous handlers are removed
            if (_handlers.TryGetValue(obj, out var data)) {
                _ = _handlers.Remove(obj);

                CommandManager.RequerySuggested -= data.CommandManagerHandler;
                data.Command.CanExecuteChanged -= data.CommandHandler;
            }

            // Register the new handlers, if requested
            if (((ICommandSource)obj).Command is IRelayCommand command &&
                GetIsCommandUpdateEnabled(obj)) {
                object dummy = new object ();

                EventHandler
                    commandManagerHandler = (_, _) => CommandManager_RequerySuggested(command, dummy),
                    commandHandler = (_, _) => Command_CanExecuteChanged(dummy);

                _handlers.Add(obj, new SubscriptionData(command, dummy, commandManagerHandler, commandHandler));

                CommandManager.RequerySuggested += commandManagerHandler;
                command.CanExecuteChanged += commandHandler;
            }
        }

        /// <summary>
        /// Invokes <see cref="CommandManager.InvalidateRequerySuggested"/> when <see cref="ICommand.CanExecuteChanged"/>
        /// is raised on the current command. This mirrors the way <see cref="IRelayCommand.NotifyCanExecuteChanged"/>
        /// worked in MVVMLight, and allows this method to also notify the <see cref="CommandManager"/> class.
        /// </summary>
        /// <param name="dummy">The dummy object used to intercept mutual invocation.</param>
        private static void Command_CanExecuteChanged(object dummy) {
            // This check is needed to stop the invocation call in case this method was raised
            // directly by a requery request. Without this, the two methods would keep calling
            // each other infinitely, so instead we use a lock to prevent this. Note that this
            // does not add overhead since there is never contention over the lock, it's merely
            // used as a check by relying on the fact that the thread would still be the same.
            if (Monitor.IsEntered(dummy)) {
                return;
            }

            CommandManager.InvalidateRequerySuggested();
        }

        /// <summary>
        /// Invokes <see cref="IRelayCommand.NotifyCanExecuteChanged"/> when a requery is requested.
        /// </summary>
        /// <param name="command">The target <see cref="IRelayCommand"/> instance.</param>
        /// <param name="dummy">The dummy object used to intercept mutual invocation.</param>
        private static void CommandManager_RequerySuggested(IRelayCommand command, object dummy) {
            lock (dummy) {
                command.NotifyCanExecuteChanged();
            }
        }

        /// <summary>
        /// Helper class to monitor property changed events with weak references.
        /// Adapted from https://agsmith.wordpress.com/2008/04/07/propertydescriptor-addvaluechanged-alternative/.
        /// </summary>
        private sealed class PropertyChangeNotifier : DependencyObject, IDisposable {
            /// <summary>
            /// Creates a new <see cref="PropertyChangeNotifier"/> instance with the specified parameters.
            /// </summary>
            /// <param name="target">The target instance to monitor.</param>
            /// <param name="propertyName">The name of the property to monitor.</param>
            public PropertyChangeNotifier(DependencyObject target, string propertyName) {
                BindingOperations.SetBinding(
                    this,
                    ValueProperty,
                    new Binding {
                        Path = new PropertyPath(propertyName),
                        Mode = BindingMode.OneWay,
                        Source = target
                    });
            }

            /// <summary>
            /// Raised whenever the monitored property changes.
            /// </summary>
            public event EventHandler PropertyChanged;

            /// <summary>
            /// An attached property that receives notifications for the monitored property.
            /// </summary>
            public static readonly DependencyProperty ValueProperty = DependencyProperty.Register(
                "Value",
                typeof(object),
                typeof(PropertyChangeNotifier),
                new FrameworkPropertyMetadata(null, new PropertyChangedCallback(OnValuePropertyChanged)));

            /// <summary>
            /// Handler for <see cref="ValueProperty"/> that simply raises <see cref="PropertyChanged"/>.
            /// </summary>
            /// <param name="d">The current target instance.</param>
            /// <param name="e">The <see cref="DependencyPropertyChangedEventArgs"/> value for the current change.</param>
            private static void OnValuePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) {
                ((PropertyChangeNotifier)d).PropertyChanged?.Invoke(d, EventArgs.Empty);
            }

            /// <inheritdoc/>
            public void Dispose() {
                BindingOperations.ClearBinding(this, ValueProperty);
            }
        }

        /// <summary>
        /// A simple model to track registration data for target <see cref="DependencyObject"/> instances.
        /// </summary>
        private sealed class SubscriptionData {
            /// <summary>
            /// Creates a new <see cref="SubscriptionData"/> instance with the specified parameters.
            /// </summary>
            /// <param name="command">The <see cref="IRelayCommand"/> instance in use.</param>
            /// <param name="dummy">T</param>
            /// <param name="commandManagerHandler">The <see cref="EventHandler"/> for <see cref="CommandManager"/>.</param>
            /// <param name="commandHandler">The <see cref="EventHandler"/> for <see cref="IRelayCommand"/>.</param>
            public SubscriptionData(
                IRelayCommand command,
                object dummy,
                EventHandler commandManagerHandler,
                EventHandler commandHandler) {
                Command = command;
                Dummy = dummy;
                CommandManagerHandler = commandManagerHandler;
                CommandHandler = commandHandler;
            }

            /// <summary>
            /// Gets the <see cref="IRelayCommand"/> instance in use.
            /// </summary>
            public IRelayCommand Command { get; }

            /// <summary>
            /// Gets the locking instance to use.
            /// </summary>
            public object Dummy { get; }

            /// <summary>
            /// Gets the <see cref="EventHandler"/> for <see cref="CommandManager"/>.
            /// </summary>
            public EventHandler CommandManagerHandler { get; }

            /// <summary>
            /// Gets the <see cref="EventHandler"/> for <see cref="IRelayCommand"/>.
            /// </summary>
            public EventHandler CommandHandler { get; }
        }
    }
}