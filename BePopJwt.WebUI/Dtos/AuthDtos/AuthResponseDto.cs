namespace BePopJwt.WebUI.Dtos.AuthDtos
{
    public class AuthResponseDto
    {
        public int UserId { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string DisplayName { get; set; } = string.Empty;
        public string PackageName { get; set; } = string.Empty;
        public int PackageLevel { get; set; }
        public string Token { get; set; } = string.Empty;
        public DateTime ExpiresAtUtc { get; set; }
    }
}
