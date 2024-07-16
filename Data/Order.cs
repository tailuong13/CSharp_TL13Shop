using System;
using System.Collections.Generic;

namespace TL13Shop.Data;

public partial class Order
{
    public int OrderId { get; set; }

    public int UserId { get; set; }

    public int ProductId { get; set; }

    public int? Quantity { get; set; }

    public int? Total { get; set; }

    public string CustomerName { get; set; } = null!;

    public string CustomerPhone { get; set; } = null!;

    public string CustomerAddress { get; set; } = null!;

    public string PaymentMethod { get; set; } = null!;

    public int StatusId { get; set; }

    public DateTime OrderDate { get; set; }

    public bool IsCancle { get; set; }

    public virtual Product Product { get; set; } = null!;

    public virtual OrderStatus Status { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
