using BCU.Contoso.Timetable.Entities.Api.Responses;
using System.ComponentModel.DataAnnotations;

namespace BCU.Contoso.Timetable.Web.Components.Pages.Events;

public class EventSearch
{
    [Key]
    public int Id { get; set; }

    [MaxLength(100)]
    public required string Name { get; set; }

    [MaxLength(500)]
    public required string Description { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }

    [MaxLength(100)]
    public string? Location { get; set; }
}