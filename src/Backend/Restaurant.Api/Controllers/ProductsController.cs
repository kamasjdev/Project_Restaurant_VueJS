using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Restaurant.Application.Abstractions;
using Restaurant.Application.DTO;

namespace Restaurant.Api.Controllers
{
    public class ProductsController : BaseController
    {
        private readonly IProductService _productService;

        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet]
        public async Task<IEnumerable<ProductDto>> GetAll()
        {
            return await _productService.GetAllAsync();
        }

        [HttpGet("{productId:guid}")]
        public async Task<ActionResult<ProductDetailsDto>> Get(Guid productId)
        {
            return OkOrNotFound(await _productService.GetAsync(productId));
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResult> Add(ProductDto productDto)
        {
            await _productService.AddAsync(productDto);
            return CreatedAtAction(nameof(Get), new { productId = productDto.Id }, null);
        }

        [Authorize]
        [HttpPut("{productId:guid}")]
        public async Task<ActionResult> Update(Guid productId, ProductDto productDto)
        {
            productDto.Id = productId;
            await _productService.UpdateAsync(productDto);
            return NoContent();
        }

        [Authorize]
        [HttpDelete("{productId:guid}")]
        public async Task<ActionResult> Delete(Guid productId)
        {
            await _productService.DeleteAsync(productId);
            return NoContent();
        }
    }
}
