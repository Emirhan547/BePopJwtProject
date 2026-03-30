using BePopJwt.Entity.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace BePopJwt.Business.Dtos.AlbumDtos
{
    public class UpdateAlbumDto
    {
        public int Id { get; set; } 
        public string? Name { get; set; }

        public string? CoverUrl { get; set; }
        public DateTime ReleaseDate { get; set; } = DateTime.Now;

        public int? ArtistId { get; set; }

    }
}
