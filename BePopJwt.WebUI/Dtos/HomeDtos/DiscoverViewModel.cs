using BePopJwt.WebUI.Dtos.AuthDtos;
using BePopJwt.WebUI.Dtos.HistoryDtos;
using BePopJwt.WebUI.Dtos.SongDtos;

namespace BePopJwt.WebUI.Dtos.HomeDtos
{
    public class DiscoverViewModel
    {
        public List<SongWithAlbumDto> AccessibleSongs { get; set; } = [];
        public List<UserHistoryDto> History { get; set; } = [];
        public List<SongWithAlbumDto> RecommendedSongs { get; set; } = [];
        public UserSessionViewModel Session { get; set; } = new();
        public string? ErrorMessage { get; set; }
    }
}
