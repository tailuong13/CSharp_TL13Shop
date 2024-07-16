﻿using System;
using System.Collections.Generic;

namespace TL13Shop.Data;

public partial class Role
{
    public int RoleId { get; set; }

    public string? RoleName { get; set; }

    public bool IsActive { get; set; }

    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
