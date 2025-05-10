using BCU.Contoso.Timetable.Database.DataTables;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BCU.Contoso.Timetable.Database.Configurations;
internal class CourseConfiguration : IEntityTypeConfiguration<Course>
{
    public void Configure(EntityTypeBuilder<Course> builder)
    {
        var table = builder.ToTable("Courses");

        table
            .HasMany(c => c.EnrolledStudents)
            .WithMany(s => s.EnrolledCourses);

        table
            .HasMany(c => c.Events)
            .WithOne(e => e.Course)
            .HasForeignKey(e => e.CourseId)
            .HasPrincipalKey(c => c.Id);
    }
}
