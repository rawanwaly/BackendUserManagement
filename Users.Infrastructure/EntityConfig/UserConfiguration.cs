
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserManagement.Domain.Core.Models;

namespace UserManagement.Infrastructure.EntityConfig
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("Users");

            builder.HasKey(u => u.Id);

            builder.Property(u => u.FirstNameAR)
                .IsRequired()
                .HasMaxLength(20);
            builder.Property(u => u.FirstNameEN)
             .IsRequired()
             .HasMaxLength(20);

            builder.Property(u => u.LastNameAR)
                .IsRequired()
                .HasMaxLength(20);
            builder.Property(u => u.LastNameEN)
            .IsRequired()
            .HasMaxLength(20);

            builder.Property(u => u.Email)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(u => u.MobileNumber)
                .IsRequired()
                .HasMaxLength(15);

            builder.Property(u => u.Address)
                .HasMaxLength(200);

            builder.Property(u => u.MaritalStatus)
                .IsRequired();

            builder.HasIndex(u => u.Email).IsUnique();
            builder.HasIndex(u => u.MobileNumber).IsUnique();
            builder.Property(u => u.isActive).HasDefaultValue(true);

        }
    }
}
