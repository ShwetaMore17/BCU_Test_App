using System.Net;
using BCU.Contoso.Timetable.ApiService.Helpers;
using BCU.Contoso.Timetable.Database;
using BCU.Contoso.Timetable.Database.DataTables;
using BCU.Contoso.Timetable.Entities.Api.Requests;
using BCU.Contoso.Timetable.Entities.Api.Responses;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BCU.Contoso.Timetable.ApiService.Controllers;

[ApiController]
[Route("event")]
public class EventController(IRefitHelper refitHelper) : ControllerBase
{
    [HttpGet("page/{pageNumber}")]
    public async Task<PagedResult<GetEventsResult>> GetEvents([FromServices] TimetableDBContext dbContext, int pageNumber, EventSortEnum sortOptions, CancellationToken cancellationToken)
    {
        var query = dbContext.Events.AsQueryable();

        switch (sortOptions)
        {
            case EventSortEnum.NameAscending:
                query = query.OrderBy(q => q.Name);
                break;
            case EventSortEnum.NameDescending:
                query = query.OrderByDescending(q => q.Name);
                break;
            case EventSortEnum.LocationAscending:
                query = query.OrderBy(q => q.Location);
                break;
            case EventSortEnum.LocationDescending:
                query = query.OrderByDescending(q => q.Location);
                break;
        }

        var events = await query.Skip(pageNumber * 30).Take(29).ToListAsync(cancellationToken);

        return new PagedResult<GetEventsResult>
        {
            TotalItems = await query.CountAsync(cancellationToken),
            Items = events.Adapt<List<GetEventsResult>>()
        };
    }

    [HttpGet("{eventId}")]
    public async Task<GetEventResult> GetEvent([FromServices] TimetableDBContext dbContext, int eventId, CancellationToken cancellationToken)
    {
        var eventResult = await dbContext.Events
            .Include(s => s.Course)
            .SingleOrDefaultAsync(s => s.Id == eventId, cancellationToken);

        if (eventResult == null)
        {
            throw refitHelper.CreateException(HttpStatusCode.NotFound, "Event does not exist");
        }

        return eventResult.Adapt<GetEventResult>();
    }

    [HttpPut("{eventId}/name")]
    public async Task<GetEventResult> UpdateEventLocation([FromServices] TimetableDBContext dbContext, int eventId, string newName, CancellationToken cancellationToken)
    {
        var eventResult = await dbContext.Events.SingleOrDefaultAsync(s => s.Id == eventId, cancellationToken);

        if (eventResult == null)
        {
            throw refitHelper.CreateException(HttpStatusCode.NotFound, "Event does not exist");
        }

        eventResult.Location = newName;
        await dbContext.SaveChangesAsync(cancellationToken);

        return eventResult.Adapt<GetEventResult>();
    }
}
