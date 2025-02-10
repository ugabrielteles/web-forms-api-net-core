using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using Moq;
using OrderApi.Enums;
using OrderApi.Hubs;
using OrderApi.Models;
using OrderApi.Repositories.Contracts;
using OrderApi.Services;
using OrderApi.ValueObjects;

namespace OrderApi.Tests.Services
{
    public class OrderServiceTest
    {

        [Fact(DisplayName = "Cria um novo pedido com sucesso")]
        public async Task Cria_Novo_Pedido_Sucesso()
        {
            // Arrange
            var mockOrderRepo = new Mock<IOrderRepository>();
            var mockStatusRepo = new Mock<IOrderStatusRepository>();
            var mockDeliveryRepo = new Mock<IDeliveryOrderRepository>();
            var mockLogger = new Mock<ILogger<OrderService>>();
            var mockHub = new Mock<IHubContext<OrderHub>>();
            var mockHubClients = new Mock<IHubClients>();
            var mockHubClientsProxy = new Mock<IClientProxy>();

            mockHubClients.Setup(x => x.All)
                .Returns(mockHubClientsProxy.Object);

            mockHub.Setup(x => x.Clients)
              .Returns(mockHubClients.Object);


            var orderStatus = new OrderStatus { OrderStatusId = (int)EnOrderStatus.Create };
            mockStatusRepo.Setup(x => x.GetById((int)EnOrderStatus.Create))
                .ReturnsAsync(orderStatus);

            var expectedOrder = new Order
            {
                OrderId = 1,
                Description = "Test Order",
                Value = 100,
                OrderStatusId = orderStatus.OrderStatusId,
                City = "Salvador",
                State = "BA",
                ZipCode = "48700000",
                Neighborhood = "Pituba",
                Number = "7",
                Street = "RUA ACM"
            };
            mockOrderRepo.Setup(x => x.Add(It.IsAny<Order>()))
                .ReturnsAsync(expectedOrder);

            var service = new OrderService(mockOrderRepo.Object, mockStatusRepo.Object,
                mockDeliveryRepo.Object, mockLogger.Object, mockHub.Object);

            var request = new CreateOrderRequest
            {
                Description = "Test Order",
                Value = 100,
                City = "Salvador",
                State = "BA",
                ZipCode = "48700000",
                Neighborhood = "Pituba",
                Number = "7",
                Street = "RUA ACM"
            };

            // Act
            var result = await service.CreateOrder(request);

            // Assert
            Assert.True(result.Result.IsValid);
            Assert.NotNull(result.Entity);
            Assert.Equal(expectedOrder.OrderId, result.Entity.OrderId);
        }

        [Fact(DisplayName = "Cria pedido com solicitação nula - DomainException")]
        public async Task Cria_Pedido_Request_Nulo_DomainException()
        {
            // Arrange
            var mockOrderRepo = new Mock<IOrderRepository>();
            var mockStatusRepo = new Mock<IOrderStatusRepository>();
            var mockDeliveryRepo = new Mock<IDeliveryOrderRepository>();
            var mockLogger = new Mock<ILogger<OrderService>>();
            var mockHub = new Mock<IHubContext<OrderHub>>();

            var service = new OrderService(mockOrderRepo.Object, mockStatusRepo.Object,
                mockDeliveryRepo.Object, mockLogger.Object, mockHub.Object);

            // Act
            var result = await service.CreateOrder(null!);

            // Assert
            Assert.False(result.Result.IsValid);
            Assert.Null(result.Entity);
            Assert.Contains(result.Result.Errors,
                error => error.ErrorMessage == "Request não foi informado!");
        }

        // Get all orders returns list of orders
        [Fact(DisplayName = "Lista todas as ordens - sucesso")]
        public async Task Lista_Todas_Ordens_Sucesso()
        {
            // Arrange
            var mockOrderRepo = new Mock<IOrderRepository>();
            var mockStatusRepo = new Mock<IOrderStatusRepository>();
            var mockDeliveryRepo = new Mock<IDeliveryOrderRepository>();
            var mockLogger = new Mock<ILogger<OrderService>>();
            var mockHub = new Mock<IHubContext<OrderHub>>();

            var expectedOrders = new List<Order>
            {
                new Order { OrderId = 1, Description = "Order 1", Value = 100 },
                new Order { OrderId = 2, Description = "Order 2", Value = 200 }
            };
            mockOrderRepo.Setup(x => x.GetAll()).ReturnsAsync(expectedOrders);

            var service = new OrderService(mockOrderRepo.Object, mockStatusRepo.Object,
                mockDeliveryRepo.Object, mockLogger.Object, mockHub.Object);

            // Act
            var result = await service.GetAll();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedOrders.Count, result.Count);
            Assert.Equal(expectedOrders[0].OrderId, result[0].OrderId);
            Assert.Equal(expectedOrders[1].OrderId, result[1].OrderId);
        }

        [Fact(DisplayName = "Obtem uma ordem por ID - Sucesso")]
        public async Task Obtem_Uma_Ordem_Por_Id_Sucesso()
        {
            // Arrange
            var mockOrderRepo = new Mock<IOrderRepository>();
            var mockStatusRepo = new Mock<IOrderStatusRepository>();
            var mockDeliveryRepo = new Mock<IDeliveryOrderRepository>();
            var mockLogger = new Mock<ILogger<OrderService>>();
            var mockHub = new Mock<IHubContext<OrderHub>>();

            var expectedOrder = new Order
            {
                OrderId = 1,
                Description = "Test Order",
                Value = 100,
                OrderStatusId = 1
            };
            mockOrderRepo.Setup(x => x.GetById(1))
                .ReturnsAsync(expectedOrder);

            var service = new OrderService(mockOrderRepo.Object, mockStatusRepo.Object,
                mockDeliveryRepo.Object, mockLogger.Object, mockHub.Object);

            // Act
            var result = await service.GetById(1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedOrder.OrderId, result.OrderId);
            Assert.Equal(expectedOrder.Description, result.Description);
        }

        [Fact(DisplayName = "Obtem todos os status de pedidos - Sucesso")]
        public async Task Obtem_Todos_Os_Status_Pedido_Sucesso()
        {
            // Arrange
            var mockOrderRepo = new Mock<IOrderRepository>();
            var mockStatusRepo = new Mock<IOrderStatusRepository>();
            var mockDeliveryRepo = new Mock<IDeliveryOrderRepository>();
            var mockLogger = new Mock<ILogger<OrderService>>();
            var mockHub = new Mock<IHubContext<OrderHub>>();

            var expectedStatuses = new List<OrderStatus>
            {
                new OrderStatus { OrderStatusId = 1, Name = "Created" },
                new OrderStatus { OrderStatusId = 2, Name = "Preparing" },
                new OrderStatus { OrderStatusId = 3, Name = "OutForDelivery" },
                new OrderStatus { OrderStatusId = 4, Name = "Delivered" }
            };
            mockStatusRepo.Setup(x => x.GetAll())
                .ReturnsAsync(expectedStatuses);

            var service = new OrderService(mockOrderRepo.Object, mockStatusRepo.Object,
                mockDeliveryRepo.Object, mockLogger.Object, mockHub.Object);

            // Act
            var result = await service.GetByOrderStatus();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedStatuses.Count, result.Count);
            Assert.Equal(expectedStatuses, result);
        }

        [Fact(DisplayName = "Atualizar status do pedido para preparando - Sucesso")]
        public async Task Atualiza_Status_Pedido_Para_Preparando()
        {
            // Arrange
            var mockOrderRepo = new Mock<IOrderRepository>();
            var mockStatusRepo = new Mock<IOrderStatusRepository>();
            var mockDeliveryRepo = new Mock<IDeliveryOrderRepository>();
            var mockLogger = new Mock<ILogger<OrderService>>();
            var mockHub = new Mock<IHubContext<OrderHub>>();

            var orderStatus = new OrderStatus { OrderStatusId = (int)EnOrderStatus.Preparing };
            mockStatusRepo.Setup(x => x.GetById((int)EnOrderStatus.Preparing))
                .ReturnsAsync(orderStatus);

            var existingOrder = new Order
            {
                OrderId = 1,
                Description = "Test Order",
                Value = 100,
                OrderStatusId = (int)EnOrderStatus.Create
            };

            mockOrderRepo.Setup(x => x.Search(It.IsAny<Expression<Func<Order, bool>>>()))
                .ReturnsAsync(existingOrder);

            mockOrderRepo.Setup(x => x.UpdateOrderStatus(It.IsAny<Order>()))
                .ReturnsAsync(existingOrder);

            var service = new OrderService(mockOrderRepo.Object, mockStatusRepo.Object,
                mockDeliveryRepo.Object, mockLogger.Object, mockHub.Object);

            // Act
            var result = await service.SendOrderToPreparing(existingOrder.OrderId);

            // Assert
            Assert.True(result.Result.IsValid);
            Assert.NotNull(result.Entity);
            Assert.Equal(orderStatus.OrderStatusId, result.Entity.OrderStatusId);
        }


        [Fact(DisplayName = "Atualizar status do pedido para saiu para entrega - Sucesso")]
        public async Task Atualiza_Status_Pedido_Para_Saiu_Para_Entrega()
        {
            // Arrange
            var mockOrderRepo = new Mock<IOrderRepository>();
            var mockStatusRepo = new Mock<IOrderStatusRepository>();
            var mockDeliveryRepo = new Mock<IDeliveryOrderRepository>();
            var mockLogger = new Mock<ILogger<OrderService>>();
            var mockHub = new Mock<IHubContext<OrderHub>>();

            var orderStatus = new OrderStatus { OrderStatusId = (int)EnOrderStatus.OutForDelivery };
            mockStatusRepo.Setup(x => x.GetById((int)EnOrderStatus.OutForDelivery))
                .ReturnsAsync(orderStatus);

            var existingOrder = new Order
            {
                OrderId = 1,
                Description = "Test Order",
                Value = 100,
                OrderStatusId = (int)EnOrderStatus.Preparing
            };
            mockOrderRepo.Setup(x => x.GetById(existingOrder.OrderId))
                .ReturnsAsync(existingOrder);

            mockOrderRepo.Setup(x => x.UpdateOrderStatus(It.IsAny<Order>()))
                .ReturnsAsync(existingOrder);

            var service = new OrderService(mockOrderRepo.Object, mockStatusRepo.Object,
                mockDeliveryRepo.Object, mockLogger.Object, mockHub.Object);

            // Act
            var result = await service.SendOrderToOutForDelivery(existingOrder.OrderId);

            // Assert
            Assert.True(result.Result.IsValid);
            Assert.NotNull(result.Entity);
            Assert.Equal(orderStatus.OrderStatusId, result.Entity.OrderStatusId);
        }

         [Fact(DisplayName = "Atualizar status do pedido para entregue - Sucesso")]
        public async Task Atualiza_Status_Pedido_Para_Saiu_Para_Entregue_Salva_DataEntrega()
        {
            // Arrange
            var mockOrderRepo = new Mock<IOrderRepository>();
            var mockStatusRepo = new Mock<IOrderStatusRepository>();
            var mockDeliveryRepo = new Mock<IDeliveryOrderRepository>();
            var mockLogger = new Mock<ILogger<OrderService>>();
            var mockHub = new Mock<IHubContext<OrderHub>>();

            var orderStatus = new OrderStatus { OrderStatusId = (int)EnOrderStatus.Delivered };
            mockStatusRepo.Setup(x => x.GetById((int)EnOrderStatus.Delivered))
                .ReturnsAsync(orderStatus);

            var existingOrder = new Order
            {
                OrderId = 1,
                OrderStatusId = (int)EnOrderStatus.OutForDelivery
            };
            mockOrderRepo.Setup(x => x.GetById(1))
                .ReturnsAsync(existingOrder);

            mockDeliveryRepo.Setup(x => x.Search(It.IsAny<Expression<Func<DeliveryOrder, bool>>>()))
                .ReturnsAsync((DeliveryOrder)null!);

            mockOrderRepo.Setup(x => x.UpdateOrderStatus(It.IsAny<Order>()))
                .ReturnsAsync(existingOrder);

            var service = new OrderService(mockOrderRepo.Object, mockStatusRepo.Object,
                mockDeliveryRepo.Object, mockLogger.Object, mockHub.Object);

            // Act
            var result = await service.SendOrderToDelivered(1);

            // Assert
            Assert.True(result.Result.IsValid);
            Assert.NotNull(result.Entity);
            Assert.Equal(orderStatus.OrderStatusId, result.Entity.OrderStatusId);
            mockDeliveryRepo.Verify(x => x.Add(It.Is<DeliveryOrder>(d => d.OrderId == 1)), Times.Once);
        }
    }
}