using NetForemost.Models;
using NetForemost.Data;
using Microsoft.EntityFrameworkCore;

namespace NetForemost.Services
{
    public class AssignmentService
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<AssignmentService> _logger;

        public AssignmentService(ApplicationDbContext context, ILogger<AssignmentService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<List<Assignment>> GetAssignmentsByUserIdAsync(int userId)
        {
            _logger.LogInformation($"Fetching assignments for user ID {userId}.");
            return await _context.Assignments.Where(a => a.UserId == userId).ToListAsync();
        }

        public async Task<Assignment> GetAssignmentByIdAsync(int id)
        {
            _logger.LogInformation($"Fetching assignment with ID {id}.");
            return await _context.Assignments.FindAsync(id);
        }

        public async Task<Assignment> CreateAssignmentAsync(Assignment assignment)
        {
            _logger.LogInformation($"Creating a new assignment: {assignment.Title}");
            _context.Assignments.Add(assignment);
            await _context.SaveChangesAsync();
            return assignment;
        }

        public async Task UpdateAssignmentAsync(Assignment assignment)
        {
            _logger.LogInformation($"Updating assignment with ID {assignment.Id}");
            _context.Assignments.Update(assignment);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAssignmentAsync(int id)
        {
            _logger.LogInformation($"Deleting assignment with ID {id}");
            var assignment = await _context.Assignments.FindAsync(id);
            if (assignment != null)
            {
                _context.Assignments.Remove(assignment);
                await _context.SaveChangesAsync();
            }
        }
    }
}
