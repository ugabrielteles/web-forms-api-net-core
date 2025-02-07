using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OrderApi.Models;

namespace OrderApi.Data.Mappings
{
    public class OrderMapping : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.ToTable("Order");

            builder.HasKey(x => x.OrderId);
            builder.Property(i => i.OrderId).HasColumnName("OrderId").ValueGeneratedOnAdd().IsRequired();
            builder.Property(i => i.Description).HasColumnName("Description").IsRequired();
            builder.Property(i => i.Street).HasColumnName("Street").IsRequired();
            builder.Property(i => i.State).HasColumnName("State").IsRequired();
            builder.Property(i => i.Number).HasColumnName("Number");
            builder.Property(i => i.Value).HasColumnName("Value").HasPrecision(18,2);
            builder.Property(i => i.City).HasColumnName("City").IsRequired();
            builder.Property(i => i.ZipCode).HasColumnName("ZipCode").IsRequired();
            builder.Property(i => i.Neighborhood).HasColumnName("Neighborhood").IsRequired();

            builder.HasOne(i => i.OrderStatus).WithMany(i => i.Orders).HasForeignKey(i => i.OrderId).IsRequired();
            builder.HasOne(i => i.DeliveryOrder).WithOne(i => i.Order).HasPrincipalKey<Order>(i => i.OrderId);
        }
    }
}