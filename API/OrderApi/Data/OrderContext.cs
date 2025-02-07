using System.Data;
using System.Data.Common;
using System.Text;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace OrderApi.Models
{

    public class OrderContext : DbContext
    {

        public OrderContext() { }
        public OrderContext(DbContextOptions<OrderContext> options) : base(options) { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<OrderStatus>().HasData(
                new OrderStatus { OrderStatusId = 1, CreateAt = DateTime.Now, Name = "Criada", Active = true },
                new OrderStatus { OrderStatusId = 2, CreateAt = DateTime.Now, Name = "Preparando", Active = true },
                new OrderStatus { OrderStatusId = 3, CreateAt = DateTime.Now, Name = "Saiu para Entrega", Active = true },
                new OrderStatus { OrderStatusId = 4, CreateAt = DateTime.Now, Name = "Entregue", Active = true }
            );
        }

        public DbSet<User> Users { get; set; }
        public DbSet<OrderStatus> OrderStatus { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<DeliveryOrder> DeliveryOrders { get; set; }

        /// <summary>
        /// Create database based of models Ef
        /// </summary>
        /// <param name="builder"></param>
        /// <returns></returns>
        public async Task Init(WebApplicationBuilder builder)
        {
            await Database.EnsureCreatedAsync();
        }

        /// <summary>
        /// Create Encrypt string by HASHBYTES(SHA2_256)
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public async Task<string> HashBytesAsync(string value)
        {
            using (DbCommand command = this.Database.GetDbConnection().CreateCommand())
            {
                command.CommandText = $"SELECT CONVERT(NVARCHAR(32),HashBytes('MD5', '{value}'),2)";

                await this.Database.OpenConnectionAsync();

                var result = await command.ExecuteScalarAsync();

                await this.Database.CloseConnectionAsync();

                return result!.ToString()!;
            }
        }

    }
}