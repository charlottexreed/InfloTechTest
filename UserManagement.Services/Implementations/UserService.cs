using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using UserManagement.Data;
using UserManagement.Models;
using UserManagement.Services.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace UserManagement.Services.Domain.Implementations;

public class UserService : IUserService
{
    private readonly IDataContext _dataAccess;
    private readonly ILogService _logs;
    public UserService(IDataContext dataAccess, ILogService logs)
    {
        _dataAccess = dataAccess;
        _logs = logs;
    }

    /// <summary>
    /// Return users by active state
    /// </summary>
    /// <param name="isActive"></param>
    /// <returns></returns>
    public async Task<IEnumerable<User>> FilterByActive(bool isActive)
    {
        var users = _dataAccess.GetAll<User>();
        return await users.Where(u => u.IsActive == isActive).ToListAsync();
    }

    public async Task<IEnumerable<User>> GetAll() => await _dataAccess.GetAll<User>().ToListAsync();

    public async Task Create(User user)
    {
        await _dataAccess.Create(user);
        await _logs.Create(new Log
        {
            Action = "Create",
            TargetUserId = user.Id,
            Timestamp = DateTime.UtcNow,
            Details = $"Created {user.Email}"
        });
    }
    public async Task Delete(User user)
    {
        await _dataAccess.Delete(user);
        await _logs.Create(new Log
        {
            Action = "Delete",
            TargetUserId = user.Id,
            Timestamp = DateTime.UtcNow,
            Details = $"Deleted {user.Email}"
        });
    }
    public async Task Update(User user)
    {
        await _dataAccess.Update(user);
        await _logs.Create(new Log
        {
            Action = "Edit",
            TargetUserId = user.Id,
            Timestamp = DateTime.UtcNow,
            Details = $"Edited {user.Email}"
        });
    }
}
