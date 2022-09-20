namespace Restaurant.Infrastructure.Mappings
{
    public class UserPoco
    {
        public virtual Guid Id { get; set; }
        public virtual string Email { get; set; }
        public virtual string Password { get; set; }
        public virtual string Role { get; set; }
        public virtual DateTime CreatedAt { get; set; }
    }
}
