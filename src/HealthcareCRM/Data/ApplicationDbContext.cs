using Microsoft.EntityFrameworkCore;
using HealthcareCRM.Models; // Model ka namespace add kiya

namespace HealthcareCRM.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // Patient Table database me create karne ke liye DbSet add kiya
        public DbSet<Patient> Patients { get; set; }
    }
}