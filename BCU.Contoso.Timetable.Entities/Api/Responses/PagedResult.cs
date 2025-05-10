using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCU.Contoso.Timetable.Entities.Api.Responses;
public class PagedResult<T>
{
    public required int TotalItems { get; set; }
    public List<T> Items { get; set; } = [];
}
