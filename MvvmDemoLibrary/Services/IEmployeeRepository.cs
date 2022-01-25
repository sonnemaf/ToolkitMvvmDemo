using MvvmDemo.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MvvmDemo.Services {

    public interface IEmployeeRepository {
        Task<IEnumerable<Employee>> ReadAsync();
    }
    
}
