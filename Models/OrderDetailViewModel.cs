namespace TL13Shop.Models
{
	public class OrderDetailViewModel
	{
		public int DetailId { get; set; }
		public int OrderId { get; set; }
		public string CustomerName { get; set; }
		public string CustomerAddress { get; set; }
		public string CustomerPhone { get; set; }
		public decimal Price { get; set; }
		public string Style { get; set; }
	}
}
