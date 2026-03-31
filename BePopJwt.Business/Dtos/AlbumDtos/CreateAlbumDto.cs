using BePopJwt.Entity.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace BePopJwt.Business.Dtos.AlbumDtos
{
    public class CreateAlbumDto
    {
        public string? Name { get; set; }

        public string? CoverUrl { get; set; }
        [JsonIgnore]
        public DateTime? ReleaseDate { get; set; } = DateTime.Now;

        public int? ArtistId { get; set; }

    }
}
