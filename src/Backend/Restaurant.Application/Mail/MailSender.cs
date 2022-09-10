using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Restaurant.Application.Abstractions;
using Restaurant.Application.Exceptions;
using Restaurant.Domain.ValueObjects;
using System.Net;
using System.Net.Mail;

namespace Restaurant.Application.Mail
{
    internal sealed class MailSender : IMailSender
    {
        private readonly EmailSettings _settings;
        private readonly ILogger<MailSender> _logger;

        public MailSender(IOptionsMonitor<EmailSettings> settings, ILogger<MailSender> logger)
        {
            _settings = settings.CurrentValue;
            _logger = logger;
        }
        
        public async Task SendAsync(Email email, EmailMessage emailMessage)
        {
            if (!_settings.SendEmailAfterConfirmOrder)
            {
                _logger.LogInformation("MailSender Service is disabled");
                return;
            }

            ValidateSettings();
            var mail = new MailMessage(_settings.Email, email.Value);
            using (var smtp = new SmtpClient(_settings.SmtpClient, _settings.SmtpPort))
            {
                mail.Subject = emailMessage.Subject;
                mail.Body = emailMessage.Body;
                smtp.Timeout = 5200;
                smtp.UseDefaultCredentials = false;
                smtp.Credentials = new NetworkCredential(_settings.Login, _settings.Password);
                smtp.EnableSsl = true;

                var cancellationTokenSource = new CancellationTokenSource(smtp.Timeout);
                cancellationTokenSource.CancelAfter(smtp.Timeout);
                var cancellationToken = cancellationTokenSource.Token;

                try
                {
                    var source = new CancellationTokenSource();
                    source.CancelAfter(TimeSpan.FromSeconds(5));
                    var token = source.Token;
                    var tcs = new TaskCompletionSource<bool>();
                    cancellationToken.Register(s => ((TaskCompletionSource<bool>)s).SetResult(true), tcs);
                    var task = smtp.SendMailAsync(mail);
                    await Task.WhenAny(task, tcs.Task);
                    token.ThrowIfCancellationRequested();
                    await task;
                }
                catch(Exception exception)
                {
                    _logger.LogError(exception, exception.Message);
                    throw new CannotSendEmailException();
                }
            }
        }

        private void ValidateSettings()
        {
            if (string.IsNullOrWhiteSpace(_settings.Login))
            {
                throw new InvalidEmailSettingsException(nameof(EmailSettings.Login));
            }

            if (string.IsNullOrWhiteSpace(_settings.Email))
            {
                throw new InvalidEmailSettingsException(nameof(EmailSettings.Email));
            }

            if (string.IsNullOrWhiteSpace(_settings.Password))
            {
                throw new InvalidEmailSettingsException(nameof(EmailSettings.Password));
            }

            if (string.IsNullOrWhiteSpace(_settings.SmtpClient))
            {
                throw new InvalidEmailSettingsException(nameof(EmailSettings.SmtpClient));
            }

            try
            {
                Email.Of(_settings.Email);
            }
            catch(Exception exception)
            {
                throw new InvalidEmailSettingsException(nameof(EmailSettings.Email), exception.Message);
            }
        }
    }
}
