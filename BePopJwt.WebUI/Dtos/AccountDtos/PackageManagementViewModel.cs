using BePopJwt.WebUI.Dtos.AuthDtos;
using BePopJwt.WebUI.Dtos.PackageDtos;

namespace BePopJwt.WebUI.Dtos.AccountDtos
{
    public class PackageManagementViewModel
    {
        public UserSessionViewModel Session { get; set; } = new();
        public List<PackageDto> Packages { get; set; } = [];
        public string? ErrorMessage { get; set; }
        public string? SuccessMessage { get; set; }
    }
}
