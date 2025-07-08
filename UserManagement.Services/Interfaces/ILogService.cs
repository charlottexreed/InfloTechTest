using System.Collections.Generic;
using System.Threading.Tasks;
using UserManagement.Models;

namespace UserManagement.Services.Domain.Interfaces;

public interface ILogService
{
    Task<IEnumerable<Log>> GetAll();
    Task Create(Log entry);
}