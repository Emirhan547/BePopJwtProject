using System;
using System.Collections.Generic;
using System.Text;

namespace BePopJwt.Business.Dtos.AuthDtos
{
    public class AuthResponseDto
    {
        public int UserId { get; set; }
        public string UserName { get; set; } 
        public string Email { get; set; }
        public string DisplayName { get; set; } 
        public string PackageName { get; set; } 
        public int PackageLevel { get; set; }
        public string Token { get; set; }
        public DateTime ExpiresAtUtc { get; set; }
    }
}
