using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCU.Contoso.Timetable.Database.DataTables;
public class Student
{
    [Key]
    public int Id { get; set; }

    [MaxLength(100)]
    public required string Name { get; set; }

    [MaxLength(100)]
    [EmailAddress]
    public required string Email { get; set; }

    [MaxLength(600)]
    public required string Address { get; set; }

    [MaxLength(50)]
    public required string PhoneNumber { get; set; }

    public virtual ICollection<Course> EnrolledCourses { get; set; } = [];
    public required DateTime EnrollmentDate { get; set; }
}
