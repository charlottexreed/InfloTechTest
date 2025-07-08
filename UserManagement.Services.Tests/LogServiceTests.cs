using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MockQueryable;
using UserManagement.Models;

namespace UserManagement.Data.Tests;

public class LogServiceTests
{
    [Fact]
    public async Task GetAll_WhenContextReturnsEntities_MustReturnSameEntities()
    {
        var service = CreateService();
        var logs = SetupLogs();

        var result = await service.GetAll();

        result.Should().BeEquivalentTo(logs);
    }

    private List<Log> SetupLogs()
    {
        var logs = new List<Log>
        {
            new Log { Id = 1, Action = "Delete", TargetUserId = 43, Details = "Deleted a@a.com", Timestamp = new DateTime(2025, 2, 1) },
            new Log { Id = 2, Action = "Create", TargetUserId = 43, Details = "Created a@a.com", Timestamp = new DateTime(2025, 3, 1) },
            new Log { Id = 3, Action = "Edit", TargetUserId = 43, Details = "Edited a@a.com", Timestamp = new DateTime(2025, 1, 4) }
        };
        // Uses mock queryable as the ListAsync means the queryable needs to be mocked
        var mockQueryable = logs.AsQueryable().BuildMock();

        _dataContext
            .Setup(s => s.GetAll<Log>())
            .Returns(mockQueryable);

        return logs;
    }

    private readonly Mock<IDataContext> _dataContext = new();
    private LogService CreateService() => new(_dataContext.Object);
}