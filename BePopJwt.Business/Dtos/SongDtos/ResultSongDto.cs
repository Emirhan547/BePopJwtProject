using BePopJwt.Business.Dtos.Common;
using BePopJwt.Entity.Entities;
using BePopJwt.Entity.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace BePopJwt.Business.Dtos.SongDtos
{
    public class ResultSongDto:BaseDto
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
