using MvvmDemo.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace MvvmDemo.Services {
    public class EmployeeRepository : IEmployeeRepository {

        public async Task<IEnumerable<Employee>> ReadAsync() {
            using (HttpClient wc = new HttpClient()) {
                string json = await wc.GetStringAsync(new Uri("http://www.reflectionit.nl/api/employees?Count=10"));
                return JsonConvert.DeserializeObject<List<Employee>>(json);
            }
        }
    }
    
}
