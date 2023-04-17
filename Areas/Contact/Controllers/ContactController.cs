using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using App.Models;
using ContactModel = App.Models.Contacts.Contact;
using Microsoft.AspNetCore.Authorization;
using App.Data;

namespace App.Areas.Contact.Controllers
{
   [Area("Contact")]
   [Authorize(Roles = RoleName.Administrator + "," + RoleName.Editor)]
   public class ContactController : Controller
   {
      private readonly AppDbContext _context;

      public ContactController(AppDbContext context)
      {
         _context = context;
      }

      [HttpGet("/admin/contact")]
      public async Task<IActionResult> Index()
      {
         return _context.Contacts != null ?
                     View(await _context.Contacts.ToListAsync()) :
                     Problem("Entity set 'AppDbContext.Contacts'  is null.");
      }

      [HttpGet("/admin/contact/detail/{id}")]
      public async Task<IActionResult> Details(int? id)
      {
         if (id == null || _context.Contacts == null)
         {
            return NotFound();
         }

         var contact = await _context.Contacts
             .FirstOrDefaultAsync(m => m.Id == id);
         if (contact == null)
         {
            return NotFound();
         }

         return View(contact);
      }

      [TempData]
      public string StatusMessage { get; set; }

      [HttpGet("/contact/")]
      [AllowAnonymous]
      public IActionResult SendContact()
      {
         return View();
      }

      [HttpPost("/contact/")]
      [AllowAnonymous]
      [ValidateAntiForgeryToken]
      public async Task<IActionResult> SendContact([Bind("FullName,Email,Phone")] ContactModel contact)
      {
         if (ModelState.IsValid)
         {
            contact.DateSent = DateTime.Now;
            _context.Add(contact);
            await _context.SaveChangesAsync();
            StatusMessage = "Cảm ơn bạn đã gửi liên hệ cho chúng tôi.";
            return RedirectToAction("Index", "Home");
         }
         return View(contact);
      }

      [HttpGet("/admin/contact/delete/{id}")]
      public async Task<IActionResult> Delete(int? id)
      {
         if (id == null || _context.Contacts == null)
         {
            return NotFound();
         }

         var contact = await _context.Contacts
             .FirstOrDefaultAsync(m => m.Id == id);
         if (contact == null)
         {
            return NotFound();
         }

         return View(contact);
      }

      [HttpPost("/admin/contact/delete/{id}"), ActionName("Delete")]
      [ValidateAntiForgeryToken]
      public async Task<IActionResult> DeleteConfirmed(int id)
      {
         if (_context.Contacts == null)
         {
            return Problem("Entity set 'AppDbContext.Contacts'  is null.");
         }
         var contact = await _context.Contacts.FindAsync(id);
         if (contact != null)
         {
            _context.Contacts.Remove(contact);
         }

         await _context.SaveChangesAsync();
         return RedirectToAction(nameof(Index));
      }
   }
}
