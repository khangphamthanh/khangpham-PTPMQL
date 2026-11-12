using DemoMVC.Models;
using Microsoft.EntityFrameworkCore;

namespace DemoMVC.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Student> Students { get; set; } = default!;

        public DbSet<Department> Departments { get; set; } = default!;

        public DbSet<Discipline> Disciplines { get; set; }
        public DbSet<FormOfTraining> FormOfTrainings { get; set; }
    }
}