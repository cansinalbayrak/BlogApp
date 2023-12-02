using Microsoft.AspNetCore.Identity;

namespace WebApp.Data.Entities
{
    public class AppUser : IdentityUser
    {
        public string Name { get; set; }
        public string Surname { get; set; }

        public List<UserFavoriteBlog> UserFavoriteBlogs { get; set; }

        public AppUser()
        {
            UserFavoriteBlogs = new List<UserFavoriteBlog>();
        }
    }
}
