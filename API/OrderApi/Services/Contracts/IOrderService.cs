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
        /// <summary>
        /// Lista todas as ordens
        /// </summary>
        /// <returns></returns>
        Task<IList<Order>> GetAll();

        /// <summary>
        /// Obtem uma ordem pelo Id
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        Task<Order?> GetById(int orderId);

        /// <summary>
        /// Obtem as ordens baseada no Status, metodo para facilitador para controle do Painel de ordens
        /// </summary>
        /// <returns></returns>
        Task<IList<OrderStatus>> GetByOrderStatus();

        /// <summary>
        /// Cria uma nova ordem
        /// </summary>
        /// <param name="Result"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<(ValidationResult Result, Order? Entity)> CreateOrder(CreateOrderRequest request);

        /// <summary>
        /// Altera o status da ordem para em preparação
        /// </summary>
        /// <param name="Result"></param>
        /// <param name="orderId"></param>
        /// <returns></returns>
        Task<(ValidationResult Result, Order? Entity)> SendOrderToPreparing(int orderId);

        /// <summary>
        /// Altera o status da ordem para em rota de entrega
        /// </summary>
        /// <param name="Result"></param>
        /// <param name="orderId"></param>
        /// <returns></returns>
        Task<(ValidationResult Result, Order? Entity)> SendOrderToOutForDelivery(int orderId);

        /// <summary>
        /// Alterar o status da Ordem para entregue
        /// </summary>
        /// <param name="Result"></param>
        /// <param name="orderId"></param>
        /// <returns></returns>
        Task<(ValidationResult Result, Order? Entity)> SendOrderToDelivered(int orderId);
    }
}