using System.Linq;
using UserManagement.Models;
using UserManagement.Services.Domain.Interfaces;
using UserManagement.Web.Models.Users;

namespace UserManagement.WebMS.Controllers;

[Route("users")]
public class UsersController : Controller
{
    private readonly IUserService _userService;
    public UsersController(IUserService userService) => _userService = userService;

    [HttpGet]
    public ViewResult List(bool? isActive)
    {
        var users = isActive.HasValue ? _userService.FilterByActive(isActive.Value) : _userService.GetAll();

        var items = users.Select(p => new UserListItemViewModel
        {
            Id = p.Id,
            Forename = p.Forename,
            Surname = p.Surname,
            Email = p.Email,
            DateOfBirth = p.DateOfBirth,
            IsActive = p.IsActive
        });

        var model = new UserListViewModel
        {
            Items = items.ToList()
        };

        return View(model);
    }

    [HttpGet("view/{id}")]
    public ViewResult View(long id)
    {
        var user = _userService.GetAll().FirstOrDefault(u => u.Id == id);
        return View(user);
    }

    [HttpGet("add")]
    public ViewResult Add()
    {
        return View();
    }
    [HttpPost("add")]
    public IActionResult Add(User user)
    {
        if (!ModelState.IsValid)
        {
            return View(user);
        }
        _userService.Create(user);
        return RedirectToAction("List");
    }

    [HttpGet("delete/{id}")]
    public ViewResult Delete(long id)
    {
        var user = _userService.GetAll().FirstOrDefault(u => u.Id == id);
        return View(user);
    }

    [HttpPost("delete/{id}")]
    public IActionResult ConfirmDelete(long id)
    {
        var user = _userService.GetAll().FirstOrDefault(u => u.Id == id);
        if (user != null)
        {
            _userService.Delete(user);
        }
        return RedirectToAction("List");
    }

    [HttpGet("edit/{id}")]
    public ViewResult Edit(long id)
    {
        var user = _userService.GetAll().FirstOrDefault(u => u.Id == id);
        return View(user);
    }

    [HttpPost("edit/{id}")]
    public IActionResult Edit(long id, User user)
    {
        if (!ModelState.IsValid)
        {
            return View(user);
        }
        _userService.Update(user);
        return RedirectToAction("List");
    }
}
