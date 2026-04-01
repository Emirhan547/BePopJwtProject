using System;
using System.Collections.Generic;
using System.Text;

namespace BePopJwt.Business.Dtos.UserDtos
{
    public class UpdateProfileDto
    {
        public string UserName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string DisplayName { get; set; } = string.Empty;
        public string? ImageUrl { get; set; }
    }
}
