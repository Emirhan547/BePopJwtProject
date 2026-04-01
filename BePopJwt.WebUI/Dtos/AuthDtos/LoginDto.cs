namespace BePopJwt.WebUI.Dtos.AuthDtos
{
    public class LoginDto
    {
        public LoginRequestDto Request { get; set; } = new();
        public string? ErrorMessage { get; set; }
    }
}
