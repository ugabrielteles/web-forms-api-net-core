using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using OrderApi.Models;
using OrderApi.Repositories.Contracts;

namespace OrderApi.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly OrderContext _context;

        public OrderRepository(OrderContext context)
        {
            _context = context;
        }
        
        public async Task<Order> Add(Order entity)
        {
            if(entity == null) throw new ArgumentNullException(nameof(entity));

            await _context.Orders.AddAsync(entity);

            await _context.SaveChangesAsync();

            return entity;
        }

        public async Task<Order> Delete(int id)
        {
            var entity = await _context.Orders.FindAsync(id);

            if(entity is null)
            {
                return entity!;
            }

            _context.Orders.Remove(entity);

            await _context.SaveChangesAsync();

            return entity;
        }

        public async Task<IList<Order>> GetAll() => await _context.Orders
                .Include(x => x.DeliveryOrder)
                .Include(x => x.OrderStatus)
                .AsNoTracking()
                .ToListAsync();

        public async Task<Order?> GetById(int id) => await _context.Orders.Include(x => x.DeliveryOrder).Include(x => x.OrderStatus).FirstOrDefaultAsync(x => x.OrderId == id);

        public async Task<string> HashBytesAsync(string value) => await _context.HashBytesAsync(value);

        public async Task<Order?> Search(Expression<Func<Order, bool>> predicate) => await _context.Orders.Include(x => x.DeliveryOrder).Include(x => x.OrderStatus).FirstOrDefaultAsync(predicate);

        public async Task<Order> Update(Order entity)
        {
           var result = _context.Orders.Update(entity);

           await _context.SaveChangesAsync();

           return result.Entity;
        }
    }
}