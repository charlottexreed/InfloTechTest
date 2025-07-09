using System.Collections.Generic;
using System.Threading.Tasks;
using UserManagement.Models;

namespace UserManagement.Services.Domain.Interfaces;

public interface ILogService
{
    Task<IEnumerable<Log>> GetAll();
    Task<IEnumerable<Log>> FilterByUser(long id);
    Task Create(Log entry);
}