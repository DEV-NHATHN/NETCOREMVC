using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using App.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace App.Areas.Database.Controllers
{
   public class DBManageController : Controller
   {
      public DBManageController(AppDbContext dbContext)
      {
         _dbContext = dbContext;
      }

      private readonly AppDbContext _dbContext;

      [Area("Database")]
      [Route("/database-manage/[action]")]
      public IActionResult Index()
      {
         return View();
      }

      [TempData]
      public string StatusMessage { get; set; }

      [HttpPost]
      public async Task<IActionResult> Migrate()
      {
         await _dbContext.Database.MigrateAsync();

         StatusMessage = "Cập nhật Database thành công";

         return RedirectToAction(nameof(Index));
      }
   }
}