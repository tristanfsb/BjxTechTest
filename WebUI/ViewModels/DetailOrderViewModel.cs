using System;

namespace WebUI.ViewModels;

public class DetailOrderViewModel
{
    public int? OrderIdFk { get; set; }
    public int? ProductIdFk { get; set; }
    public int? Quantity { get; set; }
}
