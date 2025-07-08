using System.Linq;
using System;
using UserManagement.Models;
using UserManagement.Services.Domain.Implementations;
using UserManagement.Services.Domain.Interfaces;
using System.Threading.Tasks;
using MockQueryable;
using System.Collections.Generic;

namespace UserManagement.Data.Tests;

public class UserServiceTests
{
    [Fact]
    public async Task GetAll_WhenContextReturnsEntities_MustReturnSameEntities()
    {
        // Arrange: Initializes objects and sets the value of the data that is passed to the method under test.
        var service = CreateService();
        var users = SetupUsers();

        // Act: Invokes the method under test with the arranged parameters.
        var result = await service.GetAll();

        // Assert: Verifies that the action of the method under test behaves as expected.
        result.Should().BeEquivalentTo(users);
    }

    [Fact]
    public async Task FilterByActive_ReturnsOnlyActiveUsers()
    {
        var service = CreateService();
        var users = SetupUsers();
        
        var result = await service.FilterByActive(true);

        result.Should().HaveCount(2).And.OnlyContain(u => u.IsActive);
    }

    [Fact]
    public async Task FilterByActive_ReturnsOnlyInactiveUsers()
    {
        var service = CreateService();
        var users = SetupUsers();
        
        var result = await service.FilterByActive(false);

        result.Should().HaveCount(1).And.OnlyContain(u => !u.IsActive);
    }

    private List<User> SetupUsers()
    {
        var users = new List<User>
        {
            new User { Forename = "Johnny", Surname = "User", Email = "juser@example.com", DateOfBirth = new DateTime(1930, 1, 1), IsActive = true },
            new User { Forename = "Timmy", Surname = "Smith", Email = "tsmith@example.com", DateOfBirth = new DateTime(1960, 3, 22), IsActive = true },
            new User { Forename = "Sarah", Surname = "Lopez", Email = "slopez@example.com", DateOfBirth = new DateTime(1979, 3, 12), IsActive = false }
        };
        // Uses mock queryable as the ListAsync means the queryable needs to be mocked
        var mockQueryable = users.AsQueryable().BuildMock();

        _dataContext
            .Setup(s => s.GetAll<User>())
            .Returns(mockQueryable);

        return users;
    }

    private readonly Mock<IDataContext> _dataContext = new();
    private readonly Mock<ILogService> _logService  = new();
    private UserService CreateService() => new(_dataContext.Object, _logService.Object);
}
