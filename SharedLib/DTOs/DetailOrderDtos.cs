namespace SharedLib.DTOs;

public record class DetailOrderDto
{
    public int? OrderIdFk { get; set; }
    public int? ProductIdFk { get; set; }
    public int? Quantity { get; set; }
}

public record class DetailOrderDetailDto
{
    public int DetailId { get; set; }

    public string? Code { get; set; }

    public int OrderIdFk { get; set; }

    public int ProductIdFk { get; set; }

    public int Quantity { get; set; }
}
