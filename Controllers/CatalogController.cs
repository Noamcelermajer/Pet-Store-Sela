using Microsoft.AspNetCore.Mvc;
using pet_store_noamcelermajer.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace pet_store_noamcelermajer.Controllers
{
    public class CatalogController : Controller
    {

        private readonly PetShopContext _context;
        public IActionResult Index(int categoryId)
        {
            var animals = _context.Animals
                .Include(a => a.Category)
                .Where(a => categoryId == 0 || a.CategoryId == categoryId) // Filter by category
                .ToList();

            var categories = _context.Categories.ToList();

            ViewData["Categories"] = new SelectList(categories, "CategoryId", "Name", categoryId);

            return View(animals);
        }
        public CatalogController(PetShopContext context)
        {
            _context = context;
        }
        [HttpPost]
        public async Task<IActionResult> AddComment(int AnimalId, string CommentText)
        {
            var newComment = new Comment
            {
                CommentText = CommentText,
                AnimalId = AnimalId
            };

            _context.Comments.Add(newComment);
            await _context.SaveChangesAsync();

            return RedirectToAction("Details", new { id = AnimalId });
        }

    
    public async Task<IActionResult> Details(int id)
    {
        var animal = await _context.Animals
            .Include(a => a.Category)
            .FirstOrDefaultAsync(a => a.AnimalId == id);

        if (animal == null)
        {
            return NotFound();
        }

        var comments = await _context.Comments
            .Where(c => c.AnimalId == id)
            .ToListAsync();

        ViewData["Comments"] = comments;

        return View(animal);
    }
    }
}
