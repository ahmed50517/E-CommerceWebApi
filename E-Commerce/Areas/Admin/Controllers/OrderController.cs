using E_Commerce.DTO;
using E_Commerce.Models;
using System.Security.Claims;
using E_Commerce.Repository.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using E_Commerce.Services;

namespace E_Commerce.Areas.Admin.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IUnitofWork _unitofWork;
        private readonly EmailService _emailService;
        private readonly UserService _userService;

        public OrderController(IUnitofWork unitofWork)
        {
            _unitofWork = unitofWork;
        }
        [HttpGet("myorders")]
        [Authorize(Roles = "Customer")]
        public IActionResult GetMyOrders()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized("User is not authenticated");
            }

            var orders = _unitofWork.Orders.GetAll().ToList();
            return Ok(orders);
        }
        [HttpGet("orders")]
        [Authorize(Roles = "Admin")]
        public IActionResult GetOrders()
        {
            var orders = _unitofWork.Orders.GetAll().ToList();
            return Ok(orders);
        }
        [HttpGet("orders/{id:int}")]
        [Authorize(Roles = "Admin")]
        public IActionResult GetOrderById(int id)
        {
            var order = _unitofWork.Orders.Get(o => o.Id == id);
            if (order == null)
            {
                return NotFound();
            }
            return Ok(order);
        }

        [HttpPut("orders/{id:int}/status")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateOrderStatus(int id, [FromBody] string newStatus)
        {
            var order = _unitofWork.Orders.Get(o => o.Id == id);
            if (order == null)
            {
                return NotFound($"Order with ID {id} not found");
            }

            order.Status = newStatus;
            _unitofWork.Orders.Update(order);
            _unitofWork.Save();

            return Ok(order);




            return Ok(order);
        }

        [HttpPost("placeorder")]
        [Authorize(Roles = "Customer")]
        public IActionResult PlaceOrder([FromBody] List<OrderItemDto> orderItemsDto)
        {
            if (orderItemsDto == null || !orderItemsDto.Any())
            {
                return BadRequest("Order items cannot be empty");
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized("User is not authenticated");
            }

            var order = new Order
            {
                OrderItems = new List<OrderItem>(),
                Status = "Pending", // Set initial status
                OrderDate = DateTime.UtcNow, // Set OrderDate to UTC
                CustomerId = userId // Set CustomerId from authenticated user
            };
            double totalPrice = 0;

            foreach (var itemDto in orderItemsDto)
            {
                var product = _unitofWork.Products.Get(p => p.Id == itemDto.ProductId);
                if (product == null)
                {
                    return NotFound($"Product with ID {itemDto.ProductId} not found");
                }

                if (!product.IsAvailable)
                {
                    return BadRequest($"Product with ID {itemDto.ProductId} is not available");
                }

                var orderItem = new OrderItem
                {
                    ProductId = itemDto.ProductId,
                    Quantity = itemDto.Quantity,
                    Price = product.Price * itemDto.Quantity
                };

                totalPrice += orderItem.Price;
                order.OrderItems.Add(orderItem);

                // Update product stock
                product.Stock -= itemDto.Quantity;
                if (product.Stock == 0)
                {
                    product.IsAvailable = false;
                }
                _unitofWork.Products.Update(product);
            }

            order.TotalPrice = totalPrice;
            _unitofWork.Orders.Add(order);
            _unitofWork.Save();

            return Ok(order);
        }

    }
}
