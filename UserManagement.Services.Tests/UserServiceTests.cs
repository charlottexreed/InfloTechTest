using System.Linq;
using UserManagement.Models;
using UserManagement.Services.Domain.Implementations;

namespace UserManagement.Data.Tests;

public class UserServiceTests
{
    [Fact]
    public void GetAll_WhenContextReturnsEntities_MustReturnSameEntities()
    {
        // Arrange: Initializes objects and sets the value of the data that is passed to the method under test.
        var service = CreateService();
        var users = SetupUsers();

        // Act: Invokes the method under test with the arranged parameters.
        var result = service.GetAll();

        // Assert: Verifies that the action of the method under test behaves as expected.
        result.Should().BeSameAs(users);
    }

    [Fact]
    public void FilterByActive_ReturnsOnlyActiveUsers()
    {
        var service = CreateService();
        var users = SetupUsers();
        
        var result = service.FilterByActive(true);

        result.Should().HaveCount(2).And.OnlyContain(u => u.IsActive);
    }

    [Fact]
    public void FilterByActive_ReturnsOnlyInactiveUsers()
    {
        var service = CreateService();
        var users = SetupUsers();
        
        var result = service.FilterByActive(false);

        result.Should().HaveCount(1).And.OnlyContain(u => !u.IsActive);
    }

    private IQueryable<User> SetupUsers()
    {
        var users = new[]
        {
            new User { Forename = "Johnny", Surname = "User", Email = "juser@example.com", IsActive = true },
            new User { Forename = "Timmy", Surname = "Smith", Email = "tsmith@example.com", IsActive = true },
            new User { Forename = "Sarah", Surname = "Lopez", Email = "slopez@example.com", IsActive = false }
        }.AsQueryable();

        _dataContext
            .Setup(s => s.GetAll<User>())
            .Returns(users);

        return users;
    }

    private readonly Mock<IDataContext> _dataContext = new();
    private UserService CreateService() => new(_dataContext.Object);
}
