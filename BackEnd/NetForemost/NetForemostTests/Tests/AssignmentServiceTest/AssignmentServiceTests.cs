using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using NetForemost.Data;
using NetForemost.Models;
using NetForemost.Services;

namespace NetForemostTests.Tests.AssignmentServiceTest
{
    public class AssignmentServiceTests : IDisposable
    {
        private readonly ApplicationDbContext _context;
        private readonly AssignmentService _assignmentService;

        public AssignmentServiceTests()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            _context = new ApplicationDbContext(options);
            _context.Database.EnsureCreated();

            var loggerMock = new Mock<ILogger<AssignmentService>>();
            _assignmentService = new AssignmentService(_context, loggerMock.Object);

            SeedDatabase();
        }

        private void SeedDatabase()
        {
            var assignments = new List<Assignment>
        {
            new Assignment { Id = 1, UserId = 1, Title = "Assignment 1", Description = "Description 1", DueDate = DateTime.UtcNow.AddDays(1), IsCompleted = false, IsDeleted = false, Tags = "Tag1", PriorityId = 1, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
            new Assignment { Id = 2, UserId = 1, Title = "Assignment 2", Description = "Description 2", DueDate = DateTime.UtcNow.AddDays(2), IsCompleted = false, IsDeleted = false, Tags = "Tag2", PriorityId = 2, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
            new Assignment { Id = 3, UserId = 2, Title = "Assignment 3", Description = "Description 3", DueDate = DateTime.UtcNow.AddDays(3), IsCompleted = false, IsDeleted = false, Tags = "Tag3", PriorityId = 3, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow }
        };

            _context.Assignments.AddRange(assignments);
            _context.SaveChanges();
        }

        [Fact]
        public async Task GetAssignmentsByUserIdAsync_ReturnsAssignmentsForUser()
        {
            var result = await _assignmentService.GetAssignmentsByUserIdAsync(1);

            Assert.Equal(2, result.Count);
        }

        [Fact]
        public async Task GetAssignmentByIdAsync_ReturnsAssignment()
        {
            var result = await _assignmentService.GetAssignmentByIdAsync(1);

            Assert.NotNull(result);
            Assert.Equal("Assignment 1", result.Title);
        }

        [Fact]
        public async Task CreateAssignmentAsync_CreatesAssignment()
        {
            var assignment = new Assignment
            {
                UserId = 1,
                Title = "New Assignment",
                Description = "New Assignment Description",
                DueDate = DateTime.UtcNow.AddDays(5),
                IsCompleted = false,
                IsDeleted = false,
                Tags = "NewTag",
                PriorityId = 2,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            var createdAssignment = await _assignmentService.CreateAssignmentAsync(assignment);

            var result = await _assignmentService.GetAssignmentByIdAsync(createdAssignment.Id);
            Assert.NotNull(result);
            Assert.Equal("New Assignment", result.Title);
        }
        public void Dispose()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }
    }
}
