using MvvmDemo.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MvvmDemoWpf.Views.Windows {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {

        public MainWindow() {
            InitializeComponent();
        }

        private void ListViewEmployees_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            // Replaced by the MvvmHelper.NotifyCommandParameterChanges attached property
            //var vm = MainViewModel.Current;
            //vm.RaiseSalaryCommand.NotifyCanExecuteChanged();
            //vm.DeleteCommand.NotifyCanExecuteChanged();
        }
    }
}
