using Microsoft.EntityFrameworkCore;
using P01_StudentSystem.Data;

var contextFactory = new StudentSystemContextFactory();
await using var context = contextFactory.CreateDbContext(args);

try
{
    await context.Database.MigrateAsync();

    var courses = await context.Courses
        .AsNoTracking()
        .Include(course => course.StudentsEnrolled)
        .ThenInclude(enrollment => enrollment.Student)
        .Include(course => course.Resources)
        .OrderBy(course => course.Name)
        .ToListAsync();

    Console.WriteLine("Courses and enrolled students");
    Console.WriteLine("=============================");

    foreach (var course in courses)
    {
        Console.WriteLine($"{course.Name} ({course.StartDate:d} - {course.EndDate:d}) | {course.Price:C}");

        foreach (var enrollment in course.StudentsEnrolled.OrderBy(item => item.Student.Name))
        {
            Console.WriteLine($"  - {enrollment.Student.Name}");
        }

        Console.WriteLine($"  Resources: {course.Resources.Count}");
        Console.WriteLine();
    }

    var students = await context.Students
        .AsNoTracking()
        .Include(student => student.CourseEnrollments)
        .ThenInclude(enrollment => enrollment.Course)
        .OrderBy(student => student.Name)
        .ToListAsync();

    Console.WriteLine("Students and courses");
    Console.WriteLine("====================");

    foreach (var student in students)
    {
        var courseNames = student.CourseEnrollments
            .Select(enrollment => enrollment.Course.Name)
            .OrderBy(name => name);

        Console.WriteLine($"{student.Name}: {string.Join(", ", courseNames)}");
    }
}
catch (Exception exception)
{
    Console.Error.WriteLine("The Student System database could not be opened.");
    Console.Error.WriteLine("Set STUDENT_SYSTEM_CONNECTION_STRING to a valid SQL Server connection string and try again.");
    Console.Error.WriteLine(exception.Message);
    Environment.ExitCode = 1;
}
