using GymWebApi.Data;
using GymWebApi.Entities;
using GymWebApi.Services;
using Microsoft.EntityFrameworkCore;
using Moq;

public class UserServiceTests
{
    private readonly Mock<UserDbContext> _contextMock;
    private readonly UserService _userService;

    public UserServiceTests()
    {
        _contextMock = new Mock<UserDbContext>();
        _userService = new UserService(_contextMock.Object);
    }

    [Fact]
    public async Task GetUserByIdAsync_ReturnsUser_WhenUserExists()
    {
        var userId = 1;
        var user = new User { ID = userId };
        var usersDbSetMock = new Mock<DbSet<User>>();
        usersDbSetMock.Setup(m => m.FindAsync(userId)).ReturnsAsync(user);

        _contextMock.Setup(c => c.Users).Returns(usersDbSetMock.Object);

        var result = await _userService.GetUserByIdAsync(userId);

        Assert.NotNull(result);
        Assert.Equal(userId, result.ID);
    }

    [Fact]
    public async Task GetUserByIdAsync_ReturnsNull_WhenUserDoesNotExist()
    {
        var userId = 1;
        var usersDbSetMock = new Mock<DbSet<User>>();
        usersDbSetMock.Setup(m => m.FindAsync(userId)).ReturnsAsync((User)null);

        _contextMock.Setup(c => c.Users).Returns(usersDbSetMock.Object);

        var result = await _userService.GetUserByIdAsync(userId);

        Assert.Null(result);
    }

    [Fact]
    public async Task GetUsersAsync_ReturnsAllUsers()
    {
        var users = new List<User>
        {
            new User { ID = 1 },
            new User { ID = 2 }
        }.AsQueryable();

        var usersDbSetMock = new Mock<DbSet<User>>();
        usersDbSetMock.As<IQueryable<User>>().Setup(m => m.Provider).Returns(users.Provider);
        usersDbSetMock.As<IQueryable<User>>().Setup(m => m.Expression).Returns(users.Expression);
        usersDbSetMock.As<IQueryable<User>>().Setup(m => m.ElementType).Returns(users.ElementType);
        usersDbSetMock.As<IQueryable<User>>().Setup(m => m.GetEnumerator()).Returns(users.GetEnumerator());

        _contextMock.Setup(c => c.Users).Returns(usersDbSetMock.Object);

        var result = await _userService.GetUsersAsync();

        Assert.Equal(2, result.Count());
    }

    [Fact]
    public async Task CreateUserAsync_AddsUserAndSavesChanges()
    {
        var user = new User { ID = 1 };
        var usersDbSetMock = new Mock<DbSet<User>>();

        _contextMock.Setup(c => c.Users).Returns(usersDbSetMock.Object);
        _contextMock.Setup(c => c.SaveChangesAsync(default)).ReturnsAsync(1);

        var result = await _userService.CreateUserAsync(user);

        usersDbSetMock.Verify(m => m.Add(user), Times.Once);
        _contextMock.Verify(c => c.SaveChangesAsync(default), Times.Once);
        Assert.True(result);
    }

    [Fact]
    public async Task UpdateUserAsync_UpdatesUserAndSavesChanges()
    {
        var user = new User { ID = 1 };
        var usersDbSetMock = new Mock<DbSet<User>>();

        _contextMock.Setup(c => c.Users).Returns(usersDbSetMock.Object);
        _contextMock.Setup(c => c.SaveChangesAsync(default)).ReturnsAsync(1);

        var result = await _userService.UpdateUserAsync(user);

        usersDbSetMock.Verify(m => m.Update(user), Times.Once);
        _contextMock.Verify(c => c.SaveChangesAsync(default), Times.Once);
        Assert.True(result);
    }

    [Fact]
    public async Task DeleteUserAsync_DeletesUserAndSavesChanges_WhenUserExists()
    {
        var userId = 1;
        var user = new User { ID = userId };
        var usersDbSetMock = new Mock<DbSet<User>>();
        usersDbSetMock.Setup(m => m.FindAsync(userId)).ReturnsAsync(user);

        _contextMock.Setup(c => c.Users).Returns(usersDbSetMock.Object);
        _contextMock.Setup(c => c.SaveChangesAsync(default)).ReturnsAsync(1);

        var result = await _userService.DeleteUserAsync(userId);

        usersDbSetMock.Verify(m => m.Remove(user), Times.Once);
        _contextMock.Verify(c => c.SaveChangesAsync(default), Times.Once);
        Assert.True(result);
    }

    [Fact]
    public async Task DeleteUserAsync_ReturnsFalse_WhenUserDoesNotExist()
    {
        var userId = 1;
        var usersDbSetMock = new Mock<DbSet<User>>();
        usersDbSetMock.Setup(m => m.FindAsync(userId)).ReturnsAsync((User)null);

        _contextMock.Setup(c => c.Users).Returns(usersDbSetMock.Object);

        var result = await _userService.DeleteUserAsync(userId);

        Assert.False(result);
    }
}
