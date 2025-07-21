using System;
using DataAccess.Models;

using static BCrypt.Net.BCrypt;

namespace SharedLib.DTOs;

public static class UserMapping
{
    public static User ToEntity(this UserDTO u) => new()
    {
        UserName = u.UserName!,
        Email = u.Email!,
        Password = HashPassword(u.Password!, 12),
        Role = u.Role!
    };

    public static UserDTO ToDto(this User u) => new()
    {
        UserName = u.UserName!,
        Email = u.Email!,
        Password = u.Password!,
        Role = u.Role!
    };

    public static UserDetailDto ToDetailDto(this User u, string? token = null) => new()
    {
        UserName = u.UserName,
        Email = u.Email,
        Role = u.Role,
        Token = token
    };
}
