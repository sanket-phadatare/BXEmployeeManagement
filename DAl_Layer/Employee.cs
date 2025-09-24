using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAl_Layer
{
    public class Employee
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Designation { get; set; }


        public DateTime DateOfJoining { get; set; } = DateTime.Today;

        public Decimal Salary { get; set; }

        public GenderType Gender { get; set; }

        public string State { get; set; }

        public DateTime DateOfBirth { get; set; }   // 👈 Add this
        public int Age { get; set; }
    }
}
