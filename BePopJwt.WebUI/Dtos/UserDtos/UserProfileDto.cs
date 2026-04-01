namespace BePopJwt.WebUI.Dtos.UserDtos
{
    public class UserProfileDto
    {
        public int UserId { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string DisplayName { get; set; } = string.Empty;
        public string? ImageUrl { get; set; }
        public int PackageId { get; set; }
        public string PackageName { get; set; } = string.Empty;
        public int PackageLevel { get; set; }
    }
}
