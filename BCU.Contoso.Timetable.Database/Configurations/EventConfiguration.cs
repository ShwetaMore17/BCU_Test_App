using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BCU.Contoso.Timetable.Database.DataTables;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BCU.Contoso.Timetable.Database.Configurations;
internal class EventConfiguration : IEntityTypeConfiguration<Event>
{
    public void Configure(EntityTypeBuilder<Event> builder)
    {
        builder
            .ToTable("Events")
            .HasOne(e => e.Course)
            .WithMany(c => c.Events)
            .HasForeignKey(e => e.CourseId)
            .HasPrincipalKey(c => c.Id);
    }
}
