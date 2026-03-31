using System;
using System.Collections.Generic;
using System.Text;

namespace BePopJwt.Business.Dtos.AuthDtos
{
    public class RegisterDto
    {
        public string UserName { get; set; } 
        public string Email { get; set; } 

        public string DisplayName { get; set; }
        public string? ImageUrl { get; set; }
        public string Password { get; set; } 
        public int PackageId { get; set; }
    }
}
