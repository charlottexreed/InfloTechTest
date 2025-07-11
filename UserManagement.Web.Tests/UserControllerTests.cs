using System;
using UserManagement.Models;
using UserManagement.Services.Domain.Interfaces;
using UserManagement.Web.Models.Users;
using UserManagement.WebMS.Controllers;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Linq;

namespace UserManagement.Data.Tests;

public class UserControllerTests
{
    [Fact]
    public async Task List_WhenServiceReturnsUsers_ModelMustContainUsers()
    {
        // Arrange: Initializes objects and sets the value of the data that is passed to the method under test.
        var controller = CreateController();
        var users = SetupUsers();

        // Act: Invokes the method under test with the arranged parameters.
        var result = await controller.List(null);

        // Assert: Verifies that the action of the method under test behaves as expected.
        result.Model
            .Should().BeOfType<UserListViewModel>()
            .Which.Items.Should().BeEquivalentTo(users, options => options.Excluding(u => u.Password));
    }
    [Fact]
    public async Task View_WhenAskViewOfUser_ModelShouldContainSameUser()
    {
        var controller = CreateController();
        var users = SetupUsers();

        _userService.Setup(s => s.GetById(2)).ReturnsAsync(users[1]);

        var result = await controller.View(2);

        result
            .Should().BeOfType<ViewResult>()
            .Which.Model.Should().Be(users[1]);
    }
    [Fact]
    public void Add_Get_ReturnsView()
    {
        var controller = CreateController();

        var result = controller.Add();

        result.Should().BeOfType<ViewResult>();
    }
    [Fact]
    public async Task Add_Post_WhenValidRedirects()
    {
        var controller = CreateController();
        var users = SetupUsers();

        var result = await controller.Add(users[0]);

        result.Should().BeOfType<RedirectToActionResult>()
            .Which.ActionName.Should().Be("List");
    }
    [Fact]
    public async Task Edit_Get_ReturnsView()
    {
        var controller = CreateController();
        var users = SetupUsers();

        var result = await controller.Edit(users[0].Id);

        result.Should().BeOfType<ViewResult>();
    }
    [Fact]
    public async Task Edit_Post_WhenValidRedirects()
    {
        var controller = CreateController();
        var users = SetupUsers();

        var model = new EditUserViewModel
        {
            Id = users[0].Id,
            Forename = users[0].Forename,
            Surname = users[0].Surname,
            Email = users[0].Email,
            DateOfBirth = users[0].DateOfBirth,
            IsActive = users[0].IsActive
        };


        var result = await controller.Edit(users[0].Id, model);

        result.Should().BeOfType<RedirectToActionResult>()
            .Which.ActionName.Should().Be("List");
    }
    [Fact]
    public async Task Delete_Get_ReturnsView()
    {
        var controller = CreateController();
        var users = SetupUsers();

        var result = await controller.Delete(users[0].Id);

        result.Should().BeOfType<ViewResult>();
    }
    [Fact]
    public async Task Delete_Post_WhenValidRedirects()
    {
        var controller = CreateController();
        var users = SetupUsers();

        var result = await controller.ConfirmDelete(users[0].Id);

        result.Should().BeOfType<RedirectToActionResult>()
            .Which.ActionName.Should().Be("List");
    }

    private User[] SetupUsers()
    {
        var users = new[]
        {
            new User
            {

                Id = 1,
                Forename = "Johnny",
                Surname = "User",
                Email = "juser@example.com",
                DateOfBirth = new DateTime(2001, 2, 5),
                IsActive = true
            },
            new User
            {
                Id = 2,
                Forename = "Paul",
                Surname = "Carney",
                Email = "pcarney@example.com",
                DateOfBirth = new DateTime(1992, 3, 1),
                IsActive = false
            }
        };

        _userService
            .Setup(s => s.GetAll())
            .ReturnsAsync(users);

        foreach (var user in users)
        {
        _userService
            .Setup(s => s.GetById(user.Id))
            .ReturnsAsync(user);

        _userService
            .Setup(s => s.Update(It.Is<User>(u => u.Id == user.Id)))
            .Returns(Task.CompletedTask);

        _userService
            .Setup(s => s.Delete(It.Is<User>(u => u.Id == user.Id)))
            .Returns(Task.CompletedTask);
        }

        return users;
    }

    private readonly Mock<IUserService> _userService = new();
    private UsersController CreateController() => new(_userService.Object);
}
