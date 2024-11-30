using Microsoft.EntityFrameworkCore;

namespace Students.Models
{
    public class ApplicationDbConext:DbContext

    {
        public DbSet<Student> Students { get; set; } 
        public DbSet<Class> Classes { get; set; }
        public DbSet<StudentClass> StudentClasses { get; set; }
        public ApplicationDbConext(DbContextOptions<ApplicationDbConext> options):base(options) 
        { 

        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)

        {

            optionsBuilder.UseSqlServer("Write your database connection string");

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Student>()
                .HasIndex(s => s.PhoneNumber)
                .IsUnique();
            modelBuilder.Entity<Student>()
                .HasIndex(s=>s.EmailId)
                .IsUnique();
            modelBuilder.Entity<StudentClass>()
                .HasKey(sc => new { sc.StudentId, sc.ClassId });
            modelBuilder.Entity<StudentClass>()
                .HasOne(sc => sc.Student)
                .WithMany(s => s.StudentClasses)
                .HasForeignKey(sc => sc.StudentId);
            modelBuilder.Entity<StudentClass>()
                .HasOne(sc => sc.Class)
                .WithMany(c=>c.StudentClasses)
                .HasForeignKey(sc=>sc.ClassId); 

        }
       

    }
}
