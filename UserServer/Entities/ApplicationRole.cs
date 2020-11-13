using System.Collections.Generic;

namespace UserServer.Entities
{
    public class ApplicationRole
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string NormalizedName { get; set; }
        public virtual ICollection<ApplicationUserRole> UserRoles { get; set; }
    }
}
