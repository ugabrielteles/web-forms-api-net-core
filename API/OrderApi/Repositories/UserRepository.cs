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
    public class UserRepository : IUserRepository
    {
        private readonly OrderContext _context;

        public UserRepository(OrderContext context)
        {
            _context = context;
        }
        
        public async Task<User> Add(User entity)
        {
            if(entity == null) throw new ArgumentNullException(nameof(entity));

            await _context.Users.AddAsync(entity);

            await _context.SaveChangesAsync();

            return entity;
        }

        public async Task<User> Delete(int id)
        {
            var entity = await _context.Users.FindAsync(id);

            if(entity is null)
            {
                return entity!;
            }

            _context.Users.Remove(entity);

            await _context.SaveChangesAsync();

            return entity;
        }

        public async Task<IList<User>> GetAll() => await _context.Users.ToListAsync();

        public async Task<User?> GetById(int id) => await _context.Users.FirstOrDefaultAsync(x => x.UserId == id);

        public async Task<string> HashBytesAsync(string value) => await _context.HashBytesAsync(value);

        public async Task<User?> Search(Expression<Func<User, bool>> predicate) => await _context.Users.FirstOrDefaultAsync(predicate);

        public async Task<User> Update(User entity)
        {
           var result = _context.Users.Update(entity);

           await _context.SaveChangesAsync();

           return result.Entity;
        }
    }
}