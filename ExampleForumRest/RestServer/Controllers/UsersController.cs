using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestServer.DTO;
using RestServer.Models;

namespace RestServer.Controllers
{
    /// <summary>
    /// User controller
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly ForumContext _context;
        private const int pageSize = 10;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="context">Database context</param>
        public UsersController(ForumContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Lists all users
        /// </summary>
        /// <returns>List of users</returns>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<UserDTO>), 200)]
        public async Task<ActionResult<IEnumerable<UserDTO>>> GetUsers()
        {
            return await _context.Users.Where(x => (x.Status & UserStatuses.Deleted) == 0).Select(x => UserDTO.ToDTO(x)).ToListAsync();
        }

        /// <summary>
        /// Returnss user
        /// </summary>
        /// <param name="id">User identifier</param>
        /// <returns>User data</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(UserDTO), 200)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<UserDTO>> GetUser(long id)
        {
            var user = await _context.Users.FindAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            return UserDTO.ToDTO(user);
        }

        /// <summary>
        /// Updates existing user
        /// </summary>
        /// <param name="id">User identifier</param>
        /// <param name="userDTO">Updated user data</param>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
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

        /// <summary>
        /// Creates new user
        /// </summary>
        /// <param name="userDTO">New user data</param>
        /// <returns>User data</returns>
        [HttpPost]
        [ProducesResponseType(typeof(UserDTO), 200)]
        public async Task<ActionResult<UserDTO>> PostUser(UserDTO userDTO)
        {
            var user = UserDTO.FromDTO(userDTO);

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetUser), new { id = user.Id }, UserDTO.ToDTO(user));
        }

        /// <summary>
        /// Marks existing user as deleted
        /// </summary>
        /// <param name="id">User identifier</param>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
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

        /// <summary>
        /// Lists all messages of existing user
        /// </summary>
        /// <param name="id">User identifier</param>
        /// <returns>List of messages</returns>
        [HttpGet("{id}/messages")]
        [ProducesResponseType(typeof(IEnumerable<MessageReadDTO>), 200)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
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

        /// <summary>
        /// Returns subset of up to 10 messages of existing user, starting from newest
        /// </summary>
        /// <param name="id">User identifier</param>
        /// <param name="pageNumber">Page number</param>
        /// <returns>List of messages</returns>
        [HttpGet("{id}/messages/last/{pageNumber}")]
        [ProducesResponseType(typeof(IEnumerable<MessageReadDTO>), 200)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
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
