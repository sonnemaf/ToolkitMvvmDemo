using Microsoft.Toolkit.Mvvm.Messaging;
using MvvmDemo.Messages;
using MvvmDemo.Services;
using MvvmDemo.ViewModels;
using System;
using Xunit;

namespace MvvmDemoTest {
    
    public class MainViewModelTest {

        [Fact]
        public void DeleteYes() {
            // Arrange
            var vm = new MainViewModel(new DebugLogger());

            Messenger.Default.Register<AsyncYesNoMessage>(this, m => {
                m.Reply(true);
            });

            // Act
            vm.DeleteCommand.Execute(vm.Employees[1]);

            Messenger.Default.Unregister<AsyncYesNoMessage>(this);

            // Assert
            Assert.Equal(2, vm.Employees.Count);
        }

        [Fact]
        public void RaiseSalary() {
            // Arrange
            var vm = new MainViewModel(new DebugLogger());
            var emp = vm.Employees[1];
            var oldSal = emp.Salary;

            // Act
            vm.RaiseSalaryCommand.Execute(emp);

            // Assert
            Assert.Equal(oldSal + 100, emp.Salary);
        }

        [Fact]
        public void RaiseSalaryAboveLimit() {
            // Arrange
            var vm = new MainViewModel(new DebugLogger());
            var emp = vm.Employees[1];
            emp.Salary = 6000;
            var oldSal = emp.Salary;

            // Act
            vm.RaiseSalaryCommand.Execute(emp);

            // Assert
            Assert.Equal(oldSal, emp.Salary);
        }

        [Fact]
        public void DeleteNo() {
            // Arrange
            var vm = new MainViewModel(new DebugLogger());

            Messenger.Default.Register<AsyncYesNoMessage>(this, m => {
                m.Reply(false);
            });

            // Act
            vm.DeleteCommand.Execute(vm.Employees[1]);

            Messenger.Default.Unregister<AsyncYesNoMessage>(this);

            // Assert
            Assert.Equal(3, vm.Employees.Count);
        }
    }
}
