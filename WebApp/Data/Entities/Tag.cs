namespace WebApp.Data.Entities
{
    public class Tag
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public List<Blog> Blogs { get; set; }

        public Tag()
        {

            Blogs = new List<Blog>();
        }
    }
}
