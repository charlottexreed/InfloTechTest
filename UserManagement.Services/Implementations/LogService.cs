using System.Threading.Tasks;
using UserManagement.Models;
using UserManagement.Services.Domain.Interfaces;
using UserManagement.Data;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

public class LogService : ILogService
{
    private readonly IDataContext _dataAccess;
    public LogService(IDataContext dataAccess) => _dataAccess = dataAccess;
    public async Task<IEnumerable<Log>> GetAll() => await _dataAccess.GetAll<Log>().ToListAsync();
    public async Task<IEnumerable<Log>> FilterByUser(long userId)
    {
        var logs = _dataAccess.GetAll<Log>();
        return await logs.Where(l => l.TargetUserId == userId).ToListAsync(); 
    }
    public async Task<Log?> FilterByLogId(long logId)
    {
        return await _dataAccess.GetAll<Log>().FirstOrDefaultAsync(l => l.Id == logId);
    }
    public async Task Create(Log entry)
    {
        await _dataAccess.Create(entry);
    }
}