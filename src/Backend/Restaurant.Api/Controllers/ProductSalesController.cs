using Microsoft.AspNetCore.Mvc;
using Restaurant.Application.Abstractions;
using Restaurant.Application.DTO;

namespace Restaurant.Api.Controllers
{
    public class ProductSalesController : BaseController
    {
        private readonly IProductSaleService _productSaleService;

        public ProductSalesController(IProductSaleService productSaleService)
        {
            _productSaleService = productSaleService;
        }

        [HttpGet("{productSaleId}")]
        public async Task<ActionResult<ProductSaleDto>> Get(Guid productSaleId)
        {
            return OkOrNotFound(await _productSaleService.GetAsync(productSaleId));
        }

        [HttpGet("by-email/{email}")]
        public async Task<IEnumerable<ProductSaleDto>> GetAllByEmail(string email)
        {
            return await _productSaleService.GetAllByEmailAsync(email);
        }

        [HttpGet("by-order/{orderId:guid}")]
        public async Task<IEnumerable<ProductSaleDto>> GetAllByOrderId(Guid orderId)
        {
            return await _productSaleService.GetAllByOrderIdAsync(orderId);
        }

        [HttpPost]
        public async Task<ActionResult> Add(AddProductSaleDto addProductSaleDto)
        {
            await _productSaleService.AddAsync(addProductSaleDto);
            return CreatedAtAction(nameof(Get), new { productSaleId = addProductSaleDto.Id }, null);
        }

        [HttpPut("{productSaleId:guid}")]
        public async Task<ActionResult> Update(Guid productSaleId, AddProductSaleDto addProductSaleDto)
        {
            addProductSaleDto.Id = productSaleId;
            await _productSaleService.UpdateAsync(addProductSaleDto);
            return NoContent();
        }

        [HttpDelete("{productSaleId:guid}")]
        public async Task<ActionResult> Delete(Guid productSaleId)
        {
            await _productSaleService.DeleteAsync(productSaleId);
            return NoContent();
        }
    }
}
