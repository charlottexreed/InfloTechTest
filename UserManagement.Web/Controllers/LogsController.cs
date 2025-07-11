using System.Threading.Tasks;
using UserManagement.Services.Domain.Interfaces;
using UserManagement.Web.Models.Logs;
using System.Linq;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;

[Route("logs")]
public class LogsController : Controller
{
    private readonly ILogService _logService;
    private readonly IUserService _userService;
    public LogsController(ILogService logService, IUserService userService)
    {
        _logService = logService;
        _userService = userService;
    }

    [HttpGet]
    public async Task<ViewResult> List(long? userId)
    {
        var logs = userId.HasValue ? await _logService.FilterByUser(userId.Value) : await _logService.GetAll();

        var items = logs.Select(p => new LogListItemViewModel
        {
            Id = p.Id,
            Action = p.Action,
            TargetUserId = p.TargetUserId,
            Details = p.Details,
            Timestamp = p.Timestamp
        });

        var model = new LogListViewModel
        {
            Items = items.ToList()
        };

        if (userId.HasValue)
        {
            var user = await _userService.GetById(userId.Value);
            if (user != null)
            {
                model.Forename = user.Forename;
                model.Surname = user.Surname;
            }
        }

        return View(model);
    }
    [HttpGet("log/{logId}")]
    [ActionName("GetLog")]
    public async Task<ViewResult> GetLog(long logId)
    {
        var log = await _logService.FilterByLogId(logId);
        if (log == null)
        {
            throw new InvalidOperationException($"Log with ID {logId} not found");
        }
        var model = new LogListItemViewModel
        {
            Id = log.Id,
            Action = log.Action,
            TargetUserId = log.TargetUserId,
            Details = log.Details,
            Timestamp = log.Timestamp
        };
        return View("LogDetails", model);
    }
}