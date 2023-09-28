using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

using Microsoft.EntityFrameworkCore;
using pet_store_noamcelermajer.Models;

namespace pet_store_noamcelermajer.Controllers
{
    public class AdminController : Controller
    {
        private readonly PetShopContext _context;

        public AdminController(PetShopContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var animals = await _context.Animals.Include(a => a.Category).ToListAsync();
            return View(animals);
        }

        public Category GetCategoryById(int categoryId)
        {
            return _context.Categories.FirstOrDefault(c => c.CategoryId == categoryId);
        }


        public IActionResult Edit(int id)
        {
            var animal = _context.Animals.Find(id);
            if (animal == null)
            {
                return NotFound();
            }

            var categories = _context.Categories.ToList();

            ViewBag.Categories = new SelectList(categories, "CategoryId", "Name");

            return View(animal);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(int id, Animal updatedAnimal, IFormFile newImageFile)
        {
            var animal = await _context.Animals.FindAsync(id);
            if (animal == null)
            {
                return NotFound();
            }

            // Update fields
            animal.Name = updatedAnimal.Name;
            animal.Age = updatedAnimal.Age;
            animal.Description = updatedAnimal.Description;

            // Update CategoryId only if provided in the form
            if (updatedAnimal.CategoryId != null)
            {
                animal.CategoryId = updatedAnimal.CategoryId;
            }

            // Handle image if a new one is uploaded
            if (newImageFile != null && newImageFile.Length > 0)
            {
                var fileName = Path.GetFileName(newImageFile.FileName);
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", fileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await newImageFile.CopyToAsync(stream);
                }
                animal.PictureName = fileName;
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var animal = await _context.Animals.FindAsync(id);
            if (animal == null)
            {
                return NotFound();
            }
            _context.Animals.Remove(animal);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public IActionResult Create()
        {
            var categories = _context.Categories.ToList();
            ViewData["Categories"] = new SelectList(categories, "CategoryId", "Name");
            
            return View(new Animal());
        }

        public async Task<IActionResult> Create(Animal animal, IFormFile imageFile, int categoryId)
        {
            var categories = _context.Categories.ToList();
            ViewData["Categories"] = new SelectList(categories, "CategoryId", "Name");
            ModelState.Remove("Category");
            var category = await _context.Categories.FindAsync(categoryId);
            if (category == null)
            {
                ModelState.AddModelError("CategoryId", "Invalid Category ID");
                return View("Create", animal); 
            }

            if (ModelState.IsValid)
            {
            if (imageFile != null && imageFile.Length > 0)
                {
                    var fileName = Path.GetFileName(imageFile.FileName);
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", fileName);
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await imageFile.CopyToAsync(stream);
                    }
                    
                    animal.PictureName = fileName;
                }
                animal.CategoryId = categoryId;

                _context.Add(animal);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            return View("Create", animal); 
        }
       
       [HttpPost]
        public IActionResult GetCategoryIdByName(string categoryName)
        {
            var category = _context.Categories.FirstOrDefault(c => c.Name == categoryName);
            if (category != null)
            {
                return Json(category.CategoryId); 
            }
            return Json(null);
        }

        [HttpPost]
        public IActionResult AddCategory(string Name)
        {
            if (string.IsNullOrEmpty(Name))
            {
                return Json(new { success = false });
            }

            var newCategory = new Category
            {
                Name = Name
            };

            _context.Categories.Add(newCategory);
            _context.SaveChanges();

            return Json(new { success = true });
        }


    }
}
