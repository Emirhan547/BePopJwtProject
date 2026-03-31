using System;
using System.Collections.Generic;
using System.Text;

namespace BePopJwt.Business.Dtos.PlayerDtos
{
    public class PlaySongRequestDto
    {
        public int SongId { get; set; }
        public int PlayDuration { get; set; }
    }
}
