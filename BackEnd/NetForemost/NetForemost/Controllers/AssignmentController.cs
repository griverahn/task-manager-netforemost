using Microsoft.AspNetCore.Mvc;
using NetForemost.Models;
using NetForemost.Services;

namespace NetForemost.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class AssignmentsController : ControllerBase
    {
        private readonly AssignmentService _assignmentService;
        private readonly ILogger<AssignmentsController> _logger;

        public AssignmentsController(AssignmentService assignmentService, ILogger<AssignmentsController> logger)
        {
            _assignmentService = assignmentService;
            _logger = logger;
        }

        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetAssignmentsByUserId(int userId)
        {
            _logger.LogInformation($"GetAssignmentsByUserId called with user ID: {userId}");
            var assignments = await _assignmentService.GetAssignmentsByUserIdAsync(userId);
            return Ok(assignments);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetAssignmentById(int id)
        {
            _logger.LogInformation($"GetAssignmentById called with ID: {id}");
            var assignment = await _assignmentService.GetAssignmentByIdAsync(id);
            if (assignment == null)
            {
                _logger.LogWarning($"Assignment with ID: {id} not found.");
                return NotFound();
            }
            return Ok(assignment);
        }

        [HttpPost]
        public async Task<IActionResult> CreateAssignment(Assignment assignment)
        {
            _logger.LogInformation("CreateAssignment called.");
            var createdAssignment = await _assignmentService.CreateAssignmentAsync(assignment);
            return CreatedAtAction(nameof(GetAssignmentById), new { id = createdAssignment.Id }, createdAssignment);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAssignment(int id, Assignment assignment)
        {
            if (id != assignment.Id)
            {
                _logger.LogWarning("UpdateAssignment called with mismatched ID.");
                return BadRequest();
            }
            _logger.LogInformation($"UpdateAssignment called for ID: {id}");
            await _assignmentService.UpdateAssignmentAsync(assignment);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAssignment(int id)
        {
            _logger.LogInformation($"DeleteAssignment called for ID: {id}");
            await _assignmentService.DeleteAssignmentAsync(id);
            return NoContent();
        }
    }
}
