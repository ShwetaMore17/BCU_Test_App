using BCU.Contoso.Timetable.Database;
using BCU.Contoso.Timetable.Database.DataTables;
using BCU.Contoso.Timetable.Entities.Api.Requests;
using BCU.Contoso.Timetable.Entities.Api.Responses;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BCU.Contoso.Timetable.ApiService.Controllers;

[ApiController]
[Route("student")]
public class StudentController : ControllerBase
{
    [HttpGet("page/{pageNumber}")]
    public async Task<PagedResult<GetStudentsResult>> GetStudents([FromServices] TimetableDBContext dbContext, int pageNumber, StudentSortEnum sortOptions, CancellationToken cancellationToken)
    {
        var query = dbContext.Students.AsQueryable();

        switch (sortOptions)
        {
            case StudentSortEnum.NameAscending:
                query = query.OrderBy(q => q.Name);
                break;
            case StudentSortEnum.NameDescending:
                query = query.OrderByDescending(q => q.Name);
                break;
            case StudentSortEnum.EmailAscending:
                query = query.OrderBy(q => q.Email);
                break;
            case StudentSortEnum.EmailDescending:
                query = query.OrderBy(q => q.Email);
                break;
        }

        var students = await query.Skip(pageNumber * 50).Take(50).ToListAsync(cancellationToken);

        return new PagedResult<GetStudentsResult>
        {
            TotalItems = await query.CountAsync(cancellationToken),
            Items = students.Adapt<List<GetStudentsResult>>()
        };
    }

    [HttpGet("{studentId}")]
    public async Task<GetStudentResult> GetStudent([FromServices] TimetableDBContext dbContext, int studentId, CancellationToken cancellationToken)
    {
        var student = await dbContext.Students.Include(s => s.EnrolledCourses).SingleAsync(s => s.Id == studentId, cancellationToken);

        return student.Adapt<GetStudentResult>();
    }

    [HttpPut("{studentId}/name")]
    public async Task<GetStudentResult> UpdateStudentName([FromServices] TimetableDBContext dbContext, int studentId, string newName, CancellationToken cancellationToken)
    {
        var student = await dbContext.Students.SingleAsync(s => s.Id == studentId, cancellationToken);

        student.Name = newName;
        await dbContext.SaveChangesAsync(cancellationToken);

        return student.Adapt<GetStudentResult>();
    }
}
