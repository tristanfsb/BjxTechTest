using System;

using SharedLib.DTOs;
using DataAccess.Models;
using Microsoft.EntityFrameworkCore;

namespace BusinessLogic.Services;

public class UserService(BjxDbContext context, ILogger<UserService> logger)
{

    private readonly BjxDbContext _context = context;
    private readonly ILogger _logger = logger;

    public async Task<User?> SaveUser(UserDTO userDto)
    {
        try
        {
            var user = userDto.ToEntity();
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
            return user;
        }
        catch (Exception ex)
        {
            _logger.LogError($"[ERROR]: {ex.Message} - {ex.StackTrace}");
            return null;
        }
    }

    public async Task<IList<User>?> GetUsers()
    {
        try
        {
            return await _context.Users.ToListAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError($"[ERROR]: {ex.Message} - {ex.StackTrace}");
            return null;
        }
    }

}

