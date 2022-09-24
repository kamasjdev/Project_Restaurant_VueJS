using Restaurant.Application.Abstractions;
using Restaurant.Application.DTO;
using Restaurant.Application.Exceptions;
using Restaurant.Application.Mappings;
using Restaurant.Domain.Repositories;

namespace Restaurant.Application.Services
{
    internal sealed class AdditionService : IAdditionService
    {
        private readonly IAdditonRepository _additonRepository;

        public AdditionService(IAdditonRepository additonRepository)
        {
            _additonRepository = additonRepository;
        }

        public async Task AddAsync(AdditionDto additionDto)
        {
            additionDto.Id = Guid.NewGuid();
            var addition = additionDto.AsEntity();
            await _additonRepository.AddAsync(addition);
        }

        public async Task DeleteAsync(Guid id)
        {
            var addition = await _additonRepository.GetAsync(id);

            if (addition is null)
            {
                throw new AdditionNotFoundException(id);
            }

            if (addition.ProductSales.Any())
            {
                throw new CannotDeleteAdditionOrderedException(id);
            }

            await _additonRepository.DeleteAsync(addition);
        }

        public async Task<IEnumerable<AdditionDto>> GetAllAsync()
        {
            return (await _additonRepository.GetAllAsync()).Select(a => a.AsDto());
        }

        public async Task<AdditionDto> GetAsync(Guid id)
        {
            return (await _additonRepository.GetAsync(id))?.AsDto();
        }

        public async Task UpdateAsync(AdditionDto additionDto)
        {
            var addition = await _additonRepository.GetAsync(additionDto.Id);

            if (addition is null)
            {
                throw new AdditionNotFoundException(additionDto.Id);
            }

            addition.ChangeAdditionName(additionDto.AdditionName);
            addition.ChangePrice(additionDto.Price);

            await _additonRepository.UpdateAsync(addition);
        }
    }
}
