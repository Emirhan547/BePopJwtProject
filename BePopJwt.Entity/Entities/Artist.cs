using BePopJwt.Entity.Entities.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace BePopJwt.Entity.Entities
{
    public  class Artist:BaseEntity
    {

        public string Name { get; set; }

        public string ImageUrl { get; set; } 
        public string CoverUrl { get; set; }  
        public string Bio { get; set; }
        public List<Album> Albums { get; set; }
    }
}
