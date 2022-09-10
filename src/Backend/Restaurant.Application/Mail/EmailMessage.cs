namespace Restaurant.Application.Mail
{
    public sealed class EmailMessage
    {
        private readonly string _subject;
        private readonly string _body;

        public string Subject => _subject;
        public string Body => _body;

        public EmailMessage(string subject, string body)
        {
            if (string.IsNullOrWhiteSpace(subject))
            {
                throw new RestaurantServerException("Subject cannot be empty", typeof(EmailMessage).FullName, "PrepareEmailSend");
            }

            if (string.IsNullOrWhiteSpace(body))
            {
                throw new RestaurantServerException("Body cannot be empty", typeof(EmailMessage).FullName, "PrepareEmailSend");
            }

            _subject = subject;
            _body = body;
        }
    }
}
