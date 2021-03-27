using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestServer.DTO;
using RestServer.Models;

namespace RestServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly ForumContext _context;
        private const int pageSize = 10;

        public UsersController(ForumContext context)
        {
            _context = context;
        }

        // GET: api/Users
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserDTO>>> GetUsers()
        {
            return await _context.Users.Where(x => (x.Status & UserStatuses.Deleted) == 0).Select(x => UserDTO.ToDTO(x)).ToListAsync();
        }

        // GET: api/Users/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<UserDTO>> GetUser(long id)
        {
            var user = await _context.Users.FindAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            return UserDTO.ToDTO(user);
        }

        // PUT: api/Users/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUser(long id, UserDTO userDTO)
        {
            if (id != userDTO.Id)
            {
                return BadRequest();
            }

            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            UserDTO.UpdateFromDTO(userDTO, user);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // POST: api/Users
        [HttpPost]
        public async Task<ActionResult<UserDTO>> PostUser(UserDTO userDTO)
        {
            var user = UserDTO.FromDTO(userDTO);

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetUser), new { id = user.Id }, UserDTO.ToDTO(user));
        }

        // DELETE: api/Users/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(long id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            user.Status = user.Status |= UserStatuses.Deleted;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // GET: api/Users/{id}/messages
        [HttpGet("{id}/messages")]
        public async Task<ActionResult<IEnumerable<MessageReadDTO>>> GetUserMessages(long id)
        {
            var user = await _context.Users.FindAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            return await _context.Messages
                .Where(x => id == x.AuthorId)
                .Select(x => MessageReadDTO.ToDTO(x))
                .ToListAsync();
        }

        // GET: api/Users/{id}/messages/last/{pageNumber}
        [HttpGet("{id}/messages/last/{pageNumber}")]
        public async Task<ActionResult<IEnumerable<MessageReadDTO>>> GetUserMessages(long id, int pageNumber)
        {
            var user = await _context.Users.FindAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            return await _context.Messages
                .Where(x => id == x.AuthorId)
                .Select(x => MessageReadDTO.ToDTO(x))
                .Reverse()
                .Skip(pageSize * (pageNumber - 1))
                .Take(pageSize)
                .ToListAsync();
        }
    }
}
