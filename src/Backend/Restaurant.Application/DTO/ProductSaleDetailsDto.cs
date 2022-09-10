namespace Restaurant.Application.DTO
{
    public class ProductSaleDetailsDto : ProductSaleDto
    {
        public OrderDto Order { get; set; }
    }
}
