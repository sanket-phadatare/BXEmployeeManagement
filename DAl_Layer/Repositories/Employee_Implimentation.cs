using DAl_Layer;
using DAl_Layer.DBContexct;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Business_Layer.Repositories
{
    public class Employee_Implimentation : IEmployee_
    {
        private readonly EmployeeDbContext _context;

        public Employee_Implimentation(EmployeeDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Employee>> GetAllAsync()
        {
            return await _context.Employees.ToListAsync();
        }

        public async Task<Employee> GetByIdAsync(int id)
        {
            return await _context.Employees.FindAsync(id);
        }

        public async Task AddAsync(Employee employee)
        {
            await _context.Employees.AddAsync(employee);
            await _context.SaveChangesAsync();
        }
        public async Task<bool> UpdateAsync(Employee employee)
        {
            var existingEmployee = await _context.Employees.Where(e=> e.Id == employee.Id).FirstAsync();

            // Update only the fields that changed
            existingEmployee.Name = employee.Name;
            existingEmployee.Designation = employee.Designation;
            existingEmployee.DateOfJoining = employee.DateOfJoining;
            existingEmployee.Salary = employee.Salary;
            existingEmployee.Gender = employee.Gender;
            existingEmployee.State = employee.State;

            existingEmployee.DateOfBirth = employee.DateOfBirth; // 👈 add this
            existingEmployee.Age = employee.Age;

            await _context.SaveChangesAsync();
            return true;
        }


        public async Task DeleteAsync(int id)
        {
            var employee = await _context.Employees.FindAsync(id);
            if (employee != null)
            {
                _context.Employees.Remove(employee);
                await _context.SaveChangesAsync();
            }
        }

        public async Task DeleteMultipleAsync(List<int> ids)
        {
            var employees = _context.Employees.Where(e => ids.Contains(e.Id));
            _context.Employees.RemoveRange(employees);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> IsDuplicateAsync(string name)
        {
            return await _context.Employees.AnyAsync(e => e.Name == name);
        }
    }
}
