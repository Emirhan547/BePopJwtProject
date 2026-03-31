using BePopJwt.Entity.Entities.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace BePopJwt.Entity.Entities
{
    public class Player : BaseEntity
    {
        public int UserId { get; set; }
        public AppUser User { get; set; }

        public int? SongId { get; set; }
        public Song? Song { get; set; }

        public bool IsPlaying { get; set; }
        public int CurrentPositionSeconds { get; set; }
        public DateTime? LastPlayedAt { get; set; }
    }
}
