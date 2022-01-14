using Microsoft.Toolkit.Mvvm.Input;
using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Documents;
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
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0039:Use local function", Justification = "<Pending>")]
        private static void OnNotifyCommandParameterChangesChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) {
            if (d is FrameworkElement frameworkElement) {
                DependencyPropertyDescriptor pd;

                switch (frameworkElement) {
                    case ButtonBase _:
                        pd = DependencyPropertyDescriptor.FromProperty(ButtonBase.CommandParameterProperty, typeof(ButtonBase));
                        break;
                    case MenuItem _:
                        pd = DependencyPropertyDescriptor.FromProperty(MenuItem.CommandParameterProperty, typeof(MenuItem));
                        break;
                    default:
                        return;
                }

                RoutedEventHandler unloaded = (s, e) => {
                    pd.RemoveValueChanged(frameworkElement, OnCommandParameterChanged);
                };

                if ((bool)e.NewValue) {
                    pd.AddValueChanged(frameworkElement, OnCommandParameterChanged);
                    frameworkElement.Unloaded += unloaded;
                } else {
                    pd.RemoveValueChanged(frameworkElement, OnCommandParameterChanged);
                    frameworkElement.Unloaded -= unloaded;
                }

            } else if (d is FrameworkContentElement frameworkContentElement) {
                DependencyPropertyDescriptor pd;

                switch (frameworkContentElement) {
                    case Hyperlink hl:
                        pd = DependencyPropertyDescriptor.FromProperty(Hyperlink.CommandParameterProperty, typeof(Hyperlink));
                        break;
                    default:
                        return;
                }

                RoutedEventHandler unloaded = (s, e) => {
                    pd.RemoveValueChanged(frameworkContentElement, OnCommandParameterChanged);
                };

                if ((bool)e.NewValue) {
                    pd.AddValueChanged(frameworkContentElement, OnCommandParameterChanged);
                    frameworkContentElement.Unloaded += unloaded;
                } else {
                    pd.RemoveValueChanged(frameworkContentElement, OnCommandParameterChanged);
                    frameworkContentElement.Unloaded -= unloaded;
                }

            }
        }

        private static void OnCommandParameterChanged(object sender, EventArgs e) {
            ((sender as ICommandSource).Command as IRelayCommand)?.NotifyCanExecuteChanged();
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
