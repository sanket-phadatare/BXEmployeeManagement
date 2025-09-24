using DAl_Layer;
using E_Business_Layer.Services;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BX_Employee_Management.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("allowCors")]
    public class EmployeeController : ControllerBase
    {
        private readonly IEmployee_Service _employeeService;

        public EmployeeController(IEmployee_Service employeeService)
        {
            _employeeService = employeeService;
        }

        // GET: /api/employees
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Employee>>> GetAll()
        {
            var employees = await _employeeService.GetAllEmployeesAsync();
            return Ok(employees);
        }

        // GET: /api/employees/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<Employee>> GetById(int id)
        {
            var employee = await _employeeService.GetEmployeeByIdAsync(id);
            if (employee == null) return NotFound();
            return Ok(employee);
        }

        // POST: /api/employees
        [HttpPost]
        public async Task<ActionResult> Create([FromBody] Employee employee)
        {
            if (await _employeeService.IsDuplicateEmployeeAsync(employee.Name))
            {
                return BadRequest("Duplicate employee name.");
            }

            employee.Age = CalculateAge(employee.DateOfBirth);
            await _employeeService.AddEmployeeAsync(employee);
            return CreatedAtAction(nameof(GetById), new { id = employee.Id }, employee);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] Employee employee)
        {
            if (id != null)
            {
                //return BadRequest("ID mismatch between URL and body.");

                employee.Age = CalculateAge(employee.DateOfBirth);
                var success = await _employeeService.UpdateEmployeeAsync(employee);
                if (success)
                    return Ok();
            }
            return NoContent();
        }


        // DELETE: /api/employees/{id}
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            await _employeeService.DeleteEmployeeAsync(id);
            return NoContent();
        }

        // DELETE: /api/employees (for multiple)
        [HttpDelete]
        public async Task<ActionResult> DeleteMultiple([FromBody] List<int> ids)
        {
            if (ids == null || !ids.Any())
            {
                return BadRequest("No employees selected for deletion.");
            }

            try
            {
                await _employeeService.DeleteEmployeesAsync(ids);
                if (ids != null)
                {
                    return Ok(new { success = true, message = "Selected employees deleted successfully." });
                }
                else
                {
                    return StatusCode(500, "Failed to delete selected employees.");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while deleting employees.");
            }
        
        }



        // GET: /api/states (when you add State table or for now hardcoded)
        [HttpGet("/api/states")]
        public ActionResult<IEnumerable<string>> GetStates()
        {
            var states = new List<string>();
            return Ok(states);
        }

        private int CalculateAge(DateTime dob)
        {
            var today = DateTime.Today;
            var age = today.Year - dob.Year;
            if (dob > today.AddYears(-age)) age--;
            return age;
        }

    }
}
