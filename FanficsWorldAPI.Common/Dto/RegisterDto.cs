namespace FanficsWorldAPI.Common.Dto
{
    public class RegisterDto
    {
        public string Name { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string? Bio { get; set; }

        public DateOnly BirthDate { get; set; }

        public string Password { get; set; } = string.Empty;
    }
}
