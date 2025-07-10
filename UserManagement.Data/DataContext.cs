using System.Linq;
using System;
using Microsoft.EntityFrameworkCore;
using UserManagement.Models;
using System.Threading.Tasks;

namespace UserManagement.Data;

public class DataContext : DbContext, IDataContext
{
    public DataContext() => Database.EnsureCreated();

    protected override void OnConfiguring(DbContextOptionsBuilder options)
        => options.UseInMemoryDatabase("UserManagement.Data.DataContext");


    protected override void OnModelCreating(ModelBuilder model) {
        // Hardcoded hash of the password '1234' used for all users for the purpose of the tech test
        var hashedPassword = "AQAAAAIAAYagAAAAEEeWva5sQyyGnt4AU+2O3MQ8krhEbo3uE9i5L7xO9+JJA2Jn4lQ7UxP/5h54fRh6nQ==";
        model.Entity<User>().HasData(new[]
        {
            new User { Id = 1, Forename = "Peter", Surname = "Loew", Email = "ploew@example.com", Password = hashedPassword, DateOfBirth = new DateTime(1980, 1, 12), IsActive = true },
            new User { Id = 2, Forename = "Benjamin Franklin", Surname = "Gates", Email = "bfgates@example.com", Password = hashedPassword, DateOfBirth = new DateTime(1964, 4, 13), IsActive = true },
            new User { Id = 3, Forename = "Castor", Surname = "Troy", Email = "ctroy@example.com", Password = hashedPassword, DateOfBirth = new DateTime(1986, 6, 26), IsActive = false },
            new User { Id = 4, Forename = "Memphis", Surname = "Raines", Email = "mraines@example.com", Password = hashedPassword, DateOfBirth = new DateTime(2001, 12, 12), IsActive = true },
            new User { Id = 5, Forename = "Stanley", Surname = "Goodspeed", Email = "sgodspeed@example.com", Password = hashedPassword, DateOfBirth = new DateTime(1997, 6, 1), IsActive = true },
            new User { Id = 6, Forename = "H.I.", Surname = "McDunnough", Email = "himcdunnough@example.com", Password = hashedPassword, DateOfBirth = new DateTime(1950, 10, 3), IsActive = true },
            new User { Id = 7, Forename = "Cameron", Surname = "Poe", Email = "cpoe@example.com", Password = hashedPassword, DateOfBirth = new DateTime(1967, 3, 24), IsActive = false },
            new User { Id = 8, Forename = "Edward", Surname = "Malus", Email = "emalus@example.com", Password = hashedPassword, DateOfBirth = new DateTime(1991, 4, 26), IsActive = false },
            new User { Id = 9, Forename = "Damon", Surname = "Macready", Email = "dmacready@example.com", Password = hashedPassword, DateOfBirth = new DateTime(1978, 11, 8), IsActive = false },
            new User { Id = 10, Forename = "Johnny", Surname = "Blaze", Email = "jblaze@example.com", Password = hashedPassword, DateOfBirth = new DateTime(1996, 8, 4), IsActive = true },
            new User { Id = 11, Forename = "Robin", Surname = "Feld", Email = "rfeld@example.com", Password = hashedPassword, DateOfBirth = new DateTime(1955, 2, 28), IsActive = true },
        });
    }

    public DbSet<User>? Users { get; set; }
    public DbSet<Log>? Logs { get; set; }

    public IQueryable<TEntity> GetAll<TEntity>() where TEntity : class
        => base.Set<TEntity>();

    public async Task Create<TEntity>(TEntity entity) where TEntity : class
    {
        await base.AddAsync(entity);
        await SaveChangesAsync();
    }

    public new async Task Update<TEntity>(TEntity entity) where TEntity : class
    {
        base.Update(entity);
        await SaveChangesAsync();
    }

    public async Task Delete<TEntity>(TEntity entity) where TEntity : class
    {
        base.Remove(entity);
        await SaveChangesAsync();
    }
}
