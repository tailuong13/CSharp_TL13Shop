using System;
using System.Collections.Generic;

namespace TL13Shop.Data;

public partial class Contact
{
    public int ContactId { get; set; }

    public string? FullName { get; set; }

    public string? Email { get; set; }

    public string? Message { get; set; }

    public DateTime CreatedDate { get; set; }
}
