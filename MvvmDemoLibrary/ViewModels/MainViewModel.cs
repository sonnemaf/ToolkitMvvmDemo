﻿using Microsoft.Extensions.DependencyInjection;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using Microsoft.Toolkit.Mvvm.Messaging;
using MvvmDemo.Messages;
using MvvmDemo.Models;
using MvvmDemo.Services;
using MvvmDemoLibrary.Services;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace MvvmDemo.ViewModels {

    public class MainViewModel {

        public static MainViewModel Current => Ioc.Default.GetService<MainViewModel>();

        private readonly ILogger _logger;
        private readonly IMessenger _messenger;

        public ObservableCollection<Employee> Employees { get; } = new ObservableCollection<Employee>();
        public RelayCommand<Employee> RaiseSalaryCommand { get; }
        public AsyncRelayCommand<Employee> DeleteCommand { get; }

        public MainViewModel(ILogger logger, IMessenger messenger) {
            _logger = logger;
            this._messenger = messenger;
            Employees.Add(new Employee { Name = "Fons", Salary = 2000 });
            Employees.Add(new Employee { Name = "Jim", Salary = 5000 });
            Employees.Add(new Employee { Name = "Ellen", Salary = 3000 });

            RaiseSalaryCommand = new RelayCommand<Employee>(OnRaiseSalary, emp => emp is object && emp.Salary < 5500);
            DeleteCommand = new AsyncRelayCommand<Employee>(OnDelete, emp => emp is object);
        }

        private async Task OnDelete(Employee emp) {
            if (await _messenger.Send(new AsyncYesNoMessage($"Delete {emp.Name}?"))) {
                _logger.Log($"Delete: {emp.Name}");
                Employees.Remove(emp);
            }
        }

        private void OnRaiseSalary(Employee emp) {
            _logger.Log($"OnRaiseSalary: {emp.Name}");
            emp.Salary += 100;
        }
    }


}
