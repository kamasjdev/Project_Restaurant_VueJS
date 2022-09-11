using Restaurant.Application.Abstractions;
using Restaurant.Application.DTO;
using Restaurant.Application.Exceptions;
using Restaurant.Application.Mappings;
using Restaurant.Domain.Entities;
using Restaurant.Domain.Repositories;
using Restaurant.Domain.ValueObjects;

namespace Restaurant.Application.Services
{
    internal sealed class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IProductSaleRepository _productSaleRepository;
        private readonly IClock _clock;

        public OrderService(IOrderRepository orderRepository, IProductSaleRepository productSaleRepository, IClock clock)
        {
            _orderRepository = orderRepository;
            _productSaleRepository = productSaleRepository;
            _clock = clock;
        }
        public async Task AddAsync(AddOrderDto addOrderDto)
        {
            var productSales = new List<ProductSale>();

            foreach (var productSaleId in addOrderDto.ProductSaleIds)
            {
                var productSale = await _productSaleRepository.GetAsync(productSaleId);

                if (productSale is null)
                {
                    throw new ProductSaleNotFoundException(productSaleId);
                }

                productSales.Add(productSale);
            }

            var order = new Order(Guid.NewGuid(), Guid.NewGuid().ToString(), _clock.CurrentDate(), 
                productSales.Sum(p => p.EndPrice), Email.Of(addOrderDto.Email), addOrderDto.Note, productSales);
            addOrderDto.Id = order.Id;
            await _orderRepository.AddAsync(order);

            foreach (var product in productSales)
            {
                await _productSaleRepository.UpdateAsync(product);
            }
        }

        public async Task DeleteAsync(Guid id)
        {
            var order = await _orderRepository.GetAsync(id);

            if (order is null)
            {
                throw new OrderNotFoundException(id);
            }

            foreach(var product in order.Products)
            {
                product.RemoveOrder();
            }

            await _orderRepository.DeleteAsync(order);

            foreach (var product in order.Products)
            {
                await _productSaleRepository.UpdateAsync(product);
            }
        }

        public async Task DeleteWithPositionsAsync(Guid id)
        {
            var order = await _orderRepository.GetAsync(id);

            if (order is null)
            {
                throw new OrderNotFoundException(id);
            }

            await _orderRepository.DeleteAsync(order);

            foreach (var product in order.Products)
            {
                await _productSaleRepository.DeleteAsync(product);
            }
        }

        public async Task<IEnumerable<OrderDto>> GetAllAsync()
        {
            return (await _orderRepository.GetAllAsync()).Select(o => o.AsDto());
        }

        public async Task<OrderDetailsDto> GetAsync(Guid id)
        {
            var order = await _orderRepository.GetAsync(id);
            return order?.AsDetailsDto();
        }

        public async Task UpdateAsync(AddOrderDto addOrderDto)
        {
            var order = await _orderRepository.GetAsync(addOrderDto.Id);

            if (order is null)
            {
                throw new OrderNotFoundException(addOrderDto.Id);
            }

            order.ChangeEmail(Email.Of(addOrderDto.Email));
            order.ChangeNote(addOrderDto.Note);

            foreach(var productSaleId in addOrderDto.ProductSaleIds)
            {
                var productSaleExists = order.Products.SingleOrDefault(p => p.Id == productSaleId);

                if (productSaleExists is not null)
                {
                    continue;
                }

                var productSale = await _productSaleRepository.GetAsync(productSaleId);

                if (productSale is null)
                {
                    throw new ProductSaleNotFoundException(productSaleId);
                }

                order.AddProduct(productSale);
            }

            foreach (var productSale in order.Products)
            {
                var productSaleIdExists = addOrderDto.ProductSaleIds.Any(p => p == productSale.Id);

                if (productSaleIdExists)
                {
                    continue;
                }

                order.RemoveProduct(productSale);
            }

            order.ChangePrice(order.Products.Sum(o => o.EndPrice));
            await _orderRepository.UpdateAsync(order);
        }
    }
}
