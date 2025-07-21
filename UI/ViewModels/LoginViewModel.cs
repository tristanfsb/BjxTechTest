using System.ComponentModel.DataAnnotations;

namespace UI.ViewModels;

public class LoginViewModel
{
    [Required(AllowEmptyStrings = false, ErrorMessage = "Se requiere un correo")]
    [EmailAddressAttribute(ErrorMessage = "Debe ser un correo electrónico válido")]
    public string? Email { get; set; }

    [Required(AllowEmptyStrings = false, ErrorMessage = "Se requiere una contraseña")]
    [MinLength(5, ErrorMessage = "La contraseña debe contener más 5 cáracteres")]
    public string? Password { get; set; }
}
