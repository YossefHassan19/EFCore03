using EFCore03.Contexts;
using EFCore03.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFCore03.Config
{
    public class CourseConfiguration : IEntityTypeConfiguration<Course>
    {
        public void Configure(EntityTypeBuilder<Course> builder)
        {
            builder.HasKey(c => c.Id);

            builder.Property(c => c.Title).IsRequired().HasMaxLength(150);
            builder.Property(c => c.Description).HasMaxLength(1000);
            builder.Property(c => c.Price).IsRequired().HasPrecision(10, 2);
            builder.Property(c => c.Level).IsRequired().HasConversion<string>().HasMaxLength(20);
            builder.Property(c => c.IsPublished).IsRequired();
            builder.Property(c => c.PublishedAt); 

            builder.HasMany(c => c.Enrollments)
                   .WithOne(e => e.Course)
                   .HasForeignKey(e => e.CourseId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasData(SeedData.Courses);

        }
    }
}
