using System.ComponentModel.DataAnnotations;

namespace SharedLib.DTOs;

public record class ProductDto
{   
    [Required(ErrorMessage = "Se requiere un nombre de producto")]
    public string? ProductName { get; set; }
    public string? Description { get; set; } = "";
    
    [Required(ErrorMessage = "Se requiere una cantidad de stock")]
    [Range(1, int.MaxValue, ErrorMessage = "La cantidad de stock debe ser mayor a 0")]
    public int? Stock { get; set; }

    [Required(ErrorMessage = "Se requiere un código de producto")]
    public string? Code { get; set; }
}

public record class ProductDetailDto
{
    public int ProductId { get; set; } = 0;
    public string ProductName { get; set; } = "";
    public string Description { get; set; } = "";
    public int Stock { get; set; } = 0;
    public string Code { get; set; } = "";

}
