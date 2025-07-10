using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using UserManagement.Services.Domain.Interfaces;
using UserManagement.Web.Models.Auth;

namespace UserManagement.WebMS.Controllers;

[Route("auth")]
public class AuthController : Controller
{
    private readonly IUserService _userService;
    public AuthController(IUserService userService) => _userService = userService;

    [HttpGet]
    public ViewResult Login() => View();

    [HttpPost]
    public async Task<IActionResult> Login(AuthViewModel model)
    {
        if (!ModelState.IsValid)
        {
            ModelState.AddModelError(string.Empty, "Invalid email or password");
            return View(model);
        }
        var user = await _userService.ValidateUser(model.Email, model.Password);
        if (user == null)
        {
            ModelState.AddModelError(string.Empty, "Invalid email or password");
            return View(model);
        }
        else
        {
            HttpContext.Session.SetString("UserEmail", user.Email);
            return RedirectToAction("List", "Users");
        }
    }
    [HttpGet("logout")]
    public IActionResult Logout()
    {
        HttpContext.Session.Remove("UserEmail");
        return RedirectToAction("Login");
    }
}
