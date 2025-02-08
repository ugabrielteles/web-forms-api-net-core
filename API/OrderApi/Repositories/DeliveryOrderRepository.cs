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
    public class DeliveryOrderRepository : IDeliveryOrderRepository
    {
         private readonly OrderContext _context;

        public DeliveryOrderRepository(OrderContext context)
        {
            _context = context;
        }
        
        public async Task<DeliveryOrder> Add(DeliveryOrder entity)
        {
            if(entity == null) throw new ArgumentNullException(nameof(entity));

            await _context.DeliveryOrders.AddAsync(entity);

            await _context.SaveChangesAsync();

            return entity;
        }

        public async Task<DeliveryOrder> Delete(int id)
        {
            var entity = await _context.DeliveryOrders.FindAsync(id);

            if(entity is null)
            {
                return entity!;
            }

            _context.DeliveryOrders.Remove(entity);

            await _context.SaveChangesAsync();

            return entity;
        }

        public async Task<IList<DeliveryOrder>> GetAll() => await _context.DeliveryOrders.ToListAsync();

        public async Task<DeliveryOrder?> GetById(int id) => await _context.DeliveryOrders.FirstOrDefaultAsync(x => x.DeliveryOrderId == id);

        public async Task<string> HashBytesAsync(string value) => await _context.HashBytesAsync(value);

        public async Task<DeliveryOrder?> Search(Expression<Func<DeliveryOrder, bool>> predicate) => await _context.DeliveryOrders.FirstOrDefaultAsync(predicate);

        public async Task<DeliveryOrder> Update(DeliveryOrder entity)
        {
           var result = _context.DeliveryOrders.Update(entity);

           await _context.SaveChangesAsync();

           return result.Entity;
        }
    }
}