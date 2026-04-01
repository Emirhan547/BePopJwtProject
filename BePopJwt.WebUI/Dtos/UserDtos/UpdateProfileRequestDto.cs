namespace BePopJwt.WebUI.Dtos.UserDtos
{
    public class UpdateProfileRequestDto
    {
        public string UserName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string DisplayName { get; set; } = string.Empty;
        public string? ImageUrl { get; set; }
    }
}
