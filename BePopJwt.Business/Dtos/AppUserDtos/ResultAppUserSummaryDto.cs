using System;
using System.Collections.Generic;
using System.Text;

namespace BePopJwt.Business.Dtos.AppUserDtos
{
    public class ResultAppUserSummaryDto
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string DisplayName { get; set; }
        public string? ImageUrl { get; set; }
        public int PackageId { get; set; }
    }
}
