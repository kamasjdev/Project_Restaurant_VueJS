namespace Restaurant.Application.DTO
{
    public class ProductSaleDto
    {
        public Guid Id { get; set; }
        public AdditionDto Addition { get; set; }
        public Guid? AdditionId { get; set; }
        public ProductDto Product { get; set; }
        public Guid ProductId { get; set; }
        public decimal EndPrice { get; set; }
        public Guid? OrderId { get; set; }
        public string ProductSaleState { get; set; }
        public string Email { get; set; }
    }
}
