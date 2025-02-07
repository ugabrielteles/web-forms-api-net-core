using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation.Results;
using OrderApi.Models;
using OrderApi.ValueObjects;

namespace OrderApi.Services.Contracts
{
    public interface IOrderService
    {
        Task<IList<Order>> GetAll();
        Task<Order?> GetById(int orderId);
        Task<IList<OrderStatus>> GetByOrderStatus();
        Task<(ValidationResult Result, Order? Entity)> CreateOrder(CreateOrderRequest request);
        Task<(ValidationResult Result, Order? Entity)> SendOrderToPreparing(int orderId);
        Task<(ValidationResult Result, Order? Entity)> SendOrderToOutForDelivery(int orderId);
        Task<(ValidationResult Result, Order? Entity)> SendOrderToDelivered(int orderId);
    }
}