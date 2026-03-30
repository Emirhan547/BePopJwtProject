using BePopJwt.Entity.Entities.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace BePopJwt.Entity.Entities
{
    public class Banner:BaseEntity
    {
        public string Title { get; set; }
        public string Subtitle { get; set; }

        public string ImageUrl { get; set; }

        public int? SongId { get; set; }
        public Song Song { get; set; }
    }
}
