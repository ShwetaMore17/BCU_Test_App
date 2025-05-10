using System.ComponentModel.DataAnnotations;

namespace BCU.Contoso.Timetable.Entities.Api.Responses;

public class GetStudentsResult
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