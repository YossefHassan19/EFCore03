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
    public class InstructorConfiguration : IEntityTypeConfiguration<Instructor>
    {
        public void Configure(EntityTypeBuilder<Instructor> builder)
        {
            builder.HasKey(i => i.Id);

            builder.Property(i => i.FullName).IsRequired().HasMaxLength(100);
            builder.Property(i => i.Email).IsRequired().HasMaxLength(150);
            builder.Property(i => i.Bio).HasMaxLength(500); 
            builder.Property(i => i.JoinedAt).IsRequired().HasColumnType("date");

            builder.HasIndex(i => i.Email).IsUnique();

            builder.HasMany(i => i.Courses)
                   .WithOne(c => c.Instructor)
                   .HasForeignKey(c => c.InstructorId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasData(SeedData.Instructors);

        }
    }

}
