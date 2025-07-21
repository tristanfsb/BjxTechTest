using System;
using System.Collections.Generic;

namespace DataAccess.Models;

public partial class DetailOrder
{
    public int DetailId { get; set; }

    public int OrderIdFk { get; set; }

    public int ProductIdFk { get; set; }

    public int Quantity { get; set; }

    public virtual Order OrderIdFkNavigation { get; set; } = null!;

    public virtual Product ProductIdFkNavigation { get; set; } = null!;
}
