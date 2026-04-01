using BePopJwt.WebUI.Dtos.AlbumDtos;

namespace BePopJwt.WebUI.Dtos.ArtistDtos
{
    public class ArtistWithAlbumsDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string ImageUrl { get; set; } = string.Empty;
        public string CoverUrl { get; set; } = string.Empty;
        public string Bio { get; set; } = string.Empty;
        public List<AlbumDto> Albums { get; set; } = [];
    }
}
