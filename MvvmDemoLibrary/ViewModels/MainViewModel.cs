using Microsoft.Extensions.Logging;
using CommunityToolkit.Mvvm.DependencyInjection;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using MvvmDemo.Messages;
using MvvmDemo.Models;
using MvvmDemo.Services;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace MvvmDemo.ViewModels {

    public class MainViewModel {

        public static MainViewModel Current {
            get {
                try {
                    return Ioc.Default.GetService<MainViewModel>() ?? new MainViewModel();
                } catch {
                    // Design mode!
                    return new MainViewModel();
                }
            }
        }

        private readonly ILogger<MainViewModel> _logger;
        private readonly IMessenger _messenger;
        private readonly IEmployeeRepository _repository;

        public event ErrorOccuredEventHandler? ErrorOccurred;

        protected virtual void OnErrorOccurred(string message) {
            ErrorOccurred?.Invoke(this, new ErrorOccuredEventArgs(message));
        }

        private ObservableCollection<Employee> EmployeesCollection { get; } = new ObservableCollection<Employee>();
        public ReadOnlyObservableCollection<Employee> Employees { get; }

        public RelayCommand AddCommand { get; }
        public AsyncRelayCommand<Employee> DeleteCommand { get; }
        public AsyncRelayCommand LoadCommand { get; }
        public RelayCommand<Employee> RaiseSalaryCommand { get; }

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public MainViewModel() {
            EmployeesCollection.Add(new Employee("Fons", 2000));
            EmployeesCollection.Add(new Employee("Jim", 4000));
            EmployeesCollection.Add(new Employee("Ellen", 3000));
        }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

        public MainViewModel(ILogger<MainViewModel> logger, IMessenger messenger, IEmployeeRepository repository) : this() {
            _logger = logger;
            _messenger = messenger;
            _repository = repository;

            Employees = new ReadOnlyObservableCollection<Employee>(EmployeesCollection);

            AddCommand = new RelayCommand(Add);
            RaiseSalaryCommand = new RelayCommand<Employee>(RaiseSalary, emp => emp is not null && emp.Salary < 3000);
            DeleteCommand = new AsyncRelayCommand<Employee>(Delete, emp => emp is not null);

            LoadCommand = new AsyncRelayCommand(LoadAsync, () => !LoadCommand.IsRunning);
            LoadCommand.PropertyChanged += LoadCommand_PropertyChanged;
        }

        private async Task Delete(Employee? emp) {
            if (emp is not null) {
                if (await _messenger.Send(new AsyncYesNoMessage($"Delete {emp.Name}?"))) {
                    _logger.LogInformation($"Delete: {emp.Name}");
                    EmployeesCollection.Remove(emp);
                }
            }
        }

        private void LoadCommand_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e) {
            if (e.PropertyName == nameof(LoadCommand.IsRunning)) {
                LoadCommand.NotifyCanExecuteChanged();
            }
        }

        private async Task LoadAsync() {
            try {
                EmployeesCollection.Clear();
                var list = await _repository.ReadAsync();
                foreach (var item in list) {
                    EmployeesCollection.Add(item);
                }
            } catch (Exception ex) {
                OnErrorOccurred(ex.Message);
            }
        }
        

        private void RaiseSalary(Employee? emp) {
            if (emp is not null) {
                _logger.LogInformation($"RaiseSalary: {emp.Name}");
                emp.Salary += 100;
                RaiseSalaryCommand.NotifyCanExecuteChanged();
            }
        }

        private void Add() => EmployeesCollection.Insert(0, new Employee("?", 1000));

    }
}
