﻿using System;
using System.Collections.Generic;

namespace TL13Shop.Data;

public partial class ProductImage
{
    public int ImageId { get; set; }

    public string? ImageUrl { get; set; }

    public int ProductId { get; set; }

    public bool? DefaultImage { get; set; }

    public virtual Product Product { get; set; } = null!;
}
