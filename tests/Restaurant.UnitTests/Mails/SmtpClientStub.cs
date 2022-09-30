using Restaurant.Application.Mail;
using System.Collections.Generic;
using System.Net.Mail;
using System.Threading;
using System.Threading.Tasks;

namespace Restaurant.UnitTests.Mails
{
    internal sealed class SmtpClientStub : ISmtpClient
    {
        private readonly int _timeout = 0;
        private readonly List<MailMessage> _messages = new();

        public IEnumerable<MailMessage> Messages => _messages;

        public SmtpClientStub() { }

        public SmtpClientStub(int timeout)
        {
            _timeout = timeout;
        }

        public Task SendMailAsync(MailMessage mailMessage)
        {
            Thread.Sleep(_timeout);
            _messages.Add(mailMessage);
            return Task.CompletedTask;
        }
    }
}
