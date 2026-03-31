using BePopJwt.Entity.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace BePopJwt.Business.Dtos.PackageDtos
{
    public class UpdatePackageDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Level { get; set; }
        public decimal Price { get; set; }

        public List<AppUser> Users { get; set; }
    }
}
