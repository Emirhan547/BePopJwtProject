using BePopJwt.Entity.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace BePopJwt.Business.Dtos.UserSongHistoryDtos
{
    public class UpdateUserSongHistoryDto
    {
        public int Id {  get; set; }
        public int UserId { get; set; }
        public AppUser User { get; set; }

        public int SongId { get; set; }
        public Song Song { get; set; }

        public DateTime PlayedAt { get; set; }

        public int PlayDuration { get; set; }
    }
}
