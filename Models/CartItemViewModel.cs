﻿namespace TL13Shop.Models
{
	public class CartItemViewModel
	{
		public int ProductId { get; set; }
		public string ProductName { get; set; }
		public string ProductImg {  get; set; }
		public int Quantity { get; set; }
		public decimal Price {  get; set; }
		public decimal Total => Quantity * Price;
	}
}
