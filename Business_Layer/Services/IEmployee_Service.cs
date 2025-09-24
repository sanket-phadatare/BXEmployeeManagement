using DAl_Layer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Business_Layer.Services
{
    public interface IEmployee_Service
    {

        Task<IEnumerable<Employee>> GetAllEmployeesAsync();
        Task<Employee> GetEmployeeByIdAsync(int id);
        Task AddEmployeeAsync(Employee employee);
        Task<bool> UpdateEmployeeAsync(Employee employee);
        Task DeleteEmployeeAsync(int id);
        Task DeleteEmployeesAsync(List<int> ids);
        Task<bool> IsDuplicateEmployeeAsync(string name);
    }
}
