using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCU.Contoso.Timetable.Database.DataTables;
public class Event
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

    public int? CourseId { get; set; }
    public virtual Course? Course { get; set; }
}
