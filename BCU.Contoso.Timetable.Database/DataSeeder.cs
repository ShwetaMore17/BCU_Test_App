using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BCU.Contoso.Timetable.Database.DataTables;
using Bogus;

namespace BCU.Contoso.Timetable.Database;
public static class DataSeeder
{
    private static int GetAcademicYearStartYear(DateOnly? date = null)
    {
        var academicYearStart = 8;

        date ??= DateOnly.FromDateTime(DateTime.Now);

        var year = date.Value.Year;

        if (date.Value.Month < academicYearStart)
        {
            year--;
        }

        return year;
    }

    public static IList<Student> GetStudents(int count = 200)
    {
        var academicYearStart = GetAcademicYearStartYear();

        var fake = new Faker<Student>()
            .RuleFor(s => s.Name, f => f.Name.FullName())
            .RuleFor(s => s.Email, f => f.Internet.Email())
            .RuleFor(s => s.Address, f => f.Address.FullAddress())
            .RuleFor(s => s.PhoneNumber, f => f.Phone.PhoneNumber())
            .RuleFor(s => s.EnrollmentDate, f => f.Date.Between(new DateTime(academicYearStart, 8, 1), new DateTime(academicYearStart, 10, 1)));

        return fake.Generate(count);
    }

    public static IList<Course> GetCourses(int count = 50)
    {
        var fake = new Faker<Course>()
            .RuleFor(c => c.Name, f => f.Commerce.ProductName())
            .RuleFor(c => c.Description, f => f.Lorem.Paragraph(3))
            .RuleFor(c => c.Code, f => f.Commerce.Ean13())
            .RuleFor(c => c.StartDate, f => f.Date.Future(1))
            .RuleFor(c => c.EndDate, (f, c) => c.StartDate.AddDays(f.Random.Int(30, 90)));
        return fake.Generate(count);
    }

    public static IList<Event> GetEvents(int count = 300)
    {
        var academicYearStart = GetAcademicYearStartYear();

        var fake = new Faker<Event>()
            .RuleFor(e => e.Name, f => f.Commerce.ProductName())
            .RuleFor(e => e.Location, f => f.Address.StreetName())
            .RuleFor(e => e.Description, f => f.Lorem.Paragraph(3))
            .RuleFor(e => e.StartDate, f => f.Date.Between(new DateTime(academicYearStart, 8, 1), new DateTime(academicYearStart + 1, 7, 1)))
            .RuleFor(e => e.EndDate, (f, c) => c.StartDate.AddMinutes(f.Random.Int(30, 90)));
        return fake.Generate(count);
    }
}