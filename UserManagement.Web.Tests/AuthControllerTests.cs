using System;
using Microsoft.AspNetCore.Mvc;
using UserManagement.Models;
using UserManagement.Services.Domain.Interfaces;
using UserManagement.WebMS.Controllers;

namespace UserManagement.Data.Tests;

public class AuthControllerTests
{
    [Fact]
    public void Login_ReturnsView()
    {
        var controller = CreateController();

        var result = controller.Login();

        result.Should().BeOfType<ViewResult>();
    }

    private readonly Mock<IUserService> _userService = new();
    private AuthController CreateController() => new(_userService.Object);
}