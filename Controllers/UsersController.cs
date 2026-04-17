using GroveStart.Model;
using GroveStart.Repository;
using GroveStart.Services;
using Microsoft.AspNetCore.Mvc;

namespace GroveStart.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public async Task<IActionResult> List()
        {
            var users = await _userService.List();
            return Ok(users);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var user = await _userService.Get(id);
            if (user == null) return NotFound();
            return Ok(user);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] User payload)
        {
            var user = await _userService.Register(payload);
            
            return CreatedAtAction(nameof(GetById), new { id = user.Id }, user);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] User user)
        {
            var existingUser = await _userService.Get(user.Id);
            if (existingUser == null) return NotFound();

            // Atualizar propriedades
            // existingUser.Name = user.Name;
            // existingUser.Email = user.Email;
            // etc.

           await  _userService.Update(existingUser);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
        
            return NoContent();
        }
    }
}