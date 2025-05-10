using BCU.Contoso.Timetable.Entities.Api.Requests;
using BCU.Contoso.Timetable.Entities.Api.Responses;
using BCU.Contoso.Timetable.Web.Components.Pages.Events;
using Microsoft.AspNetCore.Mvc;
using MudBlazor;
using Refit;

namespace BCU.Contoso.Timetable.Web.Apis;

public interface ITimetableApi
{
    [Get("/course/page/{pageNumber}")]
    Task<PagedResult<GetCoursesResult>> GetCourses(int pageNumber, CourseSortEnum sortOptions, CancellationToken cancellationToken);

    [Get("/course/{courseId}")]
    Task<GetCourseResult> GetCourse(int courseId, CancellationToken cancellationToken);

    [Put("/course/{courseId}/name")]
    Task<GetCourseResult> UpdateCourseName(int courseId, string newName, CancellationToken cancellationToken);

    [Get("/event/page/{pageNumber}")]
    Task<PagedResult<EventSearch>> GetEvents(int pageNumber, EventSortEnum sortOptions, CancellationToken cancellationToken);

    [Get("/event/{eventId}")]
    Task<GetEventResult> GetEvent(int eventId, CancellationToken cancellationToken);

    [Put("/event/{eventId}/name")]
    Task<GetEventResult> UpdateEventLocation(int eventId, string newName, CancellationToken cancellationToken);

    [Get("/student/page/{pageNumber}")]
    Task<PagedResult<GetStudentsResult>> GetStudents(int pageNumber, StudentSortEnum sortOptions, CancellationToken cancellationToken);

    [Get("/student/{studentId}")]
    Task<GetStudentResult> GetStudent(int studentId, CancellationToken cancellationToken);

    [Put("/student/{studentId}/name")]
    Task<GetStudentResult> UpdateStudentName(int studentId, string newName, CancellationToken cancellationToken);
}
