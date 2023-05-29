using LoggingAndLogMonitoring.Data.Entities;
using LoggingAndLogMonitoring.Domain;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace LoggingAndLogMonitoring.Api.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly ILogger<UsersController> _logger;
        private readonly IUserLogic _userLogic;

        public UsersController(IUserLogic userLogic, ILogger<UsersController> logger)
        {
            _userLogic = userLogic;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetUsers()
        {
            _logger.LogInformation("Getting all users available in API");
            var response = await _userLogic.GetUsersAsync();
            return Ok(response);
        }

        [HttpGet("{id:int}", Name = "GetUserById")]
        public async Task<IActionResult> GetUserById(int id)
        {
            _logger.LogDebug("Getting a single User in API for {Id}", id);
            var response = await _userLogic.GetUserByIdAsync(id);
            if (response == null)
            {
                _logger.LogWarning("No user found for ID: {Id}", id);
                return NotFound();
            }

            return Ok(response);
        }

        [HttpGet("{id:int}", Name = "GetUserByIdNonAsync")]
        public IActionResult GetUserByIdNon(int id)
        {
            _logger.LogDebug("Getting a single User in API for {Id}", id);
            var response = _userLogic.GetUser(id);
            if (response == null)
            {
                _logger.LogWarning("No user found for ID: {Id}", id);
                return NotFound();
            }

            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> AddUser(User user)
        {
            _logger.LogInformation("Adding a new user in API with details {User}", JsonConvert.SerializeObject(user));
            var result = await _userLogic.AddUserAsync(user, nameof(user.Id));
            return CreatedAtRoute("GetUserById", new {id = result}, result);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateUser(User user)
        {
            _logger.LogInformation("Updating a user in API with new details {User}", JsonConvert.SerializeObject(user));
            var result = await _userLogic.UpdateAsync(user, nameof(user.Id));
            return AcceptedAtRoute("GetUserById", new {id = result}, result);
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteUser([FromQuery] int id)
        {
            _logger.LogInformation("Deleting user in API with Id {Id}", id);
            await _userLogic.DeleteUserAsync(id);
            return NoContent();
        }
    }
}