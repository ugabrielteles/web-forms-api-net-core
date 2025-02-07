using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation.Results;
using OrderApi.Enums;
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

        public OrderService(IOrderRepository orderRepository, IOrderStatusRepository orderStatusRepository, ILogger<OrderService> logger)
        {
            _orderRepository = orderRepository;
            _orderStatusRepository = orderStatusRepository;
            _logger = logger;
        }

        public async Task<(ValidationResult Result, Order? Entity)> CreateOrder(CreateOrderRequest request)
        {
            var result = new ValidationResult();

            try
            {
                if (request == null) throw new ArgumentNullException(nameof(request));

                result = request.Validate();

                if (!result.IsValid)
                {
                    return (result, null);
                }

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

                return (result, entity);
            }
            catch (ArgumentNullException exception)
            {
                _logger.LogError(exception, exception.Message);
                result.Errors.Add(new ValidationFailure(propertyName: exception.Message, errorMessage: exception.Message));
                return (result, null);
            }
            catch (Exception exception)
            {
                _logger.LogCritical(exception, exception.Message);
                result.Errors.Add(new ValidationFailure(propertyName: exception.Message, errorMessage: exception.Message));
                return (result, null);
            }
        }

        public async Task<IList<Order>> GetAll() => await _orderRepository.GetAll();

        public async Task<Order?> GetById(int orderId) => await _orderRepository.GetById(orderId);

        public async Task<IList<OrderStatus>> GetByOrderStatus() => await _orderStatusRepository.GetAll();

        public async Task<(ValidationResult Result, Order? Entity)> SendOrderToDelivered(int orderId)
        {
            var result = new ValidationResult();

            try
            {
                var order = await _orderRepository.GetById(orderId);

                if (order == null)
                    throw new ArgumentNullException(nameof(order));

                var orderStatus = await _orderStatusRepository.GetById((int)EnOrderStatus.Delivered);

                order.OrderStatusId = orderStatus!.OrderStatusId;

                order.DeliveryOrder = new DeliveryOrder
                {
                    DeliveryDate = DateTime.Now,
                    OrderId = order.OrderId
                };

                var entity = await _orderRepository.Update(order);

                return (result, entity);
            }
            catch (ArgumentNullException exception)
            {
                _logger.LogError(exception, exception.Message);
                result.Errors.Add(new ValidationFailure(propertyName: exception.Message, errorMessage: exception.Message));
                return (result, null);
            }
            catch (Exception exception)
            {
                _logger.LogCritical(exception, exception.Message);
                result.Errors.Add(new ValidationFailure(propertyName: exception.Message, errorMessage: exception.Message));
                return (result, null);
            }
        }

        public async Task<(ValidationResult Result, Order? Entity)> SendOrderToOutForDelivery(int orderId)
        {
            var result = new ValidationResult();

            try
            {
                var order = await _orderRepository.GetById(orderId);

                if (order == null)
                    throw new ArgumentNullException(nameof(order));

                var orderStatus = await _orderStatusRepository.GetById((int)EnOrderStatus.OutForDelivery);

                order.OrderStatusId = orderStatus!.OrderStatusId;

                var entity = await _orderRepository.Update(order);

                return (result, entity);
            }
            catch (ArgumentNullException exception)
            {
                _logger.LogError(exception, exception.Message);
                result.Errors.Add(new ValidationFailure(propertyName: exception.Message, errorMessage: exception.Message));
                return (result, null);
            }
            catch (Exception exception)
            {
                _logger.LogCritical(exception, exception.Message);
                result.Errors.Add(new ValidationFailure(propertyName: exception.Message, errorMessage: exception.Message));
                return (result, null);
            }
        }

        public async Task<(ValidationResult Result, Order? Entity)> SendOrderToPreparing(int orderId)
        {
            var result = new ValidationResult();

            try
            {
                var order = await _orderRepository.Search(x => x.OrderId == orderId);

                if (order == null)
                    throw new ArgumentNullException(nameof(order));

                var orderStatus = await _orderStatusRepository.GetById((int)EnOrderStatus.Preparing);

                order.OrderStatusId = orderStatus!.OrderStatusId;

                var entity = await _orderRepository.Update(order);

                return (result, entity);
            }
            catch (ArgumentNullException exception)
            {
                _logger.LogError(exception, exception.Message);
                result.Errors.Add(new ValidationFailure(propertyName: exception.Message, errorMessage: exception.Message));
                return (result, null);
            }
            catch (Exception exception)
            {
                _logger.LogCritical(exception, exception.Message);
                result.Errors.Add(new ValidationFailure(propertyName: exception.Message, errorMessage: exception.Message));
                return (result, null);
            }
        }
    }
}