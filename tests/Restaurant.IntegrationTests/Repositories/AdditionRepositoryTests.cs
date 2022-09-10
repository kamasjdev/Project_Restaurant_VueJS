using Microsoft.Extensions.DependencyInjection;
using Restaurant.Domain.Entities;
using Restaurant.Domain.Repositories;
using Restaurant.IntegrationTests.Common;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Restaurant.IntegrationTests.Repositories
{
    public class AdditionRepositoryTests : BaseIntegrationTest
    {
        [Fact]
        public async Task should_add_addition()
        {
            var addition = new Addition(Guid.NewGuid(), "Addition#1", 20, AdditionKind.Drink);

            await _additionRepository.AddAsync(addition);

            var additionAdded = await _additionRepository.GetAsync(addition.Id);
            Assert.NotNull(additionAdded);
        }

        [Fact]
        public async Task should_update_addition()
        {
            var addition = await AddDefaultAdditionAsync();
            addition.ChangeAdditionName("Addition#2022");
            addition.ChangePrice(1);

            await _additionRepository.UpdateAsync(addition);

            var additionUpdated = await _additionRepository.GetAsync(addition.Id);
            Assert.NotNull(additionUpdated);
            Assert.Equal(addition.AdditionName.Value, additionUpdated.AdditionName.Value);
            Assert.Equal(addition.Price.Value, additionUpdated.Price.Value);
        }

        [Fact]
        public async Task should_delete_addition()
        {
            var addition = await AddDefaultAdditionAsync();

            await _additionRepository.DeleteAsync(addition);

            var additionDeleted = await _additionRepository.GetAsync(addition.Id);
            Assert.Null(additionDeleted);
        }

        [Fact]
        public async Task should_get_all_additions()
        {
            await AddDefaultAdditionAsync();
            await AddDefaultAdditionAsync();

            var additions = await _additionRepository.GetAllAsync();

            Assert.NotNull(additions);
            Assert.NotEmpty(additions);
            Assert.True(additions.Count() > 0);
        }

        private async Task<Addition> AddDefaultAdditionAsync()
        {
            var addition = new Addition(Guid.NewGuid(), $"Addition{Guid.NewGuid()}", 20, AdditionKind.Drink);
            await _additionRepository.AddAsync(addition);
            return addition;
        }

        private readonly IAdditonRepository _additionRepository;

        public AdditionRepositoryTests(TestApplicationFactory<Program> factory)
        {
            _additionRepository = factory.Services.GetRequiredService<IAdditonRepository>();
        }
    }
}
