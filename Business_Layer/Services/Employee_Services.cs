using DAl_Layer;
using E_Business_Layer.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Business_Layer.Services
{
    public class Employee_Services : IEmployee_Service
    {
        private readonly IEmployee_ _employeeRepo;

        public Employee_Services(IEmployee_ employeeRepo)
        {
            _employeeRepo = employeeRepo;
        }

        public async Task<IEnumerable<Employee>> GetAllEmployeesAsync()
        {
            return await _employeeRepo.GetAllAsync();
        }

        public async Task<Employee> GetEmployeeByIdAsync(int id)
        {
            return await _employeeRepo.GetByIdAsync(id);
        }

        public async Task AddEmployeeAsync(Employee employee)
        {
            employee.Age = CalculateAge(employee.DateOfBirth); // 👈 add this
            await _employeeRepo.AddAsync(employee);
        }

        public async Task<bool> UpdateEmployeeAsync(Employee employee)
        {
            employee.Age = CalculateAge(employee.DateOfBirth); // 👈 add this
            return await _employeeRepo.UpdateAsync(employee);
        }

        public async Task DeleteEmployeeAsync(int id)
        {
            await _employeeRepo.DeleteAsync(id);
        }

        public async Task DeleteEmployeesAsync(List<int> ids)
        {
            await _employeeRepo.DeleteMultipleAsync(ids);
        }

        public async Task<bool> IsDuplicateEmployeeAsync(string name)
        {
            return await _employeeRepo.IsDuplicateAsync(name);
        }

        // ✅ Add this private method
        private int CalculateAge(DateTime dob)
        {
            var today = DateTime.Today;
            var age = today.Year - dob.Year;
            if (dob > today.AddYears(-age)) age--;
            return age;
        }
    }
}
