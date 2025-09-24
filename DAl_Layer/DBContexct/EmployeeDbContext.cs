using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAl_Layer.DBContexct
{
    public class EmployeeDbContext  : DbContext
    {
        public EmployeeDbContext(DbContextOptions options) : base(options)
        {
        }

       public DbSet<Employee> Employees { get; set; } 
    }
}
