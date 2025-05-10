using BCU.Contoso.Timetable.ApiService.Helpers;
using BCU.Contoso.Timetable.Database;
using BCU.Contoso.Timetable.Database.DataTables;
using BCU.Contoso.Timetable.Entities.Api.Requests;
using BCU.Contoso.Timetable.Entities.Api.Responses;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace BCU.Contoso.Timetable.ApiService.Controllers;

[ApiController]
[Route("course")]
public class CourseController(IRefitHelper refitHelper) : ControllerBase
{
    [HttpGet("page/{pageNumber}")]
    public async Task<PagedResult<GetCoursesResult>> GetCourses([FromServices] TimetableDBContext dbContext, int pageNumber, CourseSortEnum sortOptions, CancellationToken cancellationToken)
    {
        var query = dbContext.Courses.AsQueryable();

        switch (sortOptions)
        {
            case CourseSortEnum.NameAscending:
                query = query.OrderBy(q => q.Name);
                break;
            case CourseSortEnum.NameDescending:
                query = query.OrderByDescending(q => q.Name);
                break;
            case CourseSortEnum.CodeAscending:
                query = query.OrderBy(q => q.Code);
                break;
            case CourseSortEnum.CodeDescending:
                query = query.OrderByDescending(q => q.Code);
                break;
        }

        var courses = await query.Skip(pageNumber * 10).Take(9).ToListAsync(cancellationToken);

        return new PagedResult<GetCoursesResult>
        {
            TotalItems = await query.CountAsync(cancellationToken),
            Items = courses.Adapt<List<GetCoursesResult>>()
        };
    }

    [HttpGet("{courseId}")]
    public async Task<GetCourseResult> GetCourse([FromServices] TimetableDBContext dbContext, int courseId, CancellationToken cancellationToken)
    {
        var course = await dbContext.Courses
            .Include(s => s.EnrolledStudents)
            .Include(s => s.Events)
            .SingleOrDefaultAsync(s => s.Id == courseId, cancellationToken);

        if (course == null)
        {
            throw refitHelper.CreateException(HttpStatusCode.NotFound, "Course does not exist");
        }

        return course.Adapt<GetCourseResult>();
    }

    [HttpPut("{courseId}/name")]
    public async Task<GetCourseResult> UpdateCourseName([FromServices] TimetableDBContext dbContext, int courseId, string newName, CancellationToken cancellationToken)
    {
        var course = await dbContext.Courses.SingleOrDefaultAsync(s => s.Id == courseId, cancellationToken);

        if (course == null)
        {
            throw refitHelper.CreateException(HttpStatusCode.NotFound, "Course does not exist");
        }

        course.Name = newName;
        await dbContext.SaveChangesAsync(cancellationToken);

        return course.Adapt<GetCourseResult>();
    }
}
