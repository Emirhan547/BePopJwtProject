using BePopJwt.Entity.Entities.Common;
using BePopJwt.Entity.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace BePopJwt.Entity.Entities
{
    public class Song:BaseEntity
    {
        public string Name { get; set; }

        public int Duration { get; set; } 

        public string FilePath { get; set; }

        public string CoverUrl { get; set; }

        public PackageLevel Level { get; set; } 

        public int AlbumId { get; set; }
        public Album Album { get; set; }

        public List<UserSongHistory> Histories { get; set; }
    }
}
