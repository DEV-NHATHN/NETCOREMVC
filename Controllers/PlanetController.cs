using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using App.Services;

namespace App.Controllers
{
   [Route("he-mat-troi/[action]")]
   public class PlanetController : Controller
   {
      private readonly PlanetService _planetService;
      private readonly ILogger<PlanetController> _logger;
      public PlanetController(PlanetService planetService, ILogger<PlanetController> logger)
      {
         _planetService = planetService;
         _logger = logger;
      }

      [Route("planets")]
      // nếu là /planets thì sẽ k kết hợp với he-mat-troi
      // khi đã thiết lập route ở controller thì phải thiết lập route ở action
      public IActionResult Index()
      {
         return View();
      }

      [BindProperty(SupportsGet = true, Name = "action")]
      public string Name { get; set; }

      [Route("[controller]/[action].html", Order = 1, Name = "mercury1")] // planet/mercury.html
      [Route("planet/[action]", Order = 2, Name = "mercury2")] // planet/mercury
      public IActionResult Mercury()
      {
         var planet = _planetService.GetPlanets().FirstOrDefault(p => p.Name == "Mercury");
         return View("Detail", planet);
      }

      [Route("planets/{id}")]
      // [HttpGet("")] 
      public IActionResult PlanetInfo(string id)
      {
         var planet = _planetService.GetPlanets().FirstOrDefault(p => p.Id == id);
         return View("Detail", planet);
      }
   }
}