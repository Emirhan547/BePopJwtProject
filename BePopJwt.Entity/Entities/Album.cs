using BePopJwt.Entity.Entities.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace BePopJwt.Entity.Entities
{
    public class Album:BaseEntity
    {

        public string Name { get; set; }

        public string CoverUrl { get; set; }
        public DateTime ReleaseDate { get; set; }

        public int ArtistId { get; set; }
        public Artist Artist { get; set; }

        public List<Song> Songs { get; set; }
    }
}
