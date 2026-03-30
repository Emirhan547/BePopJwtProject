using BePopJwt.Entity.Entities.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace BePopJwt.Entity.Entities
{
    public class Package:BaseEntity
    {
        public string Name { get; set; }
        public int Level { get; set; }
        public decimal Price { get; set; }

        public List<AppUser> Users { get; set; }
    }
}
