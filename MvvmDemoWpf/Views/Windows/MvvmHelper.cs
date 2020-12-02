using Microsoft.Toolkit.Mvvm.Input;
using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Input;

namespace MvvmDemoWPF.Views.Windows {

    public class MvvmHelper {

        #region NotifyCommandParameterChanges Attached Property

        /// <summary> 
        /// Identifies the NotifyCommandParameterChanges attachted property. This enables animation, styling, binding, etc...
        /// </summary>
        public static readonly DependencyProperty NotifyCommandParameterChangesProperty =
            DependencyProperty.RegisterAttached("NotifyCommandParameterChanges",
                                                typeof(bool),
                                                typeof(MvvmHelper),
                                                new PropertyMetadata(default(bool), OnNotifyCommandParameterChangesChanged));

        /// <summary>
        /// NotifyCommandParameterChanges changed handler. 
        /// </summary>
        /// <param name="d">FrameworkElement that changed its NotifyCommandParameterChanges attached property.</param>
        /// <param name="e">DependencyPropertyChangedEventArgs with the new and old value.</param> 
        private static void OnNotifyCommandParameterChangesChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) {
            if (d is ICommandSource source) {
                var value = (bool)e.NewValue;
                if (source is ButtonBase btn) {
                    var pd = DependencyPropertyDescriptor.FromProperty(ButtonBase.CommandParameterProperty, typeof(ButtonBase));

                    RoutedEventHandler unloaded = (s, e) => {
                        pd.RemoveValueChanged(btn, OnCommandParameterChanged);
                    };

                    if (value) {
                        pd.AddValueChanged(btn, OnCommandParameterChanged);
                        btn.Unloaded += unloaded;
                    } else {
                        pd.RemoveValueChanged(btn, OnCommandParameterChanged);
                        btn.Unloaded -= unloaded;
                    }
                }
            }
        }

        private static void OnCommandParameterChanged(object sender, EventArgs e) {
            ((sender as ButtonBase).Command as IRelayCommand)?.NotifyCanExecuteChanged();
        }

        /// <summary>
        /// Gets the value of the NotifyCommandParameterChanges attached property from the specified FrameworkElement.
        /// </summary>
        public static bool GetNotifyCommandParameterChanges(DependencyObject obj) {
            return (bool)obj.GetValue(NotifyCommandParameterChangesProperty);
        }


        /// <summary>
        /// Sets the value of the NotifyCommandParameterChanges attached property to the specified FrameworkElement.
        /// </summary>
        /// <param name="obj">The object on which to set the NotifyCommandParameterChanges attached property.</param>
        /// <param name="value">The property value to set.</param>
        public static void SetNotifyCommandParameterChanges(DependencyObject obj, bool value) {
            obj.SetValue(NotifyCommandParameterChangesProperty, value);
        }

        #endregion NotifyCommandParameterChanges Attached Property

    }
}
