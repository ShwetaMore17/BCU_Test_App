using BCU.Contoso.Timetable.Database.Configurations;
using BCU.Contoso.Timetable.Database.DataTables;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System.Threading;

namespace BCU.Contoso.Timetable.Database;

public class TimetableDBContext(DbContextOptions<TimetableDBContext> options) : DbContext(options)
{
    public DbSet<Student> Students { get; set; }
    public DbSet<Event> Events { get; set; }
    public DbSet<Course> Courses { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(StudentConfiguration).Assembly);
    }
}
