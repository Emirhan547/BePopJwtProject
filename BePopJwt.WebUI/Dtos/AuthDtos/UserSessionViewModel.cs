namespace BePopJwt.WebUI.Dtos.AuthDtos
{
    public class UserSessionViewModel
    {
        public bool IsAuthenticated { get; set; }
        public string? UserName { get; set; }
        public string? PackageName { get; set; }
        public int? PackageLevel { get; set; }
        
        public string? Token { get; set; }
    }
}
