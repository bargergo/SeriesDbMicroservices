using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace UserServer.Entities
{
    public class ApplicationUser
    {
        public long Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public virtual ICollection<ApplicationUserRole> UserRoles { get; set; }
    }
}
