using BePopJwt.Entity.Entities.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace BePopJwt.Entity.Entities
{
    public class UserSongHistory:BaseEntity
    {
        public int UserId { get; set; }
        public AppUser User { get; set; }

        public int SongId { get; set; }
        public Song Song { get; set; }

        public DateTime PlayedAt { get; set; }

        public int PlayDuration { get; set; } 
    }
}
