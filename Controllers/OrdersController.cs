using GroveStart.Dtos.Order;
using GroveStart.Model;
using GroveStart.Services;
using Microsoft.AspNetCore.Mvc;

namespace GroveStart.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrdersController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var orders = await _orderService.GetAllOrdersAsync();
            return Ok(orders);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var order = await _orderService.GetOrderByIdAsync(id);
            if (order == null) return NotFound();
            return Ok(order);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] RegisterOrderRequest request)
        {
            if (!ModelState.IsValid)
                return ValidationProblem(ModelState);

            try
            {
                var createdOrder = await _orderService.CreateOrderAsync(request);
                return CreatedAtAction(nameof(GetById), new { id = createdOrder.Id }, createdOrder);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] Order order)
        {
            try
            {
                // O ID vem da URL, não do body
                var existingOrder = await _orderService.GetOrderByIdAsync(id);
                if (existingOrder == null) return NotFound();
                await _orderService.UpdateOrderAsync(existingOrder);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _orderService.DeleteOrderAsync(id);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("period/{period}")]
        public async Task<IActionResult> GetByPeriod(Period period)
        {
            var orders = await _orderService.GetOrdersByPeriodAsync(period);
            return Ok(orders);
        }

        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetByUser(int userId)
        {
            var orders = await _orderService.GetOrdersByUserAsync(userId);
            return Ok(orders);
        }
    }
}