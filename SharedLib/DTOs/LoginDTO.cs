using System.ComponentModel.DataAnnotations;

namespace SharedLib.DTOs;

public record class LoginDTO
{
    [Required(ErrorMessage = "Se requiere un correo")]
    [EmailAddress(ErrorMessage = "Debe ser un correo electrónico válido")]
    public string? Email { get; init; }

    [Required(ErrorMessage = "Se requiere una contraseña")]
    [MinLength(5, ErrorMessage = "Mínimo 5 carácteres")]
    public string? Password { get; init; }
}
