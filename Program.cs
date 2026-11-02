using EFCore03.Contexts;
using EFCore03.Entities;
using EFCore03.Enums;
using Microsoft.EntityFrameworkCore;

namespace EFCore03
{
    internal class Program
    {
        static void Main(string[] args)
        {
            #region Part 01: Model and Database

            //Part 01: Model and Database

            #endregion

            #region Part 02: Seeding

            //Part 02: Seeding

            #endregion

            #region Add separate Migration for the seed data

            //Add separate Migration for the seed data

            #endregion

            #region Part 03: Operations

            //Part 03: Operations



            // Apply the migrations (schema + HasData seed) at startup
            using (var ctx = new AppDbContext())
                ctx.Database.Migrate();

            SeedStudentsAndEnrollments();

            // ---------------- CREATE ----------------
            Header("CREATE");
            int? instructorId = AddInstructor("Mona Adel", "mona.adel@example.com", "Cloud and backend trainer");
            int? newCourseId = AddCourse(instructorId ?? 1, 2, "Building Web APIs", 650m, CourseLevel.Intermediate);
            AddCourse(99, 2, "Ghost Course", 100m, CourseLevel.Beginner);
            AddCourse(1, 99, "Ghost Course", 100m, CourseLevel.Beginner);
            AddStudents();
            EnrollStudent(4, 1);
            EnrollStudent(4, newCourseId ?? 0);
            EnrollStudent(1, 1);
            EnrollStudent(99, 1);

            // ---------------- READ ----------------
            Header("READ");
            FindCourse(1, "Advanced EF Core");
            FindCourse(999, "No Such Course");
            FilterCourses(200m, 600m);
            PrintSubCategories(1);
            PrintTopLevelCategories();
            CourseRoster(1);

            // ---------------- UPDATE ----------------
            Header("UPDATE");
            ChangePrice(1, 549.99m);
            ChangePrice(999, 100m);
            UpdateProgress(3, 2, 60);
            UpdateProgress(1, 1, 100);
            UpdateProgress(1, 1, 150);
            UpdateProgress(99, 99, 50);
            PublishCourse(newCourseId ?? 0);

            // ---------------- DELETE ----------------
            Header("DELETE");
            Unenroll(3, 2);
            Unenroll(3, 2);
            DeleteCategoryUnsafe(1);
            DeleteCategorySafe(1);
            DeleteCategorySafe(7);
            DeleteCourse(3);

            // ---------------- BULK ----------------
            Header("BULK OPERATIONS");
            BulkRaisePrice(CourseLevel.Beginner, 0.10m);
            BulkDeleteDropped();

            Console.WriteLine("\nDone.");
        }

        // =====================================================================
        // Seeding from code (only when the table is empty)
        // =====================================================================
        static void SeedStudentsAndEnrollments()
        {
            using var ctx = new AppDbContext();

            if (!ctx.Students.Any())
            {
                ctx.Students.AddRange(
                    new Student { FullName = "Youssef Ali", Email = "youssef.ali@example.com", DateOfBirth = new DateTime(2001, 4, 12) },
                    new Student { FullName = "Nour Ibrahim", Email = "nour.ibrahim@example.com", DateOfBirth = new DateTime(2000, 9, 3) },
                    new Student { FullName = "Hana Samir", Email = "hana.samir@example.com", DateOfBirth = new DateTime(2002, 1, 25) },
                    new Student { FullName = "Karim Fathy", Email = "karim.fathy@example.com", DateOfBirth = new DateTime(1999, 12, 8) },
                    new Student { FullName = "Mariam Tarek", Email = "mariam.tarek@example.com", DateOfBirth = new DateTime(2003, 6, 17) });
                ctx.SaveChanges();
                Console.WriteLine("Seeded 5 students.");
            }

            if (!ctx.Enrollments.Any())
            {
                var s = ctx.Students.OrderBy(x => x.Id).Select(x => x.Id).Take(5).ToList();
                if (s.Count < 5) return;

                static Enrollment E(int student, int course, int progress, EnrollmentStatus status, decimal? grade, DateTime date) =>
                    new() { StudentId = student, CourseId = course, Progress = progress, Status = status, FinalGrade = grade, EnrolledAt = date };

                ctx.Enrollments.AddRange(
                    E(s[0], 1, 40, EnrollmentStatus.Active, null, new DateTime(2025, 2, 1)),
                    E(s[0], 2, 100, EnrollmentStatus.Completed, 92.5m, new DateTime(2025, 3, 10)),
                    E(s[1], 1, 100, EnrollmentStatus.Completed, 88m, new DateTime(2025, 2, 5)),
                    E(s[1], 3, 65, EnrollmentStatus.Active, null, new DateTime(2025, 3, 20)),
                    E(s[2], 2, 20, EnrollmentStatus.Active, null, new DateTime(2025, 4, 2)),
                    E(s[2], 5, 15, EnrollmentStatus.Dropped, null, new DateTime(2025, 4, 8)),
                    E(s[3], 3, 10, EnrollmentStatus.Dropped, null, new DateTime(2025, 4, 15)),
                    E(s[4], 5, 100, EnrollmentStatus.Completed, 95m, new DateTime(2025, 5, 1)));
                ctx.SaveChanges();
                Console.WriteLine("Seeded 8 enrollments.");
            }
        }

        // =====================================================================
        // CREATE
        // =====================================================================
        static int? AddInstructor(string name, string email, string? bio)
        {
            using var ctx = new AppDbContext();
            Console.WriteLine("\n-- Add instructor");

            if (ctx.Instructors.Any(i => i.Email == email))
            {
                Console.WriteLine($"  An instructor with email {email} already exists.");
                return null;
            }

            var instructor = new Instructor { FullName = name, Email = email, Bio = bio, JoinedAt = DateTime.Today };
            ctx.Instructors.Add(instructor);
            ctx.SaveChanges();
            Console.WriteLine($"  Instructor added. Generated Id = {instructor.Id}");
            return instructor.Id;
        }

        static int? AddCourse(int instructorId, int categoryId, string title, decimal price, CourseLevel level)
        {
            using var ctx = new AppDbContext();
            Console.WriteLine($"\n-- Add course '{title}'");

            if (!ctx.Instructors.Any(i => i.Id == instructorId))
            {
                Console.WriteLine($"  Instructor {instructorId} does not exist.");
                return null;
            }
            if (!ctx.Categories.Any(c => c.Id == categoryId))
            {
                Console.WriteLine($"  Category {categoryId} does not exist.");
                return null;
            }
            if (price < 0)
            {
                Console.WriteLine("  Price cannot be negative.");
                return null;
            }

            var course = new Course
            {
                Title = title,
                Price = price,
                Level = level,
                InstructorId = instructorId,
                CategoryId = categoryId,
                IsPublished = false
            };
            ctx.Courses.Add(course);
            ctx.SaveChanges();
            Console.WriteLine($"  Course added. Generated Id = {course.Id}");
            return course.Id;
        }

        static void AddStudents()
        {
            using var ctx = new AppDbContext();
            Console.WriteLine("\n-- Add several students (one AddRange, one SaveChanges)");

            var students = new[]
            {
            new Student { FullName = "Laila Hassan", Email = "laila.hassan@example.com", DateOfBirth = new DateTime(2001, 7, 14) },
            new Student { FullName = "Tamer Said",   Email = "tamer.said@example.com",   DateOfBirth = new DateTime(2000, 3, 30) },
            new Student { FullName = "Dina Wael",    Email = "dina.wael@example.com",    DateOfBirth = new DateTime(2002, 11, 5) },
        };

            var emails = students.Select(s => s.Email).ToList();
            if (ctx.Students.Any(s => emails.Contains(s.Email)))
            {
                Console.WriteLine("  Some of these students already exist, nothing was added.");
                return;
            }

            ctx.Students.AddRange(students);
            ctx.SaveChanges();
            foreach (var s in students)
                Console.WriteLine($"  {s.FullName} -> Id = {s.Id}");
        }

        static void EnrollStudent(int studentId, int courseId)
        {
            using var ctx = new AppDbContext();
            Console.WriteLine($"\n-- Enroll student {studentId} in course {courseId}");

            if (!ctx.Students.Any(s => s.Id == studentId))
            {
                Console.WriteLine("  Refused: the student does not exist.");
                return;
            }

            var course = ctx.Courses.Find(courseId);
            if (course is null)
            {
                Console.WriteLine("  Refused: the course does not exist.");
                return;
            }
            if (!course.IsPublished)
            {
                Console.WriteLine("  Refused: the course is not published, so it does not accept enrollments.");
                return;
            }
            if (ctx.Enrollments.Any(e => e.StudentId == studentId && e.CourseId == courseId))
            {
                Console.WriteLine("  Refused: the student is already enrolled in this course.");
                return;
            }

            ctx.Enrollments.Add(new Enrollment
            {
                StudentId = studentId,
                CourseId = courseId,
                EnrolledAt = DateTime.Now,
                Progress = 0,
                Status = EnrollmentStatus.Active
            });
            ctx.SaveChanges();
            Console.WriteLine("  Enrollment created.");
        }

        // =====================================================================
        // READ
        // =====================================================================
        static void FindCourse(int id, string title)
        {
            using var ctx = new AppDbContext();
            Console.WriteLine($"\n-- Find course by Id ({id}) and by title ('{title}')");

            var byId = ctx.Courses.Find(id);
            Console.WriteLine(byId is null
                ? $"  Course with Id {id} not found."
                : $"  By Id: {byId.Id} - {byId.Title} ({byId.Level}, {byId.Price:C})");

            var byTitle = ctx.Courses.FirstOrDefault(c => c.Title == title);
            Console.WriteLine(byTitle is null
                ? $"  Course titled '{title}' not found."
                : $"  By title: {byTitle.Id} - {byTitle.Title} ({byTitle.Level}, {byTitle.Price:C})");
        }

        static void FilterCourses(decimal min, decimal max)
        {
            using var ctx = new AppDbContext();
            Console.WriteLine($"\n-- Courses priced between {min} and {max} (highest first)");

            var courses = ctx.Courses
                .Where(c => c.Price >= min && c.Price <= max)
                .OrderByDescending(c => c.Price)
                .Select(c => new { c.Title, c.Level, c.Price })
                .ToList();

            if (courses.Count == 0)
            {
                Console.WriteLine("  No courses in this price range.");
                return;
            }
            foreach (var c in courses)
                Console.WriteLine($"  {c.Title,-30} {c.Level,-13} {c.Price,8:F2}");
        }

        static void PrintSubCategories(int parentId)
        {
            using var ctx = new AppDbContext();
            Console.WriteLine($"\n-- Direct sub-categories of category {parentId}");

            var subs = ctx.Categories
                .Where(c => c.ParentCategoryId == parentId)
                .Select(c => c.Name)
                .ToList();

            if (subs.Count == 0) Console.WriteLine("  (none)");
            foreach (var name in subs) Console.WriteLine($"  {name}");
        }

        static void PrintTopLevelCategories()
        {
            using var ctx = new AppDbContext();
            Console.WriteLine("\n-- Top-level categories");

            var top = ctx.Categories
                .Where(c => c.ParentCategoryId == null)
                .Select(c => c.Name)
                .ToList();

            foreach (var name in top) Console.WriteLine($"  {name}");
        }

        static void CourseRoster(int courseId)
        {
            using var ctx = new AppDbContext();
            Console.WriteLine($"\n-- Roster of course {courseId} (no joins)");

            var studentIds = ctx.Enrollments
                .Where(e => e.CourseId == courseId)
                .Select(e => e.StudentId)
                .ToList();

            Console.WriteLine($"  Enrolled students: {studentIds.Count}");

            var names = ctx.Students
                .Where(s => studentIds.Contains(s.Id))
                .OrderBy(s => s.FullName)
                .Select(s => s.FullName)
                .ToList();

            foreach (var name in names) Console.WriteLine($"  - {name}");
        }

        // =====================================================================
        // UPDATE
        // =====================================================================
        static void ChangePrice(int courseId, decimal newPrice)
        {
            using var ctx = new AppDbContext();
            Console.WriteLine($"\n-- Change price of course {courseId} to {newPrice}");

            if (newPrice < 0)
            {
                Console.WriteLine("  Price cannot be negative.");
                return;
            }

            var course = ctx.Courses.Find(courseId);
            if (course is null)
            {
                Console.WriteLine("  Course not found.");
                return;
            }

            var old = course.Price;
            course.Price = newPrice;
            ctx.SaveChanges();
            Console.WriteLine($"  '{course.Title}': {old:F2} -> {course.Price:F2}");
        }

        static void UpdateProgress(int studentId, int courseId, int progress)
        {
            using var ctx = new AppDbContext();
            Console.WriteLine($"\n-- Update progress of student {studentId} in course {courseId} to {progress}");

            if (progress < 0 || progress > 100)
            {
                Console.WriteLine("  Rejected: progress must be between 0 and 100.");
                return;
            }

            var enrollment = ctx.Enrollments.Find(studentId, courseId);
            if (enrollment is null)
            {
                Console.WriteLine("  Enrollment not found.");
                return;
            }

            enrollment.Progress = progress;
            if (progress == 100)
                enrollment.Status = EnrollmentStatus.Completed;

            ctx.SaveChanges();
            Console.WriteLine($"  Progress = {enrollment.Progress}%, status = {enrollment.Status}");
        }

        static void PublishCourse(int courseId)
        {
            using var ctx = new AppDbContext();
            Console.WriteLine($"\n-- Publish course {courseId}");

            var course = ctx.Courses.Find(courseId);
            if (course is null)
            {
                Console.WriteLine("  Course not found.");
                return;
            }
            if (course.IsPublished)
            {
                Console.WriteLine("  Course is already published.");
                return;
            }

            course.IsPublished = true;
            course.PublishedAt = DateTime.Now;
            ctx.SaveChanges();
            Console.WriteLine($"  '{course.Title}' published on {course.PublishedAt:yyyy-MM-dd}");
        }

        // =====================================================================
        // DELETE
        // =====================================================================
        static void Unenroll(int studentId, int courseId)
        {
            using var ctx = new AppDbContext();
            Console.WriteLine($"\n-- Unenroll student {studentId} from course {courseId}");

            var enrollment = ctx.Enrollments.Find(studentId, courseId);
            if (enrollment is null)
            {
                Console.WriteLine("  Enrollment not found.");
                return;
            }

            ctx.Enrollments.Remove(enrollment);
            ctx.SaveChanges();
            Console.WriteLine("  Enrollment deleted.");
        }

        static void DeleteCategoryUnsafe(int categoryId)
        {
            using var ctx = new AppDbContext();
            Console.WriteLine($"\n-- Delete category {categoryId} (unsafe)");

            var category = ctx.Categories.Find(categoryId);
            if (category is null)
            {
                Console.WriteLine("  Category not found.");
                return;
            }

            ctx.Categories.Remove(category);
            try
            {
                ctx.SaveChanges();
                Console.WriteLine("  Category deleted.");
            }
            catch (DbUpdateException ex)
            {
                Console.WriteLine($"  DbUpdateException: {ex.Message}");
                Console.WriteLine($"  Inner: {ex.InnerException?.Message}");
            }
        }

        static void DeleteCategorySafe(int categoryId)
        {
            using var ctx = new AppDbContext();
            Console.WriteLine($"\n-- Delete category {categoryId} (safe)");

            var category = ctx.Categories.Find(categoryId);
            if (category is null)
            {
                Console.WriteLine("  Category not found.");
                return;
            }
            if (ctx.Categories.Any(c => c.ParentCategoryId == categoryId))
            {
                Console.WriteLine($"  Refused: '{category.Name}' still has sub-categories.");
                return;
            }
            if (ctx.Courses.Any(c => c.CategoryId == categoryId))
            {
                Console.WriteLine($"  Refused: '{category.Name}' still has courses.");
                return;
            }

            ctx.Categories.Remove(category);
            ctx.SaveChanges();
            Console.WriteLine($"  '{category.Name}' deleted.");
        }

        static void DeleteCourse(int courseId)
        {
            using var ctx = new AppDbContext();
            Console.WriteLine($"\n-- Delete course {courseId}");

            var course = ctx.Courses.Find(courseId);
            if (course is null)
            {
                Console.WriteLine("  Course not found.");
                return;
            }

            int before = ctx.Enrollments.Count(e => e.CourseId == courseId);
            Console.WriteLine($"  Enrollments before: {before}");

            ctx.Courses.Remove(course);
            ctx.SaveChanges();

            int after = ctx.Enrollments.Count(e => e.CourseId == courseId);
            Console.WriteLine($"  Enrollments after:  {after}");

        }

        // =====================================================================
        // BULK OPERATIONS (single statement, entities are not loaded)
        // =====================================================================
        static void BulkRaisePrice(CourseLevel level, decimal percent)
        {
            using var ctx = new AppDbContext();
            Console.WriteLine($"\n-- Raise price of published {level} courses by {percent:P0}");

            int rows = ctx.Courses
                .Where(c => c.IsPublished && c.Level == level)
                .ExecuteUpdate(s => s.SetProperty(c => c.Price, c => c.Price * (1 + percent)));

            Console.WriteLine($"  Rows affected: {rows}");
        }

        static void BulkDeleteDropped()
        {
            using var ctx = new AppDbContext();
            Console.WriteLine("\n-- Delete all Dropped enrollments");

            int rows = ctx.Enrollments
                .Where(e => e.Status == EnrollmentStatus.Dropped)
                .ExecuteDelete();

            Console.WriteLine($"  Rows affected: {rows}");
        }

        static void Header(string title) =>
            Console.WriteLine($"\n==================== {title} ====================");



            #endregion

    }
}
