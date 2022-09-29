using Flurl.Http;
using Microsoft.Extensions.DependencyInjection;
using Restaurant.Application.DTO;
using Restaurant.Domain.Entities;
using Restaurant.Domain.Repositories;
using Restaurant.IntegrationTests.Common;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Xunit;

namespace Restaurant.IntegrationTests.Controllers
{
    public class AdditionControllerTests : BaseIntegrationTest
    {
        [Fact]
        public async Task should_add_addition_and_return_201_with_id()
        {
            var dto = new AdditionDto { AdditionName = "Addition#1", Price = 20, AdditionKind = AdditionKind.Salad.ToString() };

            var response = await _client.Request(Path).PostJsonAsync(dto);

            response.ShouldNotBeNull();
            response.StatusCode.ShouldBe((int)HttpStatusCode.Created);
            var id = GetIdFromHeader(response, Path);
            id.ShouldNotBe(Guid.Empty);
        }

        [Fact]
        public async Task should_add_addition_and_return_from_db()
        {
            var dto = new AdditionDto { AdditionName = "Addition#1", Price = 20, AdditionKind = AdditionKind.Salad.ToString() };

            var response = await _client.Request(Path).PostJsonAsync(dto);

            response.ShouldNotBeNull();
            var id = GetIdFromHeader(response, Path);
            var additionAdded = await _client.Request($"{Path}/{id}").GetJsonAsync<AdditionDto>();
            additionAdded.ShouldNotBeNull();
            additionAdded.Id.ShouldBe(id);
            additionAdded.Price.ShouldBe(dto.Price);
            additionAdded.AdditionName.ShouldBe(dto.AdditionName);
            additionAdded.AdditionKind.ShouldBe(dto.AdditionKind);
        }

        [Fact]
        public async Task should_update_addition_and_return_staus_code_204()
        {
            var addition = await AddDefaultAdditionAsync();
            var dto = new AdditionDto { AdditionName = "AdName123", Price = 19.99M, AdditionKind = AdditionKind.Drink.ToString() };

            var response = await _client.Request($"{Path}/{addition.Id.Value}").PutJsonAsync(dto);

            response.StatusCode.ShouldBe((int)HttpStatusCode.NoContent);
        }

        [Fact]
        public async Task should_update_addition_and_return_from_db()
        {
            var addition = await AddDefaultAdditionAsync();
            var dto = new AdditionDto { AdditionName = "AdaName123", Price = 15.99M, AdditionKind = AdditionKind.Drink.ToString() };

            await _client.Request($"{Path}/{addition.Id.Value}").PutJsonAsync(dto);

            var additionUpdated = await _client.Request($"{Path}/{addition.Id.Value}").GetJsonAsync<AdditionDto>();
            additionUpdated.ShouldNotBeNull();
            additionUpdated.Id.ShouldBe(addition.Id.Value);
            additionUpdated.Price.ShouldBe(dto.Price);
            additionUpdated.AdditionName.ShouldBe(dto.AdditionName);
            additionUpdated.AdditionKind.ShouldBe(dto.AdditionKind);
        }

        [Fact]
        public async Task should_delete_addition_and_return_200()
        {
            var addition = await AddDefaultAdditionAsync();

            await _client.Request($"{Path}/{addition.Id.Value}").DeleteAsync();

            var additionDeleted = await _additionRepository.GetAsync(addition.Id.Value);
            additionDeleted.ShouldBeNull();
        }

        [Fact]
        public async Task should_get_addition_and_return_200()
        {
            var addition = await AddDefaultAdditionAsync();

            var response = await _client.Request($"{Path}/{addition.Id.Value}").GetAsync();
            var dto = await response.ResponseMessage.Content.ReadFromJsonAsync<AdditionDto>();

            response.ShouldNotBeNull();
            response.StatusCode.ShouldBe((int)HttpStatusCode.OK);
            dto.ShouldNotBeNull();
            dto.Id.ShouldBe(addition.Id.Value);
            dto.Price.ShouldBe(addition.Price.Value);
            dto.AdditionName.ShouldBe(addition.AdditionName.Value);
            dto.AdditionKind.ShouldBe(addition.AdditionKind.ToString());
        }

        [Fact]
        public async Task should_get_all_additions()
        {
            await AddDefaultAdditionAsync();
            await AddDefaultAdditionAsync();
            await AddDefaultAdditionAsync();

            var response = await _client.Request(Path).GetAsync();
            var dtos = await response.ResponseMessage.Content.ReadFromJsonAsync<IEnumerable<AdditionDto>>();

            response.ShouldNotBeNull();
            response.StatusCode.ShouldBe((int)HttpStatusCode.OK);
            dtos.ShouldNotBeNull();
            dtos.ShouldNotBeEmpty();
            dtos.Count().ShouldBeGreaterThan(0);
        }

        private async Task<Addition> AddDefaultAdditionAsync()
        {
            var random = new Random();
            var cost = random.Next(1, 20);
            var additionKind = random.Next(0, 2) == 1 ? AdditionKind.Salad : AdditionKind.Drink;
            var addition = new Addition(Guid.NewGuid(), $"Addition{Guid.NewGuid().ToString("N")}", cost, additionKind);
            await _additionRepository.AddAsync(addition);
            return addition;
        }

        private const string Path = "/api/additions";
        private IAdditonRepository _additionRepository;

        public AdditionControllerTests(TestApplicationFactory<Program> factory) : base(factory)
        {
            _additionRepository = GetRequiredService<IAdditonRepository>();
        }
    }
}
