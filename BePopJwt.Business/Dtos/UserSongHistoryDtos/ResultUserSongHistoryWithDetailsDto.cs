using BePopJwt.Business.Dtos.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace BePopJwt.Business.Dtos.UserSongHistoryDtos
{
    public class ResultUserSongHistoryWithDetailsDto : BaseDto
    {
        public int UserId { get; set; }
        public UserSongHistoryUserSummaryDto User { get; set; }
        public int SongId { get; set; }
        public UserSongHistorySongSummaryDto Song { get; set; }
        public DateTime PlayedAt { get; set; }
        public int PlayDuration { get; set; }
    }
}
