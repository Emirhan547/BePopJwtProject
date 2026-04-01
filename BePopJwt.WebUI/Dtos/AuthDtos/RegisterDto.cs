using BePopJwt.WebUI.Dtos.PackageDtos;

namespace BePopJwt.WebUI.Dtos.AuthDtos
{
    public sealed class RegisterDto
    {
        public RegisterRequestDto Request { get; set; } = new();
        public List<PackageDto> Packages { get; set; } = [];
        public string? ErrorMessage { get; set; }
    }
}
