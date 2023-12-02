using Microsoft.AspNetCore.Identity;

namespace WebApp.Data.Entities
{
    public class AppRole:IdentityRole
    {
        public string Description { get; set; } = default!;
    }
}
