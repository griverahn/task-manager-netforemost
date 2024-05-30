using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using NetForemost.Data;
using NetForemost.Models;
using NetForemost.Services;


namespace NetForemostTests.Tests.UserServiceTest
{
    public class UserServiceTests : IDisposable
    {
        private readonly ApplicationDbContext _context;
        private readonly UserService _userService;

        public UserServiceTests()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            _context = new ApplicationDbContext(options);
            _context.Database.EnsureCreated();

            var loggerMock = new Mock<ILogger<UserService>>();
            _userService = new UserService(_context, loggerMock.Object);

            SeedDatabase();
        }

        private void SeedDatabase()
        {
            var users = new List<User>
        {
            new User { Id = 1, FirstName = "John", LastName = "Doe", Email = "john.doe@example.com", Phone = "1234567890", CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
            new User { Id = 2, FirstName = "Jane", LastName = "Doe", Email = "jane.doe@example.com", Phone = "0987654321", CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow }
        };

            _context.Users.AddRange(users);
            _context.SaveChanges();
        }

        public void Dispose()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }

        [Fact]
        public async Task GetAllUsersAsync_ReturnsAllUsers()
        {
            var result = await _userService.GetAllUsersAsync();

            Assert.Equal(2, result.Count);
        }

        [Fact]
        public async Task GetUserByIdAsync_ReturnsUser()
        {
            var result = await _userService.GetUserByIdAsync(1);

            Assert.NotNull(result);
            Assert.Equal("John", result.FirstName);
        }

        [Fact]
        public async Task CreateUserAsync_CreatesUser()
        {
            var user = new User
            {
                FirstName = "New",
                LastName = "User",
                Email = "new.user@example.com",
                Phone = "1122334455",
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            var createdUser = await _userService.CreateUserAsync(user);

            var result = await _userService.GetUserByIdAsync(createdUser.Id);
            Assert.NotNull(result);
            Assert.Equal("New", result.FirstName);
        }

        [Fact]
        public async Task UpdateUserAsync_UpdatesUser()
        {
            var user = await _userService.GetUserByIdAsync(1);
            user.FirstName = "Updated";

            await _userService.UpdateUserAsync(user);

            var updatedUser = await _userService.GetUserByIdAsync(1);
            Assert.Equal("Updated", updatedUser.FirstName);
        }

        [Fact]
        public async Task DeleteUserAsync_DeletesUser()
        {
            await _userService.DeleteUserAsync(1);

            var result = await _userService.GetUserByIdAsync(1);
            Assert.Null(result);
        }
    }
}
