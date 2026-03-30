using BePopJwt.Business.Dtos.ArtistDtos;
using BePopJwt.Business.Dtos.Common;
using BePopJwt.Entity.Entities;


namespace BePopJwt.Business.Dtos.AlbumDtos
{
    public class ResultAlbumDto:BaseDto
    {
        public string Name { get; set; }

        public string CoverUrl { get; set; }
        public DateTime ReleaseDate { get; set; } = DateTime.Now;

        public int ArtistId { get; set; }
        public ResultArtistDto Artist { get; set; }

        public List<Song> Songs { get; set; }
    }
}
