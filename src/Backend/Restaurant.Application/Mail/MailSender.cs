using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Restaurant.Application.Abstractions;
using Restaurant.Application.Exceptions;
using Restaurant.Domain.ValueObjects;
using System.Net.Mail;

namespace Restaurant.Application.Mail
{
    internal sealed class MailSender : IMailSender
    {
        private readonly EmailSettings _settings;
        private readonly ILogger<MailSender> _logger;
        private readonly ISmtpClient _smtpClient;

        public MailSender(IOptionsMonitor<EmailSettings> settings, ILogger<MailSender> logger, ISmtpClient smtpClient)
        {
            _settings = settings.CurrentValue;
            _logger = logger;
            _smtpClient = smtpClient;
        }
        
        public async Task SendAsync(Email email, IEmailMessage emailMessage)
        {
            if (!_settings.SendEmailAfterConfirmOrder)
            {
                _logger.LogInformation("MailSender Service is disabled");
                return;
            }

            ValidateSettings();
            var mailMessage = new MailMessage(_settings.Email, email.Value)
            {
                Subject = emailMessage.Subject,
                Body = emailMessage.Body
            };

            var cancellationTokenSource = new CancellationTokenSource(_settings.Timeout);
            cancellationTokenSource.CancelAfter(_settings.Timeout);
            var cancellationToken = cancellationTokenSource.Token;

            try
            {
                var source = new CancellationTokenSource();
                source.CancelAfter(TimeSpan.FromMilliseconds(_settings.Timeout));
                var token = source.Token;
                var tcs = new TaskCompletionSource<bool>();
                cancellationToken.Register(s => ((TaskCompletionSource<bool>)s).SetResult(true), tcs);
                var task = _smtpClient.SendMailAsync(mailMessage);
                await Task.WhenAny(task, tcs.Task);
                token.ThrowIfCancellationRequested();
                await task;
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, exception.Message);
                throw new CannotSendEmailException();
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
