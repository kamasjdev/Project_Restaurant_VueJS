using Restaurant.Domain.Exceptions;
using Restaurant.Domain.ValueObjects;
using Shouldly;
using System;
using Xunit;

namespace Restaurant.UnitTests.ValueObjects
{
    public class EntityIdTests
    {
        [Fact]
        public void should_create_entity_id()
        {
            var id = Guid.NewGuid();

            var entityId = new EntityId(id);

            entityId.Value.ShouldBe(id);
        }

        [Fact]
        public void given_empty_guid_should_throw_an_exception()
        {
            var id = Guid.Empty;
            var expectedException = new InvalidEntityIdException(id);

            var exception = Record.Exception(() => new EntityId(id));

            exception.ShouldNotBeNull();
            exception.ShouldBeOfType(expectedException.GetType());
            exception.Message.ShouldBe(expectedException.Message);
            ((InvalidEntityIdException)exception).Id.ShouldBe(expectedException.Id);
        }
    }
}
