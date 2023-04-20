using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using App.Models;
using Microsoft.EntityFrameworkCore;

namespace App.Controllers;

public class HomeController : Controller
{
   private readonly AppDbContext _db;
   private readonly ILogger<HomeController> _logger;

   public HomeController(ILogger<HomeController> logger, AppDbContext db)
   {
      _logger = logger;
      _db = db;
   }

   public string Hi()
   {
      return "Hi from Home Controller";
   }
   public IActionResult Index()
   {
      return View();
   }

   public IActionResult Privacy()
   {
      return View();
   }

   [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
   public IActionResult Error()
   {
      return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
   }

   public IActionResult List()
   {
      // get list category from database with lenth < 20
      // IEnumerable<Category> categories = _db.Categories.Where(c => c.Description.Length < 20);
      // return View(categories);

      var categories = _db.Categories;
      return View(categories);
   }

   public async Task<IActionResult> ListInner()
   {
      var qr = (from c in _db.Categories select c)
               .OrderBy(c => c.ParentCategory)
               .Include(c => c.ParentCategory);

      var categories = (await qr.ToListAsync())
               .Where(c => c.ParentCategory == null)

               .ToList();

      return View(categories);
   }

   public async Task<IActionResult> Details(int? id)
   {

      if (id == null)
      {
         return NotFound();
      }

      var category = await _db.Categories.FirstOrDefaultAsync(m => m.Id == id);

      if (category == null)
      {
         return NotFound();
      }

      return View(category);
   }
}
