using Microsoft.AspNetCore.Identity;

namespace UserServer.Entities
{
    public class ApplicationUserRole
    {
        public long Id { get; set; }
        public long UserId { get; set; }
        public long RoleId { get; set; }
        public virtual ApplicationUser User { get; set; }
        public virtual ApplicationRole Role { get; set; }
    }
}
