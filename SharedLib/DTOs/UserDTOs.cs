using System.ComponentModel.DataAnnotations;

namespace SharedLib.DTOs;

public record class UserDTO
{
    [Required(ErrorMessage = "Se requiere un nombre de usuario")]
    public string? UserName { get; set; }

    [Required(ErrorMessage = "Se requiere un email")]
    public string? Email { get; set; }

    [Required(AllowEmptyStrings = false, ErrorMessage = "Se requiere una contraseña")]
    [MinLength(5, ErrorMessage = "Debe ser mayor a 5 cáracteres")]
    public string? Password { get; set; }

    [Required(ErrorMessage = "Debe tener un rol")]
    public string? Role { get; set; }
}

public record class UserDetailDto
{
    public string? UserName { get; set; }
    public string? Email { get; set; }
    public string? Role { get; set; }
    public string? Token { get; set; }

}


