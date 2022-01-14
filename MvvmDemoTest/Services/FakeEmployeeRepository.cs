using MvvmDemo.Models;
using MvvmDemo.Services;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MvvmDemoTest.Services {

    public class FakeEmployeeRepository : IEmployeeRepository {

        public Task<IEnumerable<Employee>> ReadAsync() {
            var l = new List<Employee>() {
                new Employee("Emp 1", 1000),
                new Employee("Emp 2", 2000),
            };

            return Task.FromResult(l.AsEnumerable());
        }
    }

}
