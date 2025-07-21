using System;
using System.Collections.Generic;

namespace DataAccess.Models;

public partial class Order
{
    public int OrderId { get; set; }

    public string? ClientName { get; set; }

    public DateOnly? OrderDate { get; set; }

    public virtual ICollection<DetailOrder> DetailOrders { get; set; } = new List<DetailOrder>();
}
