using Microsoft.Toolkit.Mvvm.ComponentModel;

namespace MvvmDemo.Models {
    public class Employee : ObservableObject {

        private string _name;
        private double _salary;

        public Employee() {
        }

        public Employee(string name, double salary) {
            Name = name;
            Salary = salary;
        }

        public string Name {
            get { return _name; }
            set {
                //if (_name != value) {
                //    _name = value;
                //    OnPropertyChanged(nameof(Name));
                //}
                SetProperty(ref _name, value);
            }
        }
        public double Salary {
            get { return _salary; }
            set {
                //if (_salary != value) {
                //    _salary = value;
                //    OnPropertyChanged(nameof(Salary));
                //}
                SetProperty(ref _salary, value);
            }
        }

    }
}
