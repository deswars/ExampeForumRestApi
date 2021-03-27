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
    public class TopicsController : ControllerBase
    {
        private readonly ForumContext _context;
        private const int pageSize = 10;

        public TopicsController(ForumContext context)
        {
            _context = context;
        }

        // GET: api/Topics/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<TopicDTO>> GetTopic(long id)
        {
            var topic = await _context.Topics.FindAsync(id);

            if (topic == null)
            {
                return NotFound();
            }

            return TopicDTO.ToDTO(topic);
        }

        // PUT: api/Topics/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTopic(long id, TopicDTO topicDTO)
        {
            if (id != topicDTO.Id)
            {
                return BadRequest();
            }

            var topic = await _context.Topics.FindAsync(id);
            if (topic == null)
            {
                return NotFound();
            }

            TopicDTO.UpdateFromDTO(topicDTO, topic);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // POST: api/Topics
        [HttpPost]
        public async Task<ActionResult<TopicDTO>> PostTopic(TopicDTO topicDTO)
        {
            if (!CategoryExists(topicDTO.CategoryId))
            {
                return NotFound();
            }

            var topic = TopicDTO.FromDTO(topicDTO);

            _context.Topics.Add(topic);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetTopic), new { id = topic.Id }, TopicDTO.ToDTO(topic));
        }

        // DELETE: api/Topics/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTopic(long id)
        {
            var topic = await _context.Topics.FindAsync(id);
            if (topic == null)
            {
                return NotFound();
            }

            _context.Topics.Remove(topic);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // GET: api/Topics/{id}/messages
        [HttpGet("{id}/messages")]
        public async Task<ActionResult<IEnumerable<MessageReadDTO>>> GetTopicMessages(long id)
        {
            var topic = await _context.Topics.FindAsync(id);

            if (topic == null)
            {
                return NotFound();
            }

            return await _context.Messages
                .Where(x => id == x.TopicId)
                .Select(x => MessageReadDTO.ToDTO(x))
                .ToListAsync();
        }

        // GET: api/Topics/{id}/messages/page/{pageNumber}
        [HttpGet("{id}/messages/page/{pageNumber}")]
        public async Task<ActionResult<IEnumerable<MessageReadDTO>>> GetTopicMessagesPaged(long id, int pageNumber)
        {
            var topic = await _context.Topics.FindAsync(id);

            if (topic == null)
            {
                return NotFound();
            }

            return await _context.Messages
                .Where(x => id == x.TopicId)
                .Select(x => MessageReadDTO.ToDTO(x))
                .Skip(pageSize * (pageNumber - 1))
                .Take(pageSize)
                .ToListAsync();
        }

        private bool CategoryExists(long id)
        {
            return _context.Categories.Any(e => e.Id == id);
        }
    }
}
