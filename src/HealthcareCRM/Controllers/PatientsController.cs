using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HealthcareCRM.Data;
using HealthcareCRM.Models;
using System;
using System.Threading.Tasks;

namespace HealthcareCRM.Controllers
{
    public class PatientsController : Controller
    {
        private readonly ApplicationDbContext _context;

        // Constructor injection se database instance utha rahe hain
        public PatientsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // 1. GET: Patients (Listing aur Search both inside one view)
        public async Task<IActionResult> Index(string searchTerm)
        {
            var patientsQuery = _context.Patients.AsQueryable();

            // Agar user search bar me kuch likhta hai to filter apply hoga
            if (!string.IsNullOrEmpty(searchTerm))
            {
                patientsQuery = patientsQuery.Where(p => 
                    p.FullName.ToLower().Contains(searchTerm.ToLower()) || 
                    p.Email.ToLower().Contains(searchTerm.ToLower()));
            }

            var patientsList = await patientsQuery.OrderByDescending(p => p.CreatedAt).ToListAsync();
            
            // Search term ko wapas view me bhej rahe hain taake input field me text bacha rahe
            ViewBag.SearchTerm = searchTerm; 
            return View(patientsList);
        }

        // 2. GET: Patients/Create (Sirf Form Display Karne Ke Liye)
        public IActionResult Create()
        {
            return View();
        }

        // 3. POST: Patients/Create (Form Data Database Me Save Karne Ke Liye)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([FromForm] Patient patient)
        {
            if (ModelState.IsValid)
            {
                patient.Id = Guid.NewGuid();
                patient.CreatedAt = DateTime.UtcNow;

                _context.Add(patient);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(patient);
        }

        // 4. API Endpoint: GET /Patients/GetPatientJson/{id} (Document standard ke mutabiq Response shape check karne ke liye)
        [HttpGet]
        public async Task<JsonResult> GetPatientJson(Guid id)
        {
            var patient = await _context.Patients.FindAsync(id);
            if (patient == null)
            {
                return Json(ApiResponse<Patient>.CreateError("Patient not found."));
            }
            return Json(ApiResponse<Patient>.CreateSuccess(patient, "Patient data retrieved successfully."));
        }
    }
}