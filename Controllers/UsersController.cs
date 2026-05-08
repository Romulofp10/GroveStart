using GroveStart.Dtos;
using GroveStart.Exceptions;
using GroveStart.Model;
using GroveStart.Repository;
using GroveStart.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace GroveStart.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ILogger<UsersController> _logger;

        public UsersController(IUserService userService, ILogger<UsersController> logger)
        {
            _userService = userService;
            _logger = logger;
        }

        [HttpGet("list/")]
        public async Task<IActionResult> List()
        {
            var users = await _userService.List();
            return Ok(users);
        }

        [HttpGet()]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var user = await _userService.Get(id);
                if (user == null) throw new NotFoundException("User", id);
                return Ok(user);
            }
            catch (NotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] User payload)
        
        {
            try
            {
                var user = await _userService.Register(payload);
                return CreatedAtAction(nameof(GetById), new { id = user.Id }, user);
            }
            catch (DuplicateEmailException ex)
            {
                return Conflict(new { message = ex.Message, email = ex.Email });
            }
        }

        [HttpPost("Auth")]
        public async Task<IActionResult> Login([FromBody] AuthUserRequest payload)
        {
            try
            {
                var user = await _userService.Login(payload.Email, payload.Password);
                return Ok(new { user, message = "Bem vindo!" });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new { message = ex.Message });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateUserRequest request)
        {
            try
            {
                await _userService.Update(id, request);
            }
            catch (NotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _userService.Delete(id);
                return NoContent();
            }
            catch (Exception error)
            {
                
                throw error;
            }
        }
    }
}