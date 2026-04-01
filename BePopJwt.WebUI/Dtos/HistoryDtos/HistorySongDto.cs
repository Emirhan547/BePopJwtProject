namespace BePopJwt.WebUI.Dtos.HistoryDtos
{
    public class HistorySongDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string CoverUrl { get; set; } = string.Empty;
        public int AlbumId { get; set; }
    }
}
