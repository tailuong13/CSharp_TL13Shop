using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TL13Shop.Data;
using TL13Shop.Models;

namespace TL13Shop.ViewComponents
{
	public class OrderDetailViewComponent : ViewComponent
	{

		public readonly Tl13shopContext db;
		public OrderDetailViewComponent(Tl13shopContext context)
		{
			db = context;
		}

		public IViewComponentResult Invoke(int orderId, int productId)
		{
			var order = db.OrderDetails
				.Include(o => o.Product)
				.Include(o => o.Order)
				.FirstOrDefault(o => o.OrderId == orderId && o.ProductId == productId);
			var data = new OrderDetailViewModel
			{
				DetailId = order.DetailId,
				OrderId = order.OrderId,
				CustomerName = order.Order.CustomerName,
				CustomerAddress = order.Order.CustomerAddress,
				CustomerPhone = order.Order.CustomerPhone,
				Price = order.Product.Price,
				Style = order.Product.Color
			};

			return View(data);

		}
	}
}
