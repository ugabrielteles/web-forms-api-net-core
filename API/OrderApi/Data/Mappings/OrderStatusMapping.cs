using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OrderApi.Models;

namespace OrderApi.Data.Mappings
{
    public class OrderStatusMapping : IEntityTypeConfiguration<OrderStatus>
    {
        public void Configure(EntityTypeBuilder<OrderStatus> builder)
        {
            builder.ToTable("OrderStatus");

            builder.HasKey(i => i.OrderStatusId);
            builder.Property(i => i.OrderStatusId).HasColumnName("OrderStatusId").ValueGeneratedNever().IsRequired();
            builder.Property(i => i.Name).HasColumnName("Name").HasMaxLength(200).IsRequired();
            builder.Property(i => i.Active).HasColumnName("Active").HasColumnType("bit").IsRequired();
            builder.Property(i => i.CreateAt).HasColumnName("CreateAt").HasDefaultValue(DateTime.Now);
            builder.HasMany(i => i.Orders).WithOne(i => i.OrderStatus).HasForeignKey(x => x.OrderId);

        }
    }
}