using Microsoft.EntityFrameworkCore;
using P01_StudentSystem.Models;

namespace P01_StudentSystem.Data;

public static class ModelBuilderExtensions
{
    public static void Seed(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Student>().HasData(
            new Student
            {
                StudentId = 1,
                Name = "Mariam Hassan",
                PhoneNumber = "0101234567",
                RegisteredOn = new DateTime(2024, 1, 10),
                Birthday = new DateTime(2001, 5, 17)
            },
            new Student
            {
                StudentId = 2,
                Name = "Omar Ali",
                PhoneNumber = null,
                RegisteredOn = new DateTime(2024, 2, 5),
                Birthday = new DateTime(2000, 11, 3)
            },
            new Student
            {
                StudentId = 3,
                Name = "Nour Ahmed",
                PhoneNumber = "0117654321",
                RegisteredOn = new DateTime(2024, 3, 1),
                Birthday = null
            });

        modelBuilder.Entity<Course>().HasData(
            new Course
            {
                CourseId = 1,
                Name = "Entity Framework Core",
                Description = "Build data-driven .NET applications with EF Core.",
                StartDate = new DateTime(2024, 4, 1),
                EndDate = new DateTime(2024, 5, 15),
                Price = 1500m
            },
            new Course
            {
                CourseId = 2,
                Name = "C# Fundamentals",
                Description = "Learn the fundamentals of C# and object-oriented programming.",
                StartDate = new DateTime(2024, 3, 10),
                EndDate = new DateTime(2024, 4, 20),
                Price = 1200m
            });

        modelBuilder.Entity<Resource>().HasData(
            new Resource
            {
                ResourceId = 1,
                Name = "EF Core Documentation",
                Url = "https://learn.microsoft.com/ef/core/",
                ResourceType = ResourceType.Document,
                CourseId = 1
            },
            new Resource
            {
                ResourceId = 2,
                Name = "Code First Slides",
                Url = "https://example.com/resources/code-first-slides",
                ResourceType = ResourceType.Presentation,
                CourseId = 1
            },
            new Resource
            {
                ResourceId = 3,
                Name = "C# Classes Video",
                Url = "https://example.com/resources/csharp-classes",
                ResourceType = ResourceType.Video,
                CourseId = 2
            });

        modelBuilder.Entity<StudentCourse>().HasData(
            new StudentCourse { StudentId = 1, CourseId = 1 },
            new StudentCourse { StudentId = 1, CourseId = 2 },
            new StudentCourse { StudentId = 2, CourseId = 1 },
            new StudentCourse { StudentId = 3, CourseId = 2 });

        modelBuilder.Entity<Homework>().HasData(
            new Homework
            {
                HomeworkId = 1,
                Content = "submissions/mariam/ef-core-console-app.zip",
                ContentType = ContentType.Zip,
                SubmissionTime = new DateTime(2024, 4, 15, 18, 30, 0),
                StudentId = 1,
                CourseId = 1
            },
            new Homework
            {
                HomeworkId = 2,
                Content = "submissions/omar/ef-model.pdf",
                ContentType = ContentType.Pdf,
                SubmissionTime = new DateTime(2024, 4, 16, 10, 0, 0),
                StudentId = 2,
                CourseId = 1
            },
            new Homework
            {
                HomeworkId = 3,
                Content = "submissions/nour/csharp-basics.exe",
                ContentType = ContentType.Application,
                SubmissionTime = new DateTime(2024, 3, 28, 14, 45, 0),
                StudentId = 3,
                CourseId = 2
            });
    }
}
