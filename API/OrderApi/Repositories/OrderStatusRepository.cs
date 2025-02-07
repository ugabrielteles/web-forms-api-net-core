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
    public class OrderStatusRepository : IOrderStatusRepository
    {
        private readonly OrderContext _context;

        public OrderStatusRepository(OrderContext context)
        {
            _context = context;
        }
        
        public async Task<OrderStatus> Add(OrderStatus entity)
        {
            if(entity == null) throw new ArgumentNullException(nameof(entity));

            await _context.OrderStatus.AddAsync(entity);

            await _context.SaveChangesAsync();

            return entity;
        }

        public async Task<OrderStatus> Delete(int id)
        {
            var entity = await _context.OrderStatus.FindAsync(id);

            if(entity is null)
            {
                return entity!;
            }

            _context.OrderStatus.Remove(entity);

            await _context.SaveChangesAsync();

            return entity;
        }

        public async Task<IList<OrderStatus>> GetAll() => await _context.OrderStatus.Include(x => x.Orders)!.ThenInclude(x => x.DeliveryOrder).ToListAsync();

        public async Task<OrderStatus?> GetById(int id) => await _context.OrderStatus.Include(x => x.Orders).FirstOrDefaultAsync(x => x.OrderStatusId == id);

        public async Task<string> HashBytesAsync(string value) => await _context.HashBytesAsync(value);

        public async Task<OrderStatus?> Search(Expression<Func<OrderStatus, bool>> predicate) => await _context.OrderStatus.Include(x => x.Orders).FirstOrDefaultAsync(predicate);

        public async Task<OrderStatus> Update(OrderStatus entity)
        {
           var result = _context.OrderStatus.Update(entity);

           await _context.SaveChangesAsync();

           return result.Entity;
        }
    }
}