namespace Restaurant.Application.Exceptions
{
    public sealed class InvalidEmailSettingsException : ApplicationException
    {
        public string Field { get; }

        public InvalidEmailSettingsException(string field) : base($"Invalid EmailSettings for property: '{field}'")
        {
            Field = field;
        }

        public InvalidEmailSettingsException(string field, string detailMessage) : base($"Invalid EmailSettings for property: '{field}' with descripted messge: {detailMessage}")
        {
            Field = field;
        }
    }
}
