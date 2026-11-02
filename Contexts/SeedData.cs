using EFCore03.Entities;
using EFCore03.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFCore03.Contexts
{
    public static class SeedData 
    {
        public static readonly Category[] Categories =
        {
            new() { Id = 1, Name = "Programming",        Description = "Everything about writing code", ParentCategoryId = null },
            new() { Id = 5, Name = "Design",             Description = null,                            ParentCategoryId = null },
            
            new() { Id = 2, Name = "Backend",            Description = "Server-side development",       ParentCategoryId = 1 },
            new() { Id = 4, Name = "Frontend",           Description = "Browser-side development",      ParentCategoryId = 1 },
            new() { Id = 7, Name = "Mobile Development", Description = "Apps for phones",               ParentCategoryId = 1 },

            new() { Id = 3, Name = "ASP.NET",            Description = "Web apps with .NET",            ParentCategoryId = 2 },
            new() { Id = 6, Name = "UI Design",          Description = null,                            ParentCategoryId = 5 },
        };

        public static readonly Instructor[] Instructors =
        {
            new() { Id = 1, FullName = "Ahmed Hassan", Email = "ahmed.hassan@example.com", Bio = "Senior .NET developer and trainer", JoinedAt = new DateTime(2023, 1, 10) },
            new() { Id = 2, FullName = "Sara Mostafa", Email = "sara.mostafa@example.com", Bio = null,                                 JoinedAt = new DateTime(2023, 6, 1) },
            new() { Id = 3, FullName = "Omar Khaled",  Email = "omar.khaled@example.com",  Bio = "UI/UX designer",                    JoinedAt = new DateTime(2024, 2, 20) },
        };

        public static readonly Course[] Courses =
        {
            new() { Id = 1, Title = "ASP.NET Core Fundamentals", Description = "Build your first web app", Price = 499.99m, Level = CourseLevel.Beginner,     IsPublished = true,  PublishedAt = new DateTime(2025, 1, 15), InstructorId = 1, CategoryId = 3 },
            new() { Id = 2, Title = "Advanced EF Core",          Description = null,                       Price = 799.00m, Level = CourseLevel.Advanced,     IsPublished = true,  PublishedAt = new DateTime(2025, 3, 1),  InstructorId = 1, CategoryId = 3 },
            new() { Id = 3, Title = "C# for Beginners",          Description = "Start from zero",          Price = 299.00m, Level = CourseLevel.Beginner,     IsPublished = true,  PublishedAt = new DateTime(2024, 11, 5), InstructorId = 2, CategoryId = 1 },
            new() { Id = 4, Title = "REST API Design",           Description = "Design clean APIs",        Price = 549.50m, Level = CourseLevel.Intermediate, IsPublished = false, PublishedAt = null,                      InstructorId = 2, CategoryId = 2 },
            new() { Id = 5, Title = "HTML & CSS Basics",         Description = null,                       Price = 199.00m, Level = CourseLevel.Beginner,     IsPublished = true,  PublishedAt = new DateTime(2025, 2, 10), InstructorId = 3, CategoryId = 4 },
            new() { Id = 6, Title = "UI Design Principles",      Description = "Layout, color, typography",Price = 399.00m, Level = CourseLevel.Intermediate, IsPublished = false, PublishedAt = null,                      InstructorId = 3, CategoryId = 6 },
        };
    }
}
