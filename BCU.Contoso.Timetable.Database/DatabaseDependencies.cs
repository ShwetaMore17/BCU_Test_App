using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BCU.Contoso.Timetable.Database.DataTables;

namespace BCU.Contoso.Timetable.Database;
public static class DatabaseDependencies
{
    public static IHostApplicationBuilder AddDatabaseDependencies(this IHostApplicationBuilder builder)
    {
        builder
            .AddSqlServerDbContext<TimetableDBContext>("timetable");

        return builder;
    }

    public static async void SetupDatabase(DbContextOptions<TimetableDBContext> options)
    {
        var dbContext = new TimetableDBContext(options);
        var dbCreator = dbContext.GetService<IRelationalDatabaseCreator>();

        var strategy = dbContext.Database.CreateExecutionStrategy();
        strategy.Execute
        (
            () =>
            {
                // Create the database if it does not exist.
                // Do this first so there is then a database to start a transaction against.
                if (!dbCreator.Exists())
                {
                    dbCreator.Create();
                    dbCreator.CreateTables();
                }
            }
        );

        await dbContext.Database.MigrateAsync();

        var studentCheck = await dbContext.Set<Student>().FirstOrDefaultAsync();

        if (studentCheck != null)
        {
            return;
        }

        var students = DataSeeder.GetStudents();

        foreach (var studentChunk in students.Chunk(500))
        {
            await dbContext.Set<Student>().AddRangeAsync(studentChunk);
            await dbContext.SaveChangesAsync();
        }

        var courses = DataSeeder.GetCourses();
        await dbContext.Set<Course>().AddRangeAsync(courses);

        await dbContext.SaveChangesAsync();

        var events = DataSeeder.GetEvents();
        await dbContext.Set<Event>().AddRangeAsync(events);

        await dbContext.SaveChangesAsync();

        var studentCourseIds = students
            .SelectMany
            (
                s => courses
                    .OrderBy(_ => Guid.NewGuid())
                    .Take(new Random().Next(1, 5))
                    .Select(c => new { Id = (students.IndexOf(s) + 1), CourseId = courses.IndexOf(c) + 1 })
            )
            .ToList();

        foreach (var studentCourseId in studentCourseIds)
        {
            var course = await dbContext.Set<Course>().FirstAsync(c => c.Id == studentCourseId.CourseId);
            var student = await dbContext.Set<Student>().FirstAsync(s => s.Id == studentCourseId.Id);

            course!.EnrolledStudents.Add(student!);

            await dbContext.SaveChangesAsync();
        }

        var eventCourseIds = events
            .Select
            (
                e =>
                    new
                    {
                        Id = (events.IndexOf(e) + 1),
                        CourseId = courses.IndexOf(
                            courses
                            .OrderBy(_ => Guid.NewGuid())
                            .First()
                        ) + 1
                    }
            )
            .ToList();

        foreach (var eventCourseId in eventCourseIds)
        {
            var course = await dbContext.Set<Course>().FirstAsync(c => c.Id == eventCourseId.CourseId);
            var eventItem = await dbContext.Set<Event>().FirstAsync(e => e.Id == eventCourseId.Id);

            course!.Events.Add(eventItem!);

            await dbContext.SaveChangesAsync();
        }
    }
}
