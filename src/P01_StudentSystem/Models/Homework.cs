namespace P01_StudentSystem.Models;

public class Homework
{
    public int HomeworkId { get; set; }

    public required string Content { get; set; }

    public ContentType ContentType { get; set; }

    public DateTime SubmissionTime { get; set; }

    public int StudentId { get; set; }

    public Student Student { get; set; } = null!;

    public int CourseId { get; set; }

    public Course Course { get; set; } = null!;
}
