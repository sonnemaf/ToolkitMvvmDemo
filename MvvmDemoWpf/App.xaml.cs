using Microsoft.Extensions.DependencyInjection;
using Microsoft.Toolkit.Mvvm.Messaging;
using MvvmDemo.Messages;
using MvvmDemo.Services;
using MvvmDemo.ViewModels;
using MvvmDemoLibrary.Services;
using MvvmDemoWPF.Recipients;
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

            var messenger = WeakReferenceMessenger.Default;

            Ioc.Default = new ServiceCollection()
               .AddSingleton<IMessenger>(messenger)
               .AddSingleton<ILogger, DebugLogger>()
               .AddSingleton<MainViewModel>()
               .BuildServiceProvider();

            messenger.Register(new AsyncYesNoMessageRecipient());
        }

        
    }
}
