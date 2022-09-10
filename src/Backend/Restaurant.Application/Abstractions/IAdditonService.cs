using Restaurant.Application.DTO;

namespace Restaurant.Application.Abstractions
{
    public interface IAdditonService
    {
        Task<AdditionDto> GetAsync(Guid id);
        Task<IEnumerable<AdditionDto>> GetAllAsync();
    }
}
