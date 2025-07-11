﻿using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
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
    public async Task<ViewResult> List(bool? isActive)
    {
        var users = isActive.HasValue ? await _userService.FilterByActive(isActive.Value) : await _userService.GetAll();

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
    public async Task<ViewResult> View(long id)
    {
        var user = await _userService.GetById(id);
        return View(user);
    }

    [HttpGet("add")]
    public ViewResult Add()
    {
        return View();
    }
    [HttpPost("add")]
    public async Task<IActionResult> Add(User user)
    {
        if (!ModelState.IsValid)
        {
            return View(user);
        }
        await _userService.Create(user);
        return RedirectToAction("List");
    }

    [HttpGet("delete/{id}")]
    public async Task<ViewResult> Delete(long id)
    {
        var user = await _userService.GetById(id);
        return View(user);
    }

    [HttpPost("delete/{id}")]
    public async Task<IActionResult> ConfirmDelete(long id)
    {
        var user = await _userService.GetById(id);
        if (user != null)
        {
            await _userService.Delete(user);
        }
        return RedirectToAction("List");
    }

    [HttpGet("edit/{id}")]
    public async Task<ViewResult> Edit(long id)
    {
        var user = await _userService.GetById(id);
        if (user == null)
        {
            throw new InvalidOperationException($"User with ID {id} not found");
        }
        var model = new EditUserViewModel
        {
            Id = user.Id,
            Forename = user.Forename,
            Surname = user.Surname,
            Email = user.Email,
            DateOfBirth = user.DateOfBirth,
            IsActive = user.IsActive
        };
        return View(model);
    }

    [HttpPost("edit/{id}")]
    public async Task<IActionResult> Edit(long id, EditUserViewModel model)
    {
        Console.WriteLine($"Edit POST called for ID: {id}");
        if (!ModelState.IsValid)
        {
            return View(model);
        }
        var user = await _userService.GetById(id);
        if (user == null)
        {
            throw new InvalidOperationException($"User with ID {id} not found");
        }

        user.Forename = model.Forename;
        user.Surname = model.Surname;
        user.Email = model.Email;
        user.DateOfBirth = model.DateOfBirth;
        user.IsActive = model.IsActive;

        await _userService.Update(user);

        return RedirectToAction("List");
    }
}
