using MvvmDemo.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace MvvmDemo.Services {
    public class EmployeeRepository : IEmployeeRepository {

        private readonly HttpClient _httpClient;

        public EmployeeRepository(HttpClient httpClient) {
            _httpClient = httpClient;
        }

        public async Task<IEnumerable<Employee>> ReadAsync() {
            string json = await _httpClient.GetStringAsync(new Uri("https://www.reflectionit.nl/api/employees?Count=10"));
            var list = JsonConvert.DeserializeObject<List<Employee>>(json);
            return list ?? Enumerable.Empty<Employee>();
        }
    }

}
