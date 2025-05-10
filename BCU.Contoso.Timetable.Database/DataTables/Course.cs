using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCU.Contoso.Timetable.Database.DataTables;
public class Course
{
    [Key]
    public int Id { get; set; }

    [MaxLength(100)]
    public required string Name { get; set; }

    [MaxLength(500)]
    public required string Description { get; set; }

    [MaxLength(50)]
    public required string Code { get; set; }

    public required DateTime StartDate { get; set; }
    public required DateTime EndDate { get; set; }

    public virtual ICollection<Student> EnrolledStudents { get; set; } = [];
    public virtual ICollection<Event> Events { get; set; } = [];
}
