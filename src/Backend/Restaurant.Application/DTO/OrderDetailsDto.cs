namespace Restaurant.Application.DTO
{
    public class OrderDetailsDto : OrderDto
    {
        public IList<ProductSaleDto> Products { get; set; } = new List<ProductSaleDto>();
    }
}
