using EFCore03.Enums;

namespace EFCore03.Entities;

public class Course
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public decimal Price { get; set; }
    public CourseLevel Level { get; set; }
    public bool IsPublished { get; set; }
    public DateTime? PublishedAt { get; set; } 

    public int InstructorId { get; set; }
    public Instructor? Instructor { get; set; }

    public int CategoryId { get; set; }
    public Category? Category { get; set; }

    public ICollection<Enrollment> Enrollments { get; set; } = new List<Enrollment>();
}
