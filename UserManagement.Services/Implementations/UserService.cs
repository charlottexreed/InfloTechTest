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
    public UserService(IDataContext dataAccess) => _dataAccess = dataAccess;

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

    public async Task Create(User user) => await _dataAccess.Create(user);
    public async Task Delete(User user) => await _dataAccess.Delete(user);
    public async Task Update(User user) => await _dataAccess.Update(user);
}
