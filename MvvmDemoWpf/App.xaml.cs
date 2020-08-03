using Microsoft.Extensions.DependencyInjection;
using Microsoft.Toolkit.Mvvm.DependencyInjection;
using Microsoft.Toolkit.Mvvm.Messaging;
using MvvmDemo.Messages;
using MvvmDemo.Services;
using MvvmDemo.ViewModels;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace MvvmDemoWpf {
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application {

        public App() {
            Ioc.Default.ConfigureServices(services => {
                services.AddSingleton<ILogger, DebugLogger>();
                services.AddSingleton<MainViewModel>();
            });

            Messenger.Default.Register<AsyncYesNoMessage>(this, m => {

                Task<bool> GetResult() {
                    var result = MessageBox.Show(m.Text, "Confirm", MessageBoxButton.YesNo );
                    return Task.FromResult(result == MessageBoxResult.Yes);
                }

                m.Reply(GetResult());
            });
        }

    }
}
