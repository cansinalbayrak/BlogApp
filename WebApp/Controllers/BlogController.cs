using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using WebApp.Data;
using WebApp.Data.Entities;
using WebApp.Models;

namespace WebApp.Controllers
{
    public class BlogController : Controller
    {

        private readonly ApplicationDbContext _context;
        private readonly UserManager<AppUser> _userManager;

        public BlogController(ApplicationDbContext context, UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Add()
        {
            ViewBag.Tags = _context.Tags.ToList();

            return View();
        }

        [HttpPost]
        public IActionResult Add(BlogViewModel blogViewModel)
        {
            var blog = new Blog();
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            blog.BlogContent = blogViewModel.Content;
            blog.BlogHeader = blogViewModel.Header;
            blog.TagId = blogViewModel.TagId;
            blog.AppUserId = userId;
            blog.Date = DateTime.Now;
            blog.ImgPath = "https://picsum.photos/200/300/?blur";
            _context.Add(blog);
            _context.SaveChanges();

            return RedirectToAction("Index", "Home");
        }
        public IActionResult ListBlogs(int tagId)
        {
            var list = _context.Blogs.Where(x => x.Tag.Id == tagId).ToList();
            return View(list);
        }
        public IActionResult ListAllBlogs()
        {
            var list = _context.Blogs.ToList();
            return View(list);
        }
        public IActionResult Edit(int id)
        {
            var blog = _context.Blogs.SingleOrDefault(b => b.Id == id);
            ViewBag.Tags = _context.Tags.ToList();
            return View(blog);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(Blog blog)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            blog.AppUserId = userId;
            blog.ImgPath = "https://picsum.photos/200/300/?blur";
            blog.Date = DateTime.Now;
            _context.Blogs.Update(blog);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index", "Home");
        }
        [HttpGet]
        public IActionResult Delete(int id)
        {
            var blog = _context.Blogs.SingleOrDefault(b => b.Id == id);
            return View(blog);
        }
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var blog = _context.Blogs.SingleOrDefault(b => b.Id == id);
            _context.Blogs.Remove(blog);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index", "Home");
        }
    }
}
