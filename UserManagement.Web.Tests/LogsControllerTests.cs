using System;
using UserManagement.Services.Domain.Interfaces;
using UserManagement.Models;
using System.Threading.Tasks;
using UserManagement.Web.Models.Logs;
using Microsoft.AspNetCore.Mvc;

namespace UserManagement.Data.Tests;

public class LogsControllerTests
{
    [Fact]
    public async Task List_WhenServiceReturnsUsers_ModelMustContainUsers() {
        var controller = CreateController();
        var users = SetupLogs();

        var result = await controller.List(null);

        result.Model
            .Should().BeOfType<LogListViewModel>()
            .Which.Items.Should().BeEquivalentTo(users);
    }
    [Fact]
    public async Task List_ReturnsView()
    {
        var controller = CreateController();

        var result = await controller.List(null);

        result.Should().BeOfType<ViewResult>();
    }
    private Log[] SetupLogs()
    {
        var logs = new[]
        {
            new Log
            {

                Id = 1,
                Action = "Created",
                TargetUserId = 2,
                Details = "Created a@a.com",
                Timestamp = new DateTime(2025, 1, 1)
            },
            new Log
            {
                Id = 2,
                Action = "Edited",
                TargetUserId = 4,
                Details = "Edited c@c.com",
                Timestamp = new DateTime(2025, 3, 1)
            },
            new Log
            {
                Id = 2,
                Action = "Deleted",
                TargetUserId = 4,
                Details = "Deleted c@c.com",
                Timestamp = new DateTime(2025, 3, 11)
            }
        };

        _logsService
            .Setup(s => s.GetAll())
            .ReturnsAsync(logs);

        return logs;
    }

    private readonly Mock<ILogService> _logsService = new();
    private readonly Mock<IUserService> _userService = new();
    private LogsController CreateController() => new(_logsService.Object, _userService.Object);
}