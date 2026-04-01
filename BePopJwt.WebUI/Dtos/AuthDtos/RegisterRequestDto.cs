namespace BePopJwt.WebUI.Dtos.AuthDtos
{
    public class RegisterRequestDto
    {
        public string UserName { get; set; } 
        public string Email { get; set; } 
        public string DisplayName { get; set; } 
        public string? ImageUrl { get; set; }
        public string Password { get; set; } 
        public int PackageId { get; set; }
    }
}
