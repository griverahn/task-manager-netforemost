using NetForemost.Models;
using NetForemost.Data;
using Microsoft.EntityFrameworkCore;

namespace NetForemost.Services
{
    public class UserService
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<UserService> _logger;

        public UserService(ApplicationDbContext context, ILogger<UserService> logger)
        {
            _context = context;
            _logger = logger;
        }
        public async Task<List<User>> GetAllUsersAsync()
        {
            _logger.LogInformation("Fetching all users from the database.");
            return await _context.Users.ToListAsync();
        }

        public async Task<User> GetUserByIdAsync(int id)
        {
            _logger.LogInformation($"Fetching user with ID {id} from the database.");
            return await _context.Users.FindAsync(id);
        }

        public async Task<User> CreateUserAsync(User user)
        {
            _logger.LogInformation($"Creating a new user: {user.FirstName} {user.LastName}");
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return user;
        }

        public async Task UpdateUserAsync(User user)
        {
            _logger.LogInformation($"Updating user with ID {user.Id}");
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteUserAsync(int id)
        {
            _logger.LogInformation($"Deleting user with ID {id}");
            var user = await _context.Users.FindAsync(id);
            if (user != null)
            {
                _context.Users.Remove(user);
                await _context.SaveChangesAsync();
            }
        }
    }
}
