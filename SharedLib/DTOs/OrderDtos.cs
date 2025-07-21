using System.ComponentModel.DataAnnotations;
using DataAccess.Models;

namespace SharedLib.DTOs;

public record class OrderDto
{
    [Required(ErrorMessage = "Se requiere un cliente")]
    public string? ClientName { get; set; }
    [Required(ErrorMessage = "Se requiere una fecha")]
    public DateOnly? OrderDate { get; set; }
}

public record class OrderDetailDto
{
    public int? OrderId { get; set; }
    public string? ClientName { get; set; }
    public DateOnly? OrderDate { get; set; }
    public List<DetailOrderDetailDto>? DetailOrder { get; set; }
}
