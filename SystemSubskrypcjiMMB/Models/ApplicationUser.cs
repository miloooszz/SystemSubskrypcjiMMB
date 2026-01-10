using Microsoft.AspNetCore.Identity;

namespace SystemSubskrypcjiMMB.Models
{
    public class ApplicationUser : IdentityUser
    {
        public virtual ICollection<SubskrypcjaMMB> Subskrypcje { get; set; } = new List<SubskrypcjaMMB>();
    }
}
