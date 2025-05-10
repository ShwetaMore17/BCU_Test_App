using System.ComponentModel.DataAnnotations;

namespace BCU.Contoso.Timetable.Entities.Api.Responses;

public class GetCoursesResult
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
}