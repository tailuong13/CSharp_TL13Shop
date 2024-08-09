using System;
using System.Collections.Generic;

namespace TL13Shop.Data;

public partial class Order
{
    public int OrderId { get; set; }

    public int UserId { get; set; }

    public string CustomerName { get; set; } = null!;

    public string CustomerPhone { get; set; } = null!;

    public string CustomerAddress { get; set; } = null!;

    public string PaymentMethod { get; set; } = null!;

    public string? Note { get; set; }

    public int StatusId { get; set; }

    public DateTime OrderDate { get; set; }

    public bool IsCancel { get; set; }

    public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();

    public virtual OrderStatus Status { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
