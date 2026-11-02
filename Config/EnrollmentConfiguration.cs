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
    public class EnrollmentConfiguration : IEntityTypeConfiguration<Enrollment>
    {
        public void Configure(EntityTypeBuilder<Enrollment> builder)
        {
            builder.HasKey(e => new { e.StudentId, e.CourseId });

            builder.Property(e => e.EnrolledAt).IsRequired();
            builder.Property(e => e.Progress).IsRequired();
            builder.Property(e => e.Status).IsRequired().HasConversion<string>().HasMaxLength(20);
            builder.Property(e => e.FinalGrade).HasPrecision(5, 2);

            builder.ToTable(t =>
            {
                t.HasCheckConstraint("CK_Enrollments_Progress", "[Progress] BETWEEN 0 AND 100");
                t.HasCheckConstraint("CK_Enrollments_FinalGrade", "[FinalGrade] IS NULL OR [FinalGrade] BETWEEN 0 AND 100");
            });

        }
    }
}
