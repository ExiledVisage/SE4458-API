using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using WebAPI.Models;
using WebAPI.Services;
using WebAPI.DTOs;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        // POST: api/Users/register
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserRegisterDto userDto)
        {
            if (string.IsNullOrWhiteSpace(userDto.Password))
                return BadRequest("Password is required");

            try
            {
                var user = new User
                {
                    Username = userDto.Username,
                    Email = userDto.Email,
                    Role = userDto.Role
                };

                var createdUser = await _userService.RegisterAsync(user, userDto.Password);
                return CreatedAtAction(nameof(GetById), new { id = createdUser.Id }, createdUser);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // POST: api/Users/login
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserLoginDto userDto)
        {
            try
            {
                var user = await _userService.LoginAsync(userDto.Email, userDto.Password);
                if (user != null)
                    return Ok(user);
                else
                    return Unauthorized("Invalid credentials");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // GET: api/Users/5
        [HttpGet("get-user/{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var user = await _userService.GetUserByIdAsync(id);
            if (user == null)
                return NotFound();

            return Ok(user);
        }

        // PUT: api/Users/5
        [HttpPut("update-user/{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UserUpdateDto userDto)
        {
            if (id != userDto.Id)
                return BadRequest("User ID mismatch");

            var user = await _userService.GetUserByIdAsync(id);
            if (user == null)
                return NotFound("User not found");

            user.Username = userDto.Username;
            user.Email = userDto.Email;

            try
            {
                await _userService.UpdateUserAsync(user);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // DELETE: api/Users/5
        [HttpDelete("delete-user/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var user = await _userService.GetUserByIdAsync(id);
            if (user == null)
                return NotFound("User not found");

            try
            {
                await _userService.DeleteUserAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}