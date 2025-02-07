using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation.Results;
using OrderApi.Enums;
using OrderApi.Exceptions;
using OrderApi.Models;
using OrderApi.Repositories.Contracts;
using OrderApi.Services.Contracts;
using OrderApi.ValueObjects;

namespace OrderApi.Services
{
    public class OrderService : IOrderService
    {
        private readonly ILogger _logger;
        private readonly IOrderRepository _orderRepository;
        private readonly IOrderStatusRepository _orderStatusRepository;
        private readonly IDeliveryOrderRepository _deliveryOrderRepository;

        public OrderService(IOrderRepository orderRepository, IOrderStatusRepository orderStatusRepository, IDeliveryOrderRepository deliveryOrderRepository, ILogger<OrderService> logger)
        {
            _orderRepository = orderRepository;
            _orderStatusRepository = orderStatusRepository;
            _deliveryOrderRepository = deliveryOrderRepository;
            _logger = logger;
        }

        /// <summary>
        /// Cria uma nova ordem
        /// </summary>
        /// <param name="Result"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<(ValidationResult Result, Order? Entity)> CreateOrder(CreateOrderRequest request)
        {
            _logger.LogDebug("Criação de uma nova ordem iniciada");

            var result = new ValidationResult();

            try
            {
                if (request == null)
                    throw new DomainException("Request não foi informado!");

                //Valida os campos utilizando FluentValidation
                result = request.Validate();

                if (!result.IsValid)
                {
                    return (result, null);
                }

                //Busca o status da ordem de acordo com o Enum
                var orderStatus = await _orderStatusRepository.GetById((int)EnOrderStatus.Create);

                var entity = await _orderRepository.Add(new Models.Order
                {
                    Description = request.Description,
                    Value = request.Value,
                    City = request.City,
                    Street = request.Street,
                    State = request.State,
                    Number = request.Number,
                    Neighborhood = request.Neighborhood,
                    ZipCode = request.ZipCode,
                    OrderStatusId = orderStatus!.OrderStatusId,
                    CreateAt = DateTime.Now
                });

                 _logger.LogDebug("Criação de uma nova ordem finalizada com sucesso");

                return (result, entity);
            }
            catch (DomainException exception)
            {                
                _logger.LogError(exception, exception.Message);
                _logger.LogDebug("Criação de uma nova ordem finalizada com erro");
                result.Errors.Add(new ValidationFailure(propertyName: exception.Message, errorMessage: exception.Message));
                return (result, null);
            }
            catch (Exception exception)
            {
                _logger.LogCritical(exception, exception.Message);
                _logger.LogDebug("Criação de uma nova ordem finalizada com erro");
                result.Errors.Add(new ValidationFailure(propertyName: exception.Message, errorMessage: exception.Message));
                return (result, null);
            }
        }

        /// <summary>
        /// Lista todas as ordens
        /// </summary>
        /// <returns></returns>
        public async Task<IList<Order>> GetAll() => await _orderRepository.GetAll();

        /// <summary>
        /// Obtem uma ordem pelo Id
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        public async Task<Order?> GetById(int orderId) => await _orderRepository.GetById(orderId);

        /// <summary>
        /// Obtem as ordens baseada no Status, metodo para facilitador para controle do Painel de ordens
        /// </summary>
        /// <returns></returns>
        public async Task<IList<OrderStatus>> GetByOrderStatus() => await _orderStatusRepository.GetAll();

        public async Task<(ValidationResult Result, Order? Entity)> SendOrderToCreate(int orderId)
        {
            _logger.LogDebug("Envio da ordem: {0} para criada iniciada", orderId);

            var result = new ValidationResult();

            try
            {
                var order = await _orderRepository.GetById(orderId);

                if (order == null)
                    throw new DomainException($"A Ordem: {orderId} não foi encontrada");

                var orderStatus = await _orderStatusRepository.GetById((int)EnOrderStatus.Create);

                order.OrderStatusId = orderStatus!.OrderStatusId;                

                var entity = await _orderRepository.UpdateOrderStatus(order);

                  _logger.LogDebug("Envio da ordem: {0} para criada finalizada com sucesso", orderId);

                return (result, entity);
            }
            catch (DomainException exception)
            {
                _logger.LogError(exception, exception.Message);
                _logger.LogDebug("Envio da ordem: {0} para criada finalizada com erro", orderId);
                result.Errors.Add(new ValidationFailure(propertyName: exception.Message, errorMessage: exception.Message));
                return (result, null);
            }
            catch (Exception exception)
            {
                _logger.LogCritical(exception, exception.Message);
                _logger.LogDebug("Envio da ordem: {0} para criada finalizada com erro", orderId);
                result.Errors.Add(new ValidationFailure(propertyName: exception.Message, errorMessage: exception.Message));
                return (result, null);
            }
        }

        /// <summary>
        /// Alterar o status da Ordem para entregue
        /// </summary>
        /// <param name="Result"></param>
        /// <param name="orderId"></param>
        /// <returns></returns>
        public async Task<(ValidationResult Result, Order? Entity)> SendOrderToDelivered(int orderId)
        {
            _logger.LogDebug("Envio da ordem: {0} para entregue iniciada", orderId);

            var result = new ValidationResult();

            try
            {
                var order = await _orderRepository.GetById(orderId);

                if (order == null)
                    throw new DomainException($"A Ordem: {orderId} não foi encontrada");

                var orderStatus = await _orderStatusRepository.GetById((int)EnOrderStatus.Delivered);

                order.OrderStatusId = orderStatus!.OrderStatusId;

                var deliveryOrder = await _deliveryOrderRepository.Search(x => x.OrderId == orderId);

                if(deliveryOrder != null)
                {
                    await _deliveryOrderRepository.Delete(deliveryOrder.DeliveryOrderId);
                } 

                var entity = await _orderRepository.UpdateOrderStatus(order);
                
                await _deliveryOrderRepository.Add( new DeliveryOrder
                {
                    DeliveryDate = DateTime.Now,
                    OrderId = order.OrderId
                });               


                _logger.LogDebug("Envio da ordem: {0} para entregue finalizada com sucesso", orderId);

                return (result, entity);
            }
            catch (DomainException exception)
            {
                _logger.LogError(exception, exception.Message);
                _logger.LogDebug("Envio da ordem: {0} para entregue finalizada com erro", orderId);
                result.Errors.Add(new ValidationFailure(propertyName: exception.Message, errorMessage: exception.Message));
                return (result, null);
            }
            catch (Exception exception)
            {
                _logger.LogCritical(exception, exception.Message);
                _logger.LogDebug("Envio da ordem: {0} para entregue finalizada com erro", orderId);
                result.Errors.Add(new ValidationFailure(propertyName: exception.Message, errorMessage: exception.Message));
                return (result, null);
            }
        }

        /// <summary>
        /// Altera o status da ordem para em rota de entrega
        /// </summary>
        /// <param name="Result"></param>
        /// <param name="orderId"></param>
        /// <returns></returns>
        public async Task<(ValidationResult Result, Order? Entity)> SendOrderToOutForDelivery(int orderId)
        {            
            _logger.LogDebug("Envio da ordem: {0} para em rota de entrega iniciada", orderId);
            
            var result = new ValidationResult();

            try
            {
                var order = await _orderRepository.GetById(orderId);

                if (order == null)
                    throw new DomainException($"A Ordem: {orderId} não foi encontrada");

                var orderStatus = await _orderStatusRepository.GetById((int)EnOrderStatus.OutForDelivery);

                order.OrderStatusId = orderStatus!.OrderStatusId;

                var entity = await _orderRepository.UpdateOrderStatus(order);

                _logger.LogDebug("Envio da ordem: {0} para em rota de entrega finalizada com sucesso", orderId);

                return (result, entity);
            }
            catch (DomainException exception)
            {
                _logger.LogError(exception, exception.Message);
                _logger.LogDebug("Envio da ordem: {0} para em rota de entrega finalizada com erro", orderId);
                result.Errors.Add(new ValidationFailure(propertyName: exception.Message, errorMessage: exception.Message));
                return (result, null);
            }
            catch (Exception exception)
            {
                _logger.LogCritical(exception, exception.Message);
                _logger.LogDebug("Envio da ordem: {0} para em rota de entrega finalizada com erro", orderId);
                result.Errors.Add(new ValidationFailure(propertyName: exception.Message, errorMessage: exception.Message));
                return (result, null);
            }
        }

        /// <summary>
        /// Altera o status da ordem para em preparação
        /// </summary>
        /// <param name="Result"></param>
        /// <param name="orderId"></param>
        /// <returns></returns>
        public async Task<(ValidationResult Result, Order? Entity)> SendOrderToPreparing(int orderId)
        {
             _logger.LogDebug("Envio da ordem: {0} para em rota de entrega iniciada", orderId);

            var result = new ValidationResult();

            try
            {
                var order = await _orderRepository.Search(x => x.OrderId == orderId);

                if (order == null)
                    throw new DomainException($"A Ordem: {orderId} não foi encontrada");

                var orderStatus = await _orderStatusRepository.GetById((int)EnOrderStatus.Preparing);

                order.OrderStatusId = orderStatus!.OrderStatusId;

                var entity = await _orderRepository.UpdateOrderStatus(order);

                 _logger.LogDebug("Envio da ordem: {0} para em rota de entrega finalizada com sucesso", orderId);

                return (result, entity);
            }
            catch (DomainException exception)
            {
                _logger.LogError(exception, exception.Message);
                _logger.LogDebug("Envio da ordem: {0} para em rota de entrega finalizada com erro", orderId);
                result.Errors.Add(new ValidationFailure(propertyName: exception.Message, errorMessage: exception.Message));
                return (result, null);
            }
            catch (Exception exception)
            {
                _logger.LogCritical(exception, exception.Message);
                 _logger.LogDebug("Envio da ordem: {0} para em rota de entrega finalizada com erro", orderId);
                result.Errors.Add(new ValidationFailure(propertyName: exception.Message, errorMessage: exception.Message));
                return (result, null);
            }
        }
    }
}