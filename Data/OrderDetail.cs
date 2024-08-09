using System;
using System.Collections.Generic;

namespace TL13Shop.Data;

public partial class OrderDetail
{
    public int DetailId { get; set; }

    public int OrderId { get; set; }

    public int ProductId { get; set; }

    public double Total { get; set; }

    public int Amount { get; set; }

    public double? Discount { get; set; }

    public virtual Order Order { get; set; } = null!;

    public virtual Product Product { get; set; } = null!;
}
