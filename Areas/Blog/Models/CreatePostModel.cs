using System.ComponentModel.DataAnnotations;
using App.Models.Blogs;

namespace AppMvc.Areas.Blogs.Models
{
   public class CreatePostModel : Post
   {
      [Display(Name = "Chuyên mục")]
      public int[] CategoryIDs { get; set; }
   }
}