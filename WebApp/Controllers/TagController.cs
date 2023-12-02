using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WebApp.Data;
using WebApp.Data.Entities;
using WebApp.Models;

namespace WebApp.Controllers
{
    public class TagController : Controller
    {

        private readonly ApplicationDbContext _context;

        public TagController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Add(TagViewModel tagViewModel)
        {
            var tag = new Tag();
            tag.Id = tagViewModel.Id;
            tag.Name = tagViewModel.Name;
            _context.Tags.Add(tag);
            _context.SaveChanges();

            return RedirectToAction("TagList");
        }
        public IActionResult TagList()
        {
            var list = _context.Tags.ToList();
            return View(list);
        }
        public IActionResult Edit(int id)
        {
            var tag = _context.Tags.SingleOrDefault(b => b.Id == id);
            return View(tag);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Tag tag)
        {
            _context.Tags.Update(tag);
            await _context.SaveChangesAsync();

            return RedirectToAction("TagList");
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            var tag = _context.Tags.SingleOrDefault(b => b.Id == id);
            return View(tag);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var tag = _context.Tags.SingleOrDefault(b => b.Id == id);
            _context.Tags.Remove(tag);
            await _context.SaveChangesAsync();

            return RedirectToAction("TagList");
        }
    }
}
