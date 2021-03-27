using System;
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
    public class MessagesController : ControllerBase
    {
        private readonly ForumContext _context;
        private const int pageSize = 10;

        public MessagesController(ForumContext context)
        {
            _context = context;
        }

        // GET: api/Messages/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<MessageReadDTO>> GetMessage(long id)
        {
            var message = await _context.Messages.FindAsync(id);

            if (message == null)
            {
                return NotFound();
            }

            return MessageReadDTO.ToDTO(message);
        }

        // PUT: api/Messages/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> PutMessage(long id, MessageWriteDTO messageDTO)
        {
            if (id != messageDTO.Id)
            {
                return BadRequest();
            }

            var message = await _context.Messages.FindAsync(id);
            if (message == null)
            {
                return NotFound();
            }

            MessageWriteDTO.UpdateFromDTO(messageDTO, message);
            message.Modified = DateTime.Now;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // POST: api/Messages
        [HttpPost]
        public async Task<ActionResult<MessageReadDTO>> PostMessage(MessageWriteDTO messageDTO)
        {
            if (!TopicExists(messageDTO.TopicId) || !UserExists(messageDTO.AuthorId))
            {
                return NotFound();
            }

            var message = MessageWriteDTO.FromDTO(messageDTO);
            message.Created = DateTime.Now;

            _context.Messages.Add(message);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetMessage), new { id = message.Id }, MessageReadDTO.ToDTO(message));
        }

        // DELETE: api/Messages/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMessage(long id)
        {
            var message = await _context.Messages.FindAsync(id);
            if (message == null)
            {
                return NotFound();
            }

            _context.Messages.Remove(message);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // GET: api/Messages/{id}/Author
        [HttpGet("{id}/author")]
        public async Task<ActionResult<AuthorDTO>> GetMessageAuthor(long id)
        {
            var message = await _context.Messages.FindAsync(id);

            if (message == null)
            {
                return NotFound();
            }

            return AuthorDTO.ToDTO(await _context.Users.FindAsync(message.AuthorId));
        }


        private bool TopicExists(long id)
        {
            return _context.Topics.Any(e => e.Id == id);
        }

        private bool UserExists(long id)
        {
            return _context.Users.Any(e => e.Id == id);
        }
    }
}
