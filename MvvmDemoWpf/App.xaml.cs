using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Debug;
using Microsoft.Toolkit.Mvvm.DependencyInjection;
using Microsoft.Toolkit.Mvvm.Messaging;
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
               .AddLogging(builder => {
                   builder.AddDebug();
               })
               .AddSingleton<IMessenger>(messenger)
               .AddSingleton<IEmployeeRepository, EmployeeRepository>()
               .AddSingleton<MainViewModel>()
               .BuildServiceProvider());

            // TODO: Add Debug logger

            messenger.Register(AsyncYesNoMessageRecipient.Current);
        }

        
    }
}
