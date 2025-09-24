using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using WEB_Client_Frontend.ClientServices;
using WEB_Client_Frontend.Models;

namespace WEB_Client_Frontend.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly IHttpClientServices _httpClientServices;

        public EmployeeController(IHttpClientServices httpClientServices)
        {
            _httpClientServices = httpClientServices;
        }

        public async Task<IActionResult> Index(int page = 1)
        {
            // Fetch employees (e.g., from the database)
            var employees = await _httpClientServices.GetAsync<List<Employee>>("employee");

            // Pagination
            var totalEmployees = employees.Count;
            var totalPages = (int)Math.Ceiling((double)totalEmployees / 5);
            var employeesForPage = employees.Skip((page - 1) * 5).Take(5).ToList();

            // Pass data to the view
            ViewData["TotalEmployees"] = totalEmployees;
            ViewData["CurrentPage"] = page;
            ViewData["TotalPages"] = totalPages;

            return View(employeesForPage);
        }


        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Employee employee)
        {
            if (ModelState.IsValid)
            {
                // Calculate Age before sending it to the API
                employee.Age = CalculateAge(employee.DateOfBirth); // Calculate Age

                var response = await _httpClientServices.PostAsync<Employee>("employee", employee);

                return RedirectToAction("Index");
            }

            ModelState.AddModelError("", "Error in Adding Employee");
            return View(employee);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            if (id <= 0)
            {
                return NotFound();
            }

            // Fetch the employee from the database
            var employee = await _httpClientServices.GetAsync<Employee>($"employee/{id}");

            if (employee == null)
            {
                return NotFound();
            }

            return View(employee);
        }


        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var result = await _httpClientServices.DeleteAsync<bool>($"employee/{id}");

                if (result)
                {
                    return RedirectToAction("Index");
                }

                ModelState.AddModelError("", "Failed to delete the employee.");
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Error occurred while deleting employee.");
                return RedirectToAction("Index");
            }
        }


        [HttpPost]
        [ActionName("deleteSelected")]
        
        public async Task<IActionResult> DeleteSelected([FromBody] List<int> ids)
        {
            if (ids == null || !ids.Any())
            {
                TempData["Error"] = "No employees selected.";
                return RedirectToAction("Index");
            }

            try
            {
                var result = await _httpClientServices.DeleteWithBodyAsync<bool>("employee/deleteSelected", ids);
                if (!result)
                {
                    TempData["Error"] = "Failed to delete selected employees.";
                    return RedirectToAction("Index");
                }

                
                return RedirectToAction("Index");
            }
            catch (Exception)
            {
                TempData["Error"] = "Error occurred while deleting employees.";
                return RedirectToAction("Index");
            }
        }



        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            if (id <= 0)
                return NotFound();

            var employee = await _httpClientServices.GetAsync<Employee>($"employee/{id}");

            if (employee == null)
                return NotFound();

            return View(employee);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Employee employee)
        {
            if (ModelState.IsValid)
            {
                employee.Age = CalculateAge(employee.DateOfBirth);

                var updatedEmployee = await _httpClientServices.PutAsync<Employee>($"employee/{employee.Id}", employee);
                // Optionally do something with updatedEmployee
                return RedirectToAction("Index");
            }

            return View(employee);
        }





        // Helper method to calculate age based on DateOfBirth
        private int CalculateAge(DateTime dob)
        {
            var today = DateTime.Today;
            var age = today.Year - dob.Year;
            if (dob > today.AddYears(-age)) age--;
            return age;
        }
    }
}