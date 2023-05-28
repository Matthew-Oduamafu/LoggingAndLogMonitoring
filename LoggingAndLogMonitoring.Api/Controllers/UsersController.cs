using LoggingAndLogMonitoring.Data.Entities;
using LoggingAndLogMonitoring.Domain;
using Microsoft.AspNetCore.Mvc;

namespace LoggingAndLogMonitoring.Api.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserLogic _userLogic;

        public UsersController(IUserLogic userLogic)
        {
            _userLogic = userLogic;
        }

        [HttpGet]
        public async Task<IActionResult> GetUsers()
        {
            var response = await _userLogic.GetUsersAsync();
            return Ok(response);
        }

        [HttpGet("{id:int}", Name = "GetUserById")]
        public async Task<IActionResult> GetUserById(int id)
        {
            var response = await _userLogic.GetUserByIdAsync(id);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> AddUser(User user)
        {
            var result = await _userLogic.AddUserAsync(user, nameof(user.Id));
            return CreatedAtRoute("GetUserById", new { id = result }, result);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateUser(User user)
        {
            var result = await _userLogic.UpdateAsync(user, nameof(user.Id));
            return AcceptedAtRoute("GetUserById", new { id = result }, result);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteUser([FromQuery] int id)
        {
            await _userLogic.DeleteUserAsync(id);
            return NoContent();
        }
    }
}