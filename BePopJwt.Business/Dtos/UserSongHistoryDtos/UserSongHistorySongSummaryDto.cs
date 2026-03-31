using System;
using System.Collections.Generic;
using System.Text;

namespace BePopJwt.Business.Dtos.UserSongHistoryDtos
{
    public class UserSongHistorySongSummaryDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string CoverUrl { get; set; }
        public int AlbumId { get; set; }
    }
}
