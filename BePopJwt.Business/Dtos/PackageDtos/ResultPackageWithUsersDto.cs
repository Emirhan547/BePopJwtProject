using BePopJwt.Business.Dtos.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace BePopJwt.Business.Dtos.PackageDtos
{
    public class ResultPackageWithUsersDto : BaseDto
    {
        public string Name { get; set; }
        public int Level { get; set; }
        public decimal Price { get; set; }
        public List<PackageUserSummaryDto> Users { get; set; }
    }
}
