using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace EFCore03.Contexts.Migrations
{
    /// <inheritdoc />
    public partial class SeedData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "Description", "Name", "ParentCategoryId" },
                values: new object[,]
                {
                    { 1, "Everything about writing code", "Programming", null },
                    { 5, null, "Design", null }
                });

            migrationBuilder.InsertData(
                table: "Instructors",
                columns: new[] { "Id", "Bio", "Email", "FullName", "JoinedAt" },
                values: new object[,]
                {
                    { 1, "Senior .NET developer and trainer", "ahmed.hassan@example.com", "Ahmed Hassan", new DateTime(2023, 1, 10, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 2, null, "sara.mostafa@example.com", "Sara Mostafa", new DateTime(2023, 6, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 3, "UI/UX designer", "omar.khaled@example.com", "Omar Khaled", new DateTime(2024, 2, 20, 0, 0, 0, 0, DateTimeKind.Unspecified) }
                });

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "Description", "Name", "ParentCategoryId" },
                values: new object[,]
                {
                    { 2, "Server-side development", "Backend", 1 },
                    { 4, "Browser-side development", "Frontend", 1 },
                    { 6, null, "UI Design", 5 },
                    { 7, "Apps for phones", "Mobile Development", 1 }
                });

            migrationBuilder.InsertData(
                table: "Courses",
                columns: new[] { "Id", "CategoryId", "Description", "InstructorId", "IsPublished", "Level", "Price", "PublishedAt", "Title" },
                values: new object[] { 3, 1, "Start from zero", 2, true, "Beginner", 299.00m, new DateTime(2024, 11, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), "C# for Beginners" });

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "Description", "Name", "ParentCategoryId" },
                values: new object[] { 3, "Web apps with .NET", "ASP.NET", 2 });

            migrationBuilder.InsertData(
                table: "Courses",
                columns: new[] { "Id", "CategoryId", "Description", "InstructorId", "IsPublished", "Level", "Price", "PublishedAt", "Title" },
                values: new object[,]
                {
                    { 4, 2, "Design clean APIs", 2, false, "Intermediate", 549.50m, null, "REST API Design" },
                    { 5, 4, null, 3, true, "Beginner", 199.00m, new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "HTML & CSS Basics" },
                    { 6, 6, "Layout, color, typography", 3, false, "Intermediate", 399.00m, null, "UI Design Principles" },
                    { 1, 3, "Build your first web app", 1, true, "Beginner", 499.99m, new DateTime(2025, 1, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "ASP.NET Core Fundamentals" },
                    { 2, 3, null, 1, true, "Advanced", 799.00m, new DateTime(2025, 3, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Advanced EF Core" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Courses",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Courses",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Courses",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Courses",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Courses",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Courses",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Instructors",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Instructors",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Instructors",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 1);
        }
    }
}
