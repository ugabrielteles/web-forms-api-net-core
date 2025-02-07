using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OrderApi.Models;

namespace OrderApi.Repositories.Contracts
{
    public interface IUserRepository : IRepository<User>
    {
        Task<string> HashBytesAsync(string value);
    }
}