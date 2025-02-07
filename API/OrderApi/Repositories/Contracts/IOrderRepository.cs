using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OrderApi.Models;

namespace OrderApi.Repositories.Contracts
{
    public interface IOrderRepository : IRepository<Order>
    {
        /// <summary>
        /// Para melhor controle atualizar apenas a propriedade de status
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<Order> UpdateOrderStatus(Order entity);
    }
}