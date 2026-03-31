using System;
using System.Collections.Generic;
using System.Text;

namespace BePopJwt.Business.Dtos.SongDtos
{
    public class SongAlbumSummaryDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string CoverUrl { get; set; }
        public int ArtistId { get; set; }
    }
}
