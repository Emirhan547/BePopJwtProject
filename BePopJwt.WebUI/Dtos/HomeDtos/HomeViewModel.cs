using BePopJwt.WebUI.Dtos.ArtistDtos;
using BePopJwt.WebUI.Dtos.AuthDtos;
using BePopJwt.WebUI.Dtos.PackageDtos;
using BePopJwt.WebUI.Dtos.SongDtos;

namespace BePopJwt.WebUI.Dtos.HomeDtos
{
    public class HomeViewModel
    {
        public List<SongWithAlbumDto> Songs { get; set; } = [];
        public List<ArtistWithAlbumsDto> Artists { get; set; } = [];
        public List<PackageDto> Packages { get; set; } = [];
        public UserSessionViewModel Session { get; set; } = new();
    }
}
