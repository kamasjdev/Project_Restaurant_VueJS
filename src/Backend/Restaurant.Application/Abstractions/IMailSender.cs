using Restaurant.Application.Mail;
using Restaurant.Domain.ValueObjects;

namespace Restaurant.Application.Abstractions
{

    public interface IMailSender
    {
        Task SendAsync(Email email, EmailMessage emailMessage);
    }
}
