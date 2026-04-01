using BePopJwt.WebUI.Dtos.SongDtos;

namespace BePopJwt.WebUI.Dtos.HomeDtos
{
    public class DiscoverViewModel
    {
        public List<SongWithAlbumDto> AccessibleSongs { get; set; } = [];
        public bool IsAuthenticated { get; set; }
        public string? ErrorMessage { get; set; }
    }
}
