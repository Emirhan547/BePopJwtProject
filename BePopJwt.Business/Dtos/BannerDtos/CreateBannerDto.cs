using BePopJwt.Entity.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace BePopJwt.Business.Dtos.BannerDtos
{
    public class CreateBannerDto
    {
        public string Title { get; set; }
        public string Subtitle { get; set; }

        public string ImageUrl { get; set; }

        public int? SongId { get; set; }
        public Song Song { get; set; }
    }
}
