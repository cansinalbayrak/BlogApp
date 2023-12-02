using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;
using WebApp.Data.Entities;

namespace WebApp.Data
{
    public class ApplicationDbContext : IdentityDbContext<AppUser>
    {

        public DbSet<Blog> Blogs { get; set; }
        public DbSet<AppUser> AppUsers { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {

            builder.Entity<UserFavoriteBlog>()
                .HasKey(ufb => new { ufb.AppUserId, ufb.BlogId });

            builder.Entity<AppUser>().HasMany(x => x.UserFavoriteBlogs)
                .WithOne(x => x.AppUser)
                .HasForeignKey(x => x.AppUserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Blog>()
                .HasMany(x => x.UserFavoriteBlogs)
                .WithOne(x => x.Blog)
                .HasForeignKey(x => x.BlogId)
                .OnDelete(DeleteBehavior.Restrict);

            base.OnModelCreating(builder);
        }
    }
}
