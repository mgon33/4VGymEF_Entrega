using _4VGymEF.Models;
using Microsoft.EntityFrameworkCore;

namespace _4VGymEF.Data
{
    public class GymDbContext : DbContext
    {
        public GymDbContext(DbContextOptions<GymDbContext> options)
            : base(options)
        {
        }
        // Indicamos las clases de modelo para mapearlas a tablas de SQL Server
        public DbSet<Activity> Activities { get; set; }
        // Añade el resto (piensa cuales debes poner para que se conviertan en tablas)
        public DbSet<ActivityType> ActivityTypes { get; set; }
        public DbSet<Instructor> Instructors { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            // La migración creó la tabla como "Intructors": mantenemos ese nombre
            modelBuilder.Entity<Instructor>().ToTable("Intructors");
        }
    }

}
