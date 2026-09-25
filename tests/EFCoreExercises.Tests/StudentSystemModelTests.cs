using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using P01_StudentSystem.Data;
using P01_StudentSystem.Models;

namespace EFCoreExercises.Tests;

public sealed class StudentSystemModelTests
{
    private static readonly StudentSystemContext Context = CreateContext();
    private static readonly IModel Model = Context.GetService<IDesignTimeModel>().Model;

    [Fact]
    public void Student_properties_match_the_exercise_constraints()
    {
        var student = Model.FindEntityType(typeof(Student));

        Assert.NotNull(student);
        Assert.Equal(100, student.FindProperty(nameof(Student.Name))!.GetMaxLength());

        var phoneNumber = student.FindProperty(nameof(Student.PhoneNumber))!;
        Assert.Equal(10, phoneNumber.GetMaxLength());
        Assert.False(phoneNumber.IsUnicode());
        Assert.True(phoneNumber.IsFixedLength());
        Assert.True(phoneNumber.IsNullable);

        Assert.True(student.FindProperty(nameof(Student.Birthday))!.IsNullable);
    }

    [Fact]
    public void Student_course_join_has_a_composite_key_and_required_relationships()
    {
        var enrollment = Model.FindEntityType(typeof(StudentCourse));

        Assert.NotNull(enrollment);
        Assert.Equal(
            [nameof(StudentCourse.StudentId), nameof(StudentCourse.CourseId)],
            enrollment.FindPrimaryKey()!.Properties.Select(property => property.Name));
        Assert.Equal(2, enrollment.GetForeignKeys().Count());
        Assert.All(enrollment.GetForeignKeys(), foreignKey => Assert.True(foreignKey.IsRequired));
    }

    [Fact]
    public void Course_resource_and_homework_mappings_match_the_exercise()
    {
        var course = Model.FindEntityType(typeof(Course))!;
        var resource = Model.FindEntityType(typeof(Resource))!;
        var homework = Model.FindEntityType(typeof(Homework))!;

        Assert.Equal(80, course.FindProperty(nameof(Course.Name))!.GetMaxLength());
        Assert.True(course.FindProperty(nameof(Course.Description))!.IsNullable);
        Assert.Equal(50, resource.FindProperty(nameof(Resource.Name))!.GetMaxLength());
        Assert.False(resource.FindProperty(nameof(Resource.Url))!.IsUnicode());
        Assert.False(homework.FindProperty(nameof(Homework.Content))!.IsUnicode());
        Assert.Equal("HomeworkSubmissions", homework.GetTableName());
        Assert.Equal(2, homework.GetForeignKeys().Count());
    }

    [Fact]
    public void Seed_data_covers_every_required_student_system_entity()
    {
        Assert.NotEmpty(Model.FindEntityType(typeof(Student))!.GetSeedData());
        Assert.NotEmpty(Model.FindEntityType(typeof(Course))!.GetSeedData());
        Assert.NotEmpty(Model.FindEntityType(typeof(Resource))!.GetSeedData());
        Assert.NotEmpty(Model.FindEntityType(typeof(StudentCourse))!.GetSeedData());
        Assert.NotEmpty(Model.FindEntityType(typeof(Homework))!.GetSeedData());
    }

    private static StudentSystemContext CreateContext()
    {
        var options = new DbContextOptionsBuilder<StudentSystemContext>()
            .UseSqlServer(@"Server=(localdb)\MSSQLLocalDB;Database=StudentSystemModelTests;Trusted_Connection=True;TrustServerCertificate=True")
            .Options;

        return new StudentSystemContext(options);
    }
}
