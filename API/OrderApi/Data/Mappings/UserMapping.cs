using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OrderApi.Models;

namespace OrderApi.Data.Mappings
{
    public class UserMapping : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("Users");

            builder.HasKey(i => i.UserId);
            builder.Property(i => i.UserId).HasColumnName("UserId").ValueGeneratedOnAdd();
            builder.Property(i => i.Name).HasColumnName("Name").HasMaxLength(200).IsRequired();
            builder.Property(i => i.Password)
                .HasColumnName("Password")
                .HasColumnType("varbinary(max)")                
                .HasComputedColumnSql("HASHBYTES('SHA2_256', [Password])", stored: true)
                .IsRequired();
            builder.Property(i => i.Email).HasColumnName("Email").HasMaxLength(200).IsRequired();
            builder.Property(i => i.CreateAt).HasColumnName("CreateAt").HasDefaultValue(DateTime.Now);
        }
    }
}