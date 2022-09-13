using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NSubstitute;
using Restaurant.Application.Exceptions;
using Restaurant.Application.Mail;
using Restaurant.Domain.Entities;
using Restaurant.Domain.ValueObjects;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;
using Xunit;
using static Restaurant.Application.Mail.EmailMessage;

namespace Restaurant.UnitTests.Mails
{
    public class MailSenderTests
    {
        [Fact]
        public async Task should_send_email()
        {
            var email = Email.Of("email@test.com");
            _emailSettings.CurrentValue.Returns(new EmailSettings { SendEmailAfterConfirmOrder = true, Login = "login", Email = email.Value, Password = "password", SmtpClient = "smtpClient", SmtpPort = 100, Timeout = 100 });
            var mailMessage = _emailMessageBuilder.ConstructEmailFromOrder(CreateDefaultOrderWithPositions());
            var mailSender = new MailSender(_emailSettings, _logger, _smtpClient);

            var exception = await Record.ExceptionAsync(() => mailSender.SendAsync(email, mailMessage));

            exception.ShouldBeNull();
            await _smtpClient.Received(1).SendMailAsync(Arg.Any<MailMessage>());
        }

        [Fact]
        public async Task given_options_disabled_mail_shouldnt_send()
        {
            var email = Email.Of("email@test.com");
            _emailSettings.CurrentValue.Returns(new EmailSettings { SendEmailAfterConfirmOrder = false });
            var mailMessage = _emailMessageBuilder.ConstructEmailFromOrder(CreateDefaultOrderWithPositions());
            var mailSender = new MailSender(_emailSettings, _logger, _smtpClient);

            await mailSender.SendAsync(email, mailMessage);

            await _smtpClient.DidNotReceiveWithAnyArgs().SendMailAsync(Arg.Any<MailMessage>());
        }

        [Fact]
        public async Task given_invalid_login_should_throw_an_exception()
        {
            var email = Email.Of("email@test.com");
            _emailSettings.CurrentValue.Returns(new EmailSettings { SendEmailAfterConfirmOrder = true });
            var mailMessage = _emailMessageBuilder.ConstructEmailFromOrder(CreateDefaultOrderWithPositions());
            var mailSender = new MailSender(_emailSettings, _logger, _smtpClient);
            var expectedException = new InvalidEmailSettingsException(nameof(EmailSettings.Login));

            var exception = await Record.ExceptionAsync(() => mailSender.SendAsync(email, mailMessage));

            exception.ShouldNotBeNull();
            exception.ShouldBeOfType(expectedException.GetType());
            exception.Message.ShouldBe(expectedException.Message);
            ((InvalidEmailSettingsException)exception).Field.ShouldBe(expectedException.Field);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("  ")]
        [InlineData("abc")]
        [InlineData("abc1234124@")]
        public async Task given_empty_email_should_throw_an_exception(string emailSettings)
        {
            var email = Email.Of("email@test.com");
            _emailSettings.CurrentValue.Returns(new EmailSettings { SendEmailAfterConfirmOrder = true, Login = "login", Email = emailSettings, Password = "password", SmtpClient = "test", SmtpPort = 100 });
            var mailMessage = _emailMessageBuilder.ConstructEmailFromOrder(CreateDefaultOrderWithPositions());
            var mailSender = new MailSender(_emailSettings, _logger, _smtpClient);
            var expectedException = new InvalidEmailSettingsException(nameof(EmailSettings.Email));

            var exception = await Record.ExceptionAsync(() => mailSender.SendAsync(email, mailMessage));

            exception.ShouldNotBeNull();
            exception.ShouldBeOfType(expectedException.GetType());
            exception.Message.ShouldContain(expectedException.Message);
            ((InvalidEmailSettingsException)exception).Field.ShouldBe(expectedException.Field);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("  ")]
        public async Task given_empty_password_should_throw_an_exception(string password)
        {
            var email = Email.Of("email@test.com");
            _emailSettings.CurrentValue.Returns(new EmailSettings { SendEmailAfterConfirmOrder = true, Login = "login", Email = email.Value, Password = password, SmtpClient = "test", SmtpPort = 100 });
            var mailMessage = _emailMessageBuilder.ConstructEmailFromOrder(CreateDefaultOrderWithPositions());
            var mailSender = new MailSender(_emailSettings, _logger, _smtpClient);
            var expectedException = new InvalidEmailSettingsException(nameof(EmailSettings.Password));

            var exception = await Record.ExceptionAsync(() => mailSender.SendAsync(email, mailMessage));

            exception.ShouldNotBeNull();
            exception.ShouldBeOfType(expectedException.GetType());
            exception.Message.ShouldContain(expectedException.Message);
            ((InvalidEmailSettingsException)exception).Field.ShouldBe(expectedException.Field);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("  ")]
        public async Task given_empty_smtp_client_should_throw_an_exception(string smtpClient)
        {
            var email = Email.Of("email@test.com");
            _emailSettings.CurrentValue.Returns(new EmailSettings { SendEmailAfterConfirmOrder = true, Login = "login", Email = email.Value, Password = "password", SmtpClient = smtpClient, SmtpPort = 100 });
            var mailMessage = _emailMessageBuilder.ConstructEmailFromOrder(CreateDefaultOrderWithPositions());
            var mailSender = new MailSender(_emailSettings, _logger, _smtpClient);
            var expectedException = new InvalidEmailSettingsException(nameof(EmailSettings.SmtpClient));

            var exception = await Record.ExceptionAsync(() => mailSender.SendAsync(email, mailMessage));

            exception.ShouldNotBeNull();
            exception.ShouldBeOfType(expectedException.GetType());
            exception.Message.ShouldContain(expectedException.Message);
            ((InvalidEmailSettingsException)exception).Field.ShouldBe(expectedException.Field);
        }

        [Fact]
        public async Task cannot_send_email_timeout_exceeded()
        {
            var email = Email.Of("email@test.com");
            _emailSettings.CurrentValue.Returns(new EmailSettings { SendEmailAfterConfirmOrder = true, Login = "login", Email = email.Value, Password = "password", SmtpClient = "smtpClient", SmtpPort = 100 });
            var mailMessage = _emailMessageBuilder.ConstructEmailFromOrder(CreateDefaultOrderWithPositions());
            var smtpClient = new SmtpClientStub(20);
            var expectedException = new CannotSendEmailException();
            var mailSender = new MailSender(_emailSettings, _logger, smtpClient);

            var exception = await Record.ExceptionAsync(() => mailSender.SendAsync(email, mailMessage));

            exception.ShouldNotBeNull();
            exception.ShouldBeOfType(expectedException.GetType());
            exception.Message.ShouldContain(expectedException.Message);
        }

        private Order CreateDefaultOrderWithPositions()
        {
            var productSales = new List<ProductSale> { CreateDefaultProductSale(), CreateDefaultProductSale() };
            var order = new Order(Guid.NewGuid(), "ORDER", _currentDate, productSales.Sum(p => p.EndPrice), Email.Of("email@email.com"), products: productSales);
            return order;
        }

        private ProductSale CreateDefaultProductSale()
        {
            var product = CreateDefaultProduct();
            var productSale = new ProductSale(Guid.NewGuid(), product, ProductSaleState.Ordered, Email.Of("email@email.com"));
            return productSale;
        }

        private Product CreateDefaultProduct()
        {
            var product = new Product(Guid.NewGuid(), Guid.NewGuid().ToString("N"), 10, ProductKind.MainDish);
            return product;
        }

        private readonly DateTime _currentDate;
        private readonly ILogger<MailSender> _logger;
        private readonly IOptionsMonitor<EmailSettings> _emailSettings;
        private readonly EmailMessageBuilder _emailMessageBuilder;
        private readonly ISmtpClient _smtpClient;


        public MailSenderTests()
        {
            _currentDate = new DateTime(2022, 9, 13, 18, 20, 30);
            _emailSettings = Substitute.For<IOptionsMonitor<EmailSettings>>();
            _logger = Substitute.For<ILogger<MailSender>>();
            _emailMessageBuilder = new EmailMessageBuilder();
            _smtpClient = Substitute.For<ISmtpClient>();
        }
    }
}
