namespace BePopJwt.WebUI.Dtos.SongDtos
{
    public class SongWithAlbumDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int Duration { get; set; }
        public string FilePath { get; set; } = string.Empty;
        public string CoverUrl { get; set; } = string.Empty;
        public int Level { get; set; }
         public int AlbumId { get; set; }
        public SongAlbumSummaryDto? Album { get; set; }
    }
}
