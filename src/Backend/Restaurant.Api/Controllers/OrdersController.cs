using Microsoft.AspNetCore.Mvc;
using Restaurant.Application.Abstractions;
using Restaurant.Application.DTO;

namespace Restaurant.Api.Controllers
{
    public class OrdersController : BaseController
    {
        private readonly IOrderService _orderService;

        public OrdersController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpGet]
        public async Task<IEnumerable<OrderDto>> GetAll()
        {
            return await _orderService.GetAllAsync();
        }

        [HttpGet("{orderId:guid}")]
        public async Task<ActionResult<OrderDetailsDto>> Get(Guid orderId)
        {
            return OkOrNotFound(await _orderService.GetAsync(orderId));
        }

        [HttpPost]
        public async Task<ActionResult> Add(AddOrderDto addOrderDto)
        {
            await _orderService.AddAsync(addOrderDto);
            return CreatedAtAction(nameof(Get), new { orderId = addOrderDto.Id }, null);
        }

        [HttpPut("{orderId:guid}")]
        public async Task<ActionResult> Update(Guid orderId, AddOrderDto addOrderDto)
        {
            addOrderDto.Id = orderId;
            await _orderService.UpdateAsync(addOrderDto);
            return NoContent();
        }

        [HttpDelete("{orderId:guid}")]
        public async Task<ActionResult> Delete(Guid orderId)
        {
            await _orderService.DeleteAsync(orderId);
            return NoContent();
        }
    }
}
