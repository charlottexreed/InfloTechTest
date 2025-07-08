using System.Threading.Tasks;
using UserManagement.Models;
using UserManagement.Services.Domain.Interfaces;
using UserManagement.Data;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

public class LogService : ILogService
{
    private readonly IDataContext _dataAccess;
    public LogService(IDataContext dataAccess) => _dataAccess = dataAccess;
    public async Task<IEnumerable<Log>> GetAll() => await _dataAccess.GetAll<Log>().ToListAsync();
    public async Task Create(Log entry)
    {
        await _dataAccess.Create(entry);
    }
}