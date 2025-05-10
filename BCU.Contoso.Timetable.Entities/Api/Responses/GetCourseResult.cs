using System.ComponentModel.DataAnnotations;

namespace BCU.Contoso.Timetable.Entities.Api.Responses;

public class GetCourseResult
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
    public virtual List<GetCourseResultStudent> EnrolledStudents { get; set; } = [];
    public virtual List<GetCourseResultEvent> Events { get; set; } = [];
}

public class GetCourseResultStudent
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

    public required DateTime EnrollmentDate { get; set; }

}

public class GetCourseResultEvent
{
    [Key]
    public int Id { get; set; }

    [MaxLength(100)]
    public required string Name { get; set; }

    [MaxLength(500)]
    public required string Description { get; set; }
    public required DateTime StartDate { get; set; }
    public required DateTime EndDate { get; set; }

    [MaxLength(100)]
    public required string Location { get; set; }

}