using Microsoft.EntityFrameworkCore;
using P01_StudentSystem.Models;

namespace P01_StudentSystem.Data;

public class StudentSystemContext : DbContext
{
    public StudentSystemContext(DbContextOptions<StudentSystemContext> options)
        : base(options)
    {
    }

    public DbSet<Student> Students => Set<Student>();

    public DbSet<Course> Courses => Set<Course>();

    public DbSet<Resource> Resources => Set<Resource>();

    public DbSet<Homework> HomeworkSubmissions => Set<Homework>();

    public DbSet<StudentCourse> StudentCourses => Set<StudentCourse>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Student>(entity =>
        {
            entity.Property(student => student.Name)
                .HasMaxLength(100)
                .IsUnicode();

            entity.Property(student => student.PhoneNumber)
                .HasMaxLength(10)
                .IsFixedLength()
                .IsUnicode(false);
        });

        modelBuilder.Entity<Course>(entity =>
        {
            entity.Property(course => course.Name)
                .HasMaxLength(80)
                .IsUnicode();

            entity.Property(course => course.Description)
                .IsUnicode();

            entity.Property(course => course.Price)
                .HasPrecision(18, 2);
        });

        modelBuilder.Entity<Resource>(entity =>
        {
            entity.Property(resource => resource.Name)
                .HasMaxLength(50)
                .IsUnicode();

            entity.Property(resource => resource.Url)
                .IsUnicode(false);

            entity.HasOne(resource => resource.Course)
                .WithMany(course => course.Resources)
                .HasForeignKey(resource => resource.CourseId);
        });

        modelBuilder.Entity<Homework>(entity =>
        {
            entity.ToTable("HomeworkSubmissions");

            entity.Property(homework => homework.Content)
                .IsUnicode(false);

            entity.HasOne(homework => homework.Student)
                .WithMany(student => student.HomeworkSubmissions)
                .HasForeignKey(homework => homework.StudentId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(homework => homework.Course)
                .WithMany(course => course.HomeworkSubmissions)
                .HasForeignKey(homework => homework.CourseId);
        });

        modelBuilder.Entity<StudentCourse>(entity =>
        {
            entity.HasKey(enrollment => new { enrollment.StudentId, enrollment.CourseId });

            entity.HasOne(enrollment => enrollment.Student)
                .WithMany(student => student.CourseEnrollments)
                .HasForeignKey(enrollment => enrollment.StudentId);

            entity.HasOne(enrollment => enrollment.Course)
                .WithMany(course => course.StudentsEnrolled)
                .HasForeignKey(enrollment => enrollment.CourseId);
        });

        modelBuilder.Seed();
    }
}
