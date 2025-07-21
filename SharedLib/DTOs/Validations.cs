using System.ComponentModel.DataAnnotations;

namespace SharedLib.DTOs;

using ErrorDictionary = System.Collections.Generic.Dictionary<string, string[]>;

public static class ValidateDtos
{
    public static ErrorDictionary ValidateDto<T>(this T userDto) where T : class
    {
        var context = new ValidationContext(userDto);
        var results = new List<ValidationResult>();

        if (!Validator.TryValidateObject(userDto, context, results, true))
        {
            return (ErrorDictionary)results
                    .GroupBy(e => e.MemberNames.FirstOrDefault() ?? "")
                    .ToDictionary(g => g.Key, g => g.Select(e => e.ErrorMessage ?? "Error de validación").ToArray());
        }

        return new ErrorDictionary();
    }
}
