using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HealthcareCRM.Data;
using HealthcareCRM.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace HealthcareCRM.Controllers
{
    public class DoctorsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DoctorsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // 1. GET: Doctors (Listing Page with optional search filter)
        public async Task<IActionResult> Index(string searchString)
        {
            var doctors = from d in _context.Doctors
                          select d;

            if (!string.IsNullOrEmpty(searchString))
            {
                doctors = doctors.Where(s => s.Name.Contains(searchString) || s.Specialization.Contains(searchString));
            }

            return View(await doctors.OrderBy(d => d.Name).ToListAsync());
        }

        // 2. GET: Doctors/Create (Show Form)
        public IActionResult Create()
        {
            return View();
        }

        // 3. POST: Doctors/Create (Save Form Data)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,Specialization,Email,Phone,RoomNumber")] Doctor doctor)
        {
            if (ModelState.IsValid)
            {
                doctor.Id = Guid.NewGuid();
                doctor.CreatedAt = DateTime.UtcNow;

                _context.Add(doctor);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(doctor);
        }

        // 4. GET: Doctors/GetDoctorJson/{id} (Official Document JSON Format Check)
        [HttpGet]
        public async Task<IActionResult> GetDoctorJson(Guid id)
        {
            var doctor = await _context.Doctors.FindAsync(id);
            if (doctor == null)
            {
                return Json(new { success = false, data = (object)null!, message = "Doctor profile record not found." });
            }

            var responseData = new
            {
                doctorId = doctor.Id,
                name = doctor.Name,
                specialization = doctor.Specialization,
                email = doctor.Email,
                phone = doctor.Phone,
                room = doctor.RoomNumber
            };

            // Wrapped inside { success, data, message } strictly as mandated by guidelines
            return Json(new { success = true, data = responseData, message = "Doctor directory record fetched successfully." });
        }
    }
}