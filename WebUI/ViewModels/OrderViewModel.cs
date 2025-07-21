using System;
using System.ComponentModel.DataAnnotations;

namespace WebUI.ViewModels;

public class OrderViewModel
{
    [Required(AllowEmptyStrings = false, ErrorMessage = "Se requiere un cliente")]
    public string? ClientName { get; set; }
    [Required(ErrorMessage = "Se requiere una fecha")]
    public DateOnly? OrderDate { get; set; }
}
