namespace WebApp.Data.Entities
{
    public class UserFavoriteBlog
    {

        public Blog Blog { get; set; }
        public int BlogId { get; set; }
        public AppUser AppUser { get; set; }
        public string AppUserId { get; set; }
    }
}
