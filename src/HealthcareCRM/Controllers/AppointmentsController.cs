using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using HealthcareCRM.Data;
using HealthcareCRM.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace HealthcareCRM.Controllers
{
    public class AppointmentsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AppointmentsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // 1. GET: Appointments (Listing Page)
        public async Task<IActionResult> Index()
        {
            // Include user data so we can show patient names alongside appointments
            var appointments = await _context.Appointments
                .Include(a => a.Patient)
                .OrderByDescending(a => a.AppointmentDate)
                .ToListAsync();

            return View(appointments);
        }

        // 2. GET: Appointments/Create (Show Form)
        public async Task<IActionResult> Create()
        {
            var patients = await _context.Patients.ToListAsync();
            
            // Dropdown list populating for UI selecting fields
            ViewBag.PatientsList = new SelectList(patients, "Id", "FullName");
            return View();
        }

        // 3. POST: Appointments/Create (Save Form Data)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("PatientId,AppointmentDate,Reason,Status")] Appointment appointment)
        {
            if (ModelState.IsValid)
            {
                appointment.Id = Guid.NewGuid();
                appointment.CreatedAt = DateTime.UtcNow;
                
                _context.Add(appointment);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            // If layout fails, reload the dropdown list data securely
            var patients = await _context.Patients.ToListAsync();
            ViewBag.PatientsList = new SelectList(patients, "Id", "FullName");
            return View(appointment);
        }

        // 4. GET: Appointments/GetAppointmentJson/{id} (Official Document JSON Format Check)
        [HttpGet]
        public async Task<IActionResult> GetAppointmentJson(Guid id)
        {
            var appointment = await _context.Appointments
                .Include(a => a.Patient)
                .FirstOrDefaultAsync(a => a.Id == id);

            if (appointment == null)
            {
                return Json(new { success = false, data = (object)null!, message = "Appointment record not found." });
            }

            var responseData = new
            {
                appointmentId = appointment.Id,
                patientName = appointment.Patient?.FullName,
                date = appointment.AppointmentDate.ToString("yyyy-MM-dd HH:mm"),
                reason = appointment.Reason,
                status = appointment.Status
            };

            // Wrapped inside { success, data, message } strictly as mandated by guidelines
            return Json(new { success = true, data = responseData, message = "Appointment data fetched successfully." });
        }
    }
}