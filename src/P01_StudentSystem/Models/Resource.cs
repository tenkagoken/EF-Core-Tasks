namespace P01_StudentSystem.Models;

public class Resource
{
    public int ResourceId { get; set; }

    public required string Name { get; set; }

    public required string Url { get; set; }

    public ResourceType ResourceType { get; set; }

    public int CourseId { get; set; }

    public Course Course { get; set; } = null!;
}
