using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Debug;
using CommunityToolkit.Mvvm.DependencyInjection;
using CommunityToolkit.Mvvm.Messaging;
using MvvmDemo.Services;
using MvvmDemo.ViewModels;
using MvvmDemoWPF.Recipients;
using System.Net.Http;
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
               .AddSingleton<HttpClient>()
               .AddSingleton<IMessenger>(messenger)
               .AddSingleton<IEmployeeRepository, EmployeeRepository>()
               .AddSingleton<MainViewModel>()
               .BuildServiceProvider());

            messenger.Register(AsyncYesNoMessageRecipient.Current);

            MainViewModel.Current.ErrorOccurred += Current_ErrorOccurred;
        }

        private void Current_ErrorOccurred(object sender, MvvmDemo.Messages.ErrorOccuredEventArgs e) {
            MessageBox.Show(e.Message);
        }
    }
}
