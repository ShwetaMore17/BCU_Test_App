using BCU.Contoso.Timetable.Database.DataTables;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BCU.Contoso.Timetable.Database.Configurations;
internal class StudentConfiguration : IEntityTypeConfiguration<Student>
{
    public void Configure(EntityTypeBuilder<Student> builder)
    {
        builder
            .ToTable("Students")
            .HasMany(s => s.EnrolledCourses)
            .WithMany(c => c.EnrolledStudents);
    }
}
