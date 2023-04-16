using App.Models;

namespace App.Services
{
   public class ProductService : List<ProductModel>
   {
      public ProductService()
      {
         Add(new ProductModel { Id = 1, Name = "iPhone", Price = 999.99m });
         Add(new ProductModel { Id = 2, Name = "iPad", Price = 799.99m });
         Add(new ProductModel { Id = 3, Name = "iPod", Price = 299.99m });
         Add(new ProductModel { Id = 4, Name = "iMac", Price = 1999.99m });
         Add(new ProductModel { Id = 5, Name = "MacBook", Price = 1499.99m });
      }

      public List<ProductModel> GetProducts()
      {
         return this;
      }
   }
}