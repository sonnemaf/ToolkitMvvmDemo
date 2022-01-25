using Microsoft.Toolkit.Mvvm.ComponentModel;

namespace MvvmDemo.Models {

    [ObservableObject]
    public partial class Employee { // Does not inherit from ObservableObject!!

        [ObservableProperty]
        private string? _name;

        [ObservableProperty]
        private double _salary;

        public Employee(string? name, double salary) {
            Name = name;
            Salary = salary;
        }
    }
}
