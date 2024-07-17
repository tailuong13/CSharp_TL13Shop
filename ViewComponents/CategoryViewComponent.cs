using Microsoft.AspNetCore.Mvc;
using TL13Shop.Data;
using TL13Shop.Models;

namespace TL13Shop.ViewComponents
{
	public class CategoryViewComponent : ViewComponent
	{
		public readonly Tl13shopContext db;

		public CategoryViewComponent(Tl13shopContext context)
		{
			db = context;
		}

		public IViewComponentResult Invoke()
		{
			var category = db.Categories.Select(p => new CategoryViewModel
			{
				Id = p.CategoryId,
				Name = p.CategoryName
			});
			
			return View(category);
		}
	}
}
