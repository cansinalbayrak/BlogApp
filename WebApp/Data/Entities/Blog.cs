using System.Drawing;

namespace WebApp.Data.Entities
{
    public class Blog
    {
        public int Id { get; set; }
        public string BlogHeader { get; set; }
        public string BlogContent { get; set; }
        public DateTime Date { get; set; }
        public string ImgPath { get; set; }

        public string AppUserId { get; set; }
        public AppUser AppUser { get; set; }

        public Tag Tag { get; set; }
        public int TagId { get; set; }

        public List<UserFavoriteBlog> UserFavoriteBlogs { get; set; }



        public Blog()
        {
            UserFavoriteBlogs = new List<UserFavoriteBlog>();
        }


    }
}
