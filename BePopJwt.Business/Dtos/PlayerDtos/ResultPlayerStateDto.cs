using System;
using System.Collections.Generic;
using System.Text;

namespace BePopJwt.Business.Dtos.PlayerDtos
{
    public class ResultPlayerStateDto
    {
        public int UserId { get; set; }
        public int? SongId { get; set; }
        public bool IsPlaying { get; set; }
        public int CurrentPositionSeconds { get; set; }
        public DateTime? LastPlayedAt { get; set; }
    }
}
