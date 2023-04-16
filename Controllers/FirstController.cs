using App.Services;
using Microsoft.AspNetCore.Mvc;
namespace App.Controllers
{
   public class FirstController : Controller
   {
      private readonly ILogger<FirstController> _logger;

      private readonly ProductService _productService;
      public FirstController(ILogger<FirstController> logger, ProductService productService)
      {
         _logger = logger;
         _productService = productService;
      }

      public string Index()
      {
         _logger.LogInformation("Hello from FirstController");
         _logger.LogWarning("Hello from FirstController");
         _logger.LogError("Hello from FirstController");
         _logger.LogCritical("Hello from FirstController");
         _logger.LogDebug("Hello from FirstController");
         _logger.LogTrace("Hello from FirstController");

         return "Hello from FirstController";
      }

      public IActionResult Readme()
      {
         var content = @"# Hello from FirstController
This is a readme file for the FirstController.
It is a markdown file.
## This is a subheading
### This is a sub-subheading
This is a paragraph.
This is another paragraph.
";
         return Content(content, "text/markdown");
      }

      public IActionResult Bird()
      {
         string filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "bird.jfif");

         var bytes = System.IO.File.ReadAllBytes(filePath);

         return File(bytes, "image/jpeg");
      }

      public IActionResult IphonePrice()
      {
         return Json(new { price = 999.99 });
      }

      public IActionResult Privacy()
      {
         var url = Url.Action("Privacy", "Home");
         _logger.LogInformation($"Privacy URL: {url}");
         // return LocalRedirect(url);
         return Redirect("https://www.google.com");
      }

      public IActionResult HelloView(string username = "Guest")
      {
         // return View("/Custom/View/hello.cshtml", username);
         return View("hello", username);
      }

      [AcceptVerbs("POST", "GET")]
      public IActionResult ViewProduct(int? id)
      {
         if (id == null)
         {
            return NotFound();
         }

         var product = _productService.Find(p => p.Id == id);

         if (product == null)
         {
            return NotFound();
         }

         // this.ViewData["product"] = product;
         // return View("ViewProduct");

         /*
         1. Kiểu dữ liệu truyền tải: ViewData là một đối tượng Dictionary trong khi ViewBag là một đối tượng dynamic.
         2. Cách truy cập: Để truy cập ViewData, chúng ta phải ép kiểu trước khi sử dụng nó trong View. Trong khi đó, ViewBag không yêu cầu ép kiểu và cho phép truy cập dữ liệu theo cú pháp thuộc tính.
         3. Sử dụng trong Lambda Expression: ViewBag có thể được sử dụng trong biểu thức Lambda, trong khi ViewData thì không được hỗ trợ.
         */

         ViewBag.product = product;
         return View("ViewProduct");
      }
   }
}