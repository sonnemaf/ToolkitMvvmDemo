using Microsoft.Extensions.DependencyInjection;
using Microsoft.Toolkit.Mvvm.DependencyInjection;
using Microsoft.Toolkit.Mvvm.Messaging;
using MvvmDemo.Messages;
using MvvmDemo.Services;
using MvvmDemo.ViewModels;
using MvvmDemoWPF.Recipients;
using System.Windows;

namespace MvvmDemoWpf {
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application {

        public App() {

            var messenger = WeakReferenceMessenger.Default;

            Ioc.Default.ConfigureServices(new ServiceCollection()
               .AddSingleton<IMessenger>(messenger)
               .AddSingleton<ILogger, DebugLogger>()
               .AddSingleton<MainViewModel>()
               .BuildServiceProvider());

            messenger.Register(new AsyncYesNoMessageRecipient());
        }

        
    }
}
