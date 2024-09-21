using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using TL13Shop.Data;
using TL13Shop.Models;

namespace TL13Shop.Controllers
{
    public class ShopController : Controller
    {
        private readonly Tl13shopContext db;

        public ShopController(Tl13shopContext context)
        {
            db = context;
        }

        public IActionResult Index(int? CategoryId)
        {
            var products = db.Products.AsQueryable();
            var productImages = db.ProductImages.AsQueryable();

            if (CategoryId.HasValue)
            {
                products = products.Where(p => p.CategoryId == CategoryId);
            }

            var data = products.Select(p => new ProductsViewModel
            {
                Id = p.ProductId,
                Name = p.ProductName,
                Price = p.Price,
                ShortDescription = p.ShortDescription ?? "",
                DefaultImgUrl = productImages.Where(i => i.ProductId == p.ProductId && i.DefaultImage == true).Select(i => i.ImageUrl).FirstOrDefault() ?? "",
                ProductImgs = productImages.Where(i => i.ProductId == p.ProductId).Select(i => i.ImageUrl).ToList()
            });

            return View(data);
        }

        public IActionResult Search(string? query)
        {
            var products = db.Products.AsQueryable();
            var productImages = db.ProductImages.AsQueryable();

            if (query != null)
            {
                products = products.Where(p => p.ProductName.Contains(query));
            }

            var data = products.Select(p => new ProductsViewModel
            {
                Id = p.ProductId,
                Name = p.ProductName,
                Price = p.Price,
                ShortDescription = p.ShortDescription ?? "",
                DefaultImgUrl = productImages.Where(i => i.ProductId == p.ProductId && i.DefaultImage == true).Select(i => i.ImageUrl).FirstOrDefault() ?? "",
                ProductImgs = productImages.Where(i => i.ProductId == p.ProductId).Select(i => i.ImageUrl).ToList()
            });

            return View(data);
        }

        public IActionResult Detail(int productId)
        {
            var product = db.Products.Include(p => p.Category).SingleOrDefault(p => p.ProductId == productId);

            if (product == null)
            {
                TempData["Message"] = "Can't found the product with " + productId.ToString();
                return Redirect("/404");
            }

            var data = new ProductDetailViewModel
            {
                Id = product.ProductId,
                Name = product.ProductName,
                Price = product.Price,
                Category = product.Category.CategoryName,
                ShortDescription = product.ShortDescription ?? "",
                LongDescription = product.LongDescription ?? "",
                ProductImgs = db.ProductImages.Where(i => i.ProductId == product.ProductId).Select(i => i.ImageUrl).ToList(),
                Quantity = product.Quantity,
            };


            return View(data);
        }
    }
}
