using System;
using System.Collections.Generic;

namespace DataAccess.Models;

public partial class Product
{
    public int ProductId { get; set; }

    public string ProductName { get; set; } = null!;

    public string? Description { get; set; }

    public int Stock { get; set; }

    public string? Code { get; set; }

    public virtual ICollection<DetailOrder> DetailOrders { get; set; } = new List<DetailOrder>();
}
