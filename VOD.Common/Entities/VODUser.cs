using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;

namespace VOD.Common.Entities
{
    public class VodUser : IdentityUser
    {
        public string Token { get; set; }
        public DateTime TokenExpires { get; set; }

        [NotMapped] public IList<Claim> Cliams { get; set; } = new List<Claim>();
    }
}