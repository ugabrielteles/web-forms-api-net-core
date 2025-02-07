using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OrderApi.Models;

namespace OrderApi.Data.Mappings
{
    public class DeliveryOrderMapping : IEntityTypeConfiguration<DeliveryOrder>
    {
        public void Configure(EntityTypeBuilder<DeliveryOrder> builder)
        {
            builder.ToTable("DeliveryOrders");

            builder.HasKey(i => i.DeliveryOrderId);
            builder.Property(i => i.DeliveryOrderId).HasColumnName("DeliveryOrderId").ValueGeneratedOnAdd();
            builder.Property(i => i.DeliveryDate).HasColumnName("DeliveryDate").IsRequired();
            builder.HasOne(i => i.Order).WithOne().HasForeignKey<DeliveryOrder>(i => i.OrderId);
        }

        
    }
    
}