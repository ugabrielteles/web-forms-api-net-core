using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OrderApi.Models;
using OrderApi.Services.Contracts;
using OrderApi.ValueObjects;

namespace OrderApi.Controllers
{
    [ApiController]
    [Route("api/orders")]
    [Authorize]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }
        

        [HttpPost]
        public async Task<IActionResult> Create(CreateOrderRequest request)
        {
            var response = await _orderService.CreateOrder(request);

            if(!response.Result.IsValid)
               return BadRequest(response.Result.Errors);

            return Ok(response.Entity);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var orders = await _orderService.GetAll();

            return Ok(orders ?? new List<Order>());
        }

        [HttpGet("{orderId}")]
        public async Task<IActionResult> GetById([FromRoute] int orderId)
        {
            var order = await _orderService.GetById(orderId);

            if(order == null)
            {
                return NotFound();
            }

            return Ok(order);
        }

        [HttpPatch("{orderId}/send-order-to-preparing")]
        public async Task<IActionResult> SendOrderToPreparing([FromRoute(Name = "orderId")] int orderId)
        {
            var response = await _orderService.SendOrderToPreparing(orderId);

            if(!response.Result.IsValid)
               return BadRequest(response.Result.Errors);

            return Ok(response.Entity);
        }

        [HttpPatch("{orderId}/send-order-to-out-for-delivery")]
        public async Task<IActionResult> SendOrderToOutForDelivery([FromRoute(Name = "orderId")] int orderId)
        {
            var response = await _orderService.SendOrderToOutForDelivery(orderId);

            if(!response.Result.IsValid)
               return BadRequest(response.Result.Errors);

            return Ok(response.Entity);
        }

        [HttpPatch("{orderId}/send-order-to-delivered")]
        public async Task<IActionResult> SendOrderToDeliveried([FromRoute(Name = "orderId")] int orderId)
        {
            var response = await _orderService.SendOrderToDelivered(orderId);

            if(!response.Result.IsValid)
               return BadRequest(response.Result.Errors);

            return Ok(response.Entity);
        }

        [HttpGet("order-by-status")]
        public async Task<IActionResult> GetOrdersByStatus()
        {
            var ordersByStatus = await _orderService.GetByOrderStatus();

            return Ok(ordersByStatus ?? new List<OrderStatus>()); 
        }
    }
}