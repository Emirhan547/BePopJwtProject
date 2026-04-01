namespace BePopJwt.WebUI.Dtos.HistoryDtos
{
    public class UserHistoryDto
    {
        public int Id { get; set; }
        public int SongId { get; set; }
        public DateTime PlayedAt { get; set; }
        public int PlayDuration { get; set; }
        public HistorySongDto Song { get; set; } = new();
    }
}
