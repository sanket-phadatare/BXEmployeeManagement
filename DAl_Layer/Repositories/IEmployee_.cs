using DAl_Layer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Business_Layer.Repositories
{
    public interface IEmployee_
    {

        Task<IEnumerable<Employee>> GetAllAsync();
        Task<Employee> GetByIdAsync(int id);
        Task AddAsync(Employee employee);
        Task<bool> UpdateAsync(Employee employee);
        Task DeleteAsync(int id);
        Task DeleteMultipleAsync(List<int> ids);
        Task<bool> IsDuplicateAsync(string name);


    }
}
