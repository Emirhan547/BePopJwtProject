using BePopJwt.Entity.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace BePopJwt.Business.Dtos.ArtistDtos
{
    public class UpdateArtistDto
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public string ImageUrl { get; set; }
        public string CoverUrl { get; set; }
        public string Bio { get; set; }
        public List<Album> Albums { get; set; }
    }
}
