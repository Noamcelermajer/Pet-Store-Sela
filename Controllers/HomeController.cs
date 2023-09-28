using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using pet_store_noamcelermajer.Models;

namespace pet_store_noamcelermajer.Controllers;
using Microsoft.EntityFrameworkCore; 
using System.Linq;
public class HomeController : Controller
{
    private readonly PetShopContext _context;

    public HomeController(PetShopContext context)
    {
        _context = context;
    }

    public IActionResult Index()
    {
        var commentCounts = _context.Comments
                                    .GroupBy(c => c.AnimalId)
                                    .Select(g => new
                                    {
                                        AnimalId = g.Key,
                                        CommentCount = g.Count()
                                    })
                                    .ToList();

        var mostCommentedAnimals = _context.Animals
                                           .Include(a => a.Category)
                                           .Take(2)
                                           .ToList();

        foreach (var animal in mostCommentedAnimals)
        {
            var commentCount = commentCounts.FirstOrDefault(cc => cc.AnimalId == animal.AnimalId);
            if (commentCount != null)
            {
                animal.CommentCount = commentCount.CommentCount;
            }
            else
            {
                animal.CommentCount = 0; 
            }
        }

        return View(mostCommentedAnimals);
    }
}
