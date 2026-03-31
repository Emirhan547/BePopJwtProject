using BePopJwt.Business.Dtos.AlbumDtos;
using BePopJwt.Business.Dtos.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace BePopJwt.Business.Dtos.ArtistDtos
{
    public class ResultArtistWithAlbumDto:BaseDto
    {
        public string Name { get; set; }

        public string ImageUrl { get; set; }
        public string CoverUrl { get; set; }
        public string Bio { get; set; }
        public List<AlbumDto>Albums { get; set; }
    }
}
