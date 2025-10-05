using NUlid;

namespace FanficsWorldAPI.DataAccess.Entities
{
    public class User
    {
        public string Id { get; set; } = Ulid.NewUlid().ToString();

        public string Name { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string? Bio { get; set; }

        public DateOnly BirthDate { get; set; }

        public int RoleId { get; set; }

        public bool IsActive { get; set; }

        public short FailedLoginAttempts { get; set; }

        public string PasswordSalt { get; set; } = string.Empty;

        public string PasswordHash { get; set; } = string.Empty;

        public Role Role { get; set; } = null!;

        public List<Fanfic> Fanfics { get; set; } = [];
    }
}
