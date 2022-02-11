using Microsoft.Extensions.Logging.Abstractions;
using CommunityToolkit.Mvvm.Messaging;
using MvvmDemo.Messages;
using MvvmDemo.Services;
using MvvmDemo.ViewModels;
using MvvmDemoTest.Recipients;
using MvvmDemoTest.Services;
using System;
using System.Threading.Tasks;
using Xunit;

namespace MvvmDemoTest {
    
    public class MainViewModelTest {

        private static MainViewModel CreateViewModel() {
            return new MainViewModel(
                NullLogger<MainViewModel>.Instance, 
                WeakReferenceMessenger.Default, 
                new FakeEmployeeRepository());
        }

        [Fact]
        public void RaiseSalary() {
            // Arrange
            var vm = CreateViewModel();
            var emp = vm.Employees[0];
            emp.Salary = 1000;

            // Act
            vm.RaiseSalaryCommand.Execute(emp);

            // Assert
            Assert.Equal(1100, emp.Salary);
        }

        [Fact]
        public async Task LoadAsync() {
            // Arrange
            var vm = CreateViewModel();

            // Act
            await vm.LoadCommand.ExecuteAsync(null);

            // Assert
            Assert.Equal(2, vm.Employees.Count);
        }


        [Fact]
        public void RaiseSalaryAboveLimit() {
            // Arrange
            var vm = CreateViewModel();
            var emp = vm.Employees[1];
            emp.Salary = 6000;
            var oldSal = emp.Salary;

            // Act
            vm.RaiseSalaryCommand.Execute(emp);

            // Assert
            Assert.Equal(oldSal, emp.Salary);
        }

        [Fact]
        public void DeleteYes() {
            // Arrange
            var vm = CreateViewModel();

            var yes = new FakeAsyncYesNoMessageRecipient(true);

            WeakReferenceMessenger.Default.Register(yes);

            // Act
            vm.DeleteCommand.Execute(vm.Employees[1]);

            WeakReferenceMessenger.Default.Unregister<AsyncYesNoMessage>(yes);

            // Assert
            Assert.Equal(2, vm.Employees.Count);
        }

        [Fact]
        public void DeleteNo() {
            // Arrange
            var vm = CreateViewModel();

            var no = new FakeAsyncYesNoMessageRecipient(false);

            WeakReferenceMessenger.Default.Register<AsyncYesNoMessage>(no);

            // Act
            vm.DeleteCommand.Execute(vm.Employees[1]);

            WeakReferenceMessenger.Default.Unregister<AsyncYesNoMessage>(no);

            // Assert
            Assert.Equal(3, vm.Employees.Count);
        }
    }
}
