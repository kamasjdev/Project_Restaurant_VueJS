using Restaurant.Application.Mail;
using System.Net.Mail;
using System.Threading;
using System.Threading.Tasks;

namespace Restaurant.UnitTests.Mails
{
    internal sealed class SmtpClientStub : ISmtpClient
    {
        private readonly int _timeout;

        public SmtpClientStub(int timeout)
        {
            _timeout = timeout;
        }

        public Task SendMailAsync(MailMessage mailMessage)
        {
            Thread.Sleep(_timeout);
            return Task.CompletedTask;
        }
    }
}
