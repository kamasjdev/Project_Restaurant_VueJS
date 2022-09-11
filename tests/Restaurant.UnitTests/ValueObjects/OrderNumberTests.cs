using Restaurant.Domain.Exceptions;
using Restaurant.Domain.ValueObjects;
using Shouldly;
using Xunit;

namespace Restaurant.UnitTests.ValueObjects
{
    public class OrderNumberTests
    {
        [Fact]
        public void should_create_order_number()
        {
            var number = "1234123";

            var orderNumber = new OrderNumber(number);

            orderNumber.ShouldNotBeNull();
            orderNumber.Value.ShouldBe(number);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public void given_invalid_order_number_should_throw_an_exception(string number)
        {
            var expectedException = new OrderNumberCannotBeEmptyException();
            var exception = Record.Exception(() => new OrderNumber(number));

            exception.ShouldNotBeNull();
            exception.ShouldBeOfType(expectedException.GetType());
            exception.Message.ShouldBe(expectedException.Message);
        }
    }
}
