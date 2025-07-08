using System.Threading.Tasks;
using UserManagement.Services.Domain.Interfaces;
using UserManagement.Web.Models.Logs;
using System.Linq;

[Route("logs")]
public class LogsController : Controller
{
    private readonly ILogService _logService;
    public LogsController(ILogService logService) => _logService = logService;

    [HttpGet]
    public async Task<ViewResult> List()
    {
        var logs = await _logService.GetAll();

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

        return View(model);
    }
}