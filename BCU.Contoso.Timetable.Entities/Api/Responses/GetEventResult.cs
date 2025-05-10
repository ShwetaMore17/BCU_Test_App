using System.ComponentModel.DataAnnotations;

namespace BCU.Contoso.Timetable.Entities.Api.Responses;

public class GetEventResult
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
    public virtual GetEventResultCourse? Course { get; set; }
}

public class GetEventResultCourse
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