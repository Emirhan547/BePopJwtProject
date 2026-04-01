using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace BePopJwt.Entity.Entities
{
    public class AppUser : IdentityUser<int>
    {
        public string DisplayName { get; set; }
        public string? ImageUrl { get; set; }

        public int PackageId { get; set; }
        public Package Package { get; set; }
        public List<UserSongHistory> Histories { get; set; }
        public Player? Player { get; set; }
    }
}
