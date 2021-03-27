using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RestServer.DTO;
using RestServer.Models;

namespace RestServer.Controllers
{
    /// <summary>
    /// Message controller
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class MessagesController : ControllerBase
    {
        private readonly ForumContext _context;
        private const int pageSize = 10;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="context">Database context</param>
        public MessagesController(ForumContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Returns message
        /// </summary>
        /// <param name="id">Message identifier</param>
        /// <returns>Message data</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(MessageReadDTO), 200)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<MessageReadDTO>> GetMessage(long id)
        {
            var message = await _context.Messages.FindAsync(id);

            if (message == null)
            {
                return NotFound();
            }

            return MessageReadDTO.ToDTO(message);
        }

        /// <summary>
        /// Updates existing message. Sets message status to "edited" and editTime to current time 
        /// </summary>
        /// <param name="id">Message identifier</param>
        /// <param name="messageDTO">Updated message data</param>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
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
            message.Status |= MessageStatuses.Edited;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        /// <summary>
        /// Creates new message. Sets createdDate to current time
        /// </summary>
        /// <param name="messageDTO">New message data</param>
        /// <returns>Message data</returns>
        [HttpPost]
        [ProducesResponseType(typeof(MessageReadDTO), 200)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
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

        /// <summary>
        /// Deletes existing message
        /// </summary>
        /// <param name="id">Message identifier</param>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
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

        /// <summary>
        /// Return author of message
        /// </summary>
        /// <param name="id">Message identifier</param>
        /// <returns>Author data</returns>
        [HttpGet("{id}/author")]
        [ProducesResponseType(typeof(AuthorDTO), 200)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
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
