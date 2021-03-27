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
    /// Topic controller
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class TopicsController : ControllerBase
    {
        private readonly ForumContext _context;
        private const int pageSize = 10;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="context">Database context</param>
        public TopicsController(ForumContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Returns topic
        /// </summary>
        /// <param name="id">Topic identifier</param>
        /// <returns>Topic data</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(TopicDTO), 200)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<TopicDTO>> GetTopic(long id)
        {
            var topic = await _context.Topics.FindAsync(id);

            if (topic == null)
            {
                return NotFound();
            }

            return TopicDTO.ToDTO(topic);
        }

        /// <summary>
        /// Updates existing topic 
        /// </summary>
        /// <param name="id">Topic identifier</param>
        /// <param name="topicDTO">Updated topic data</param>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
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

        /// <summary>
        /// Creates new topic
        /// </summary>
        /// <param name="topicDTO">New topic data</param>
        /// <returns>Topic data</returns>
        [HttpPost]
        [ProducesResponseType(typeof(TopicDTO), 200)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
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

        /// <summary>
        /// Deletes existing topic and all messages inside topic
        /// </summary>
        /// <param name="id">Topic identifier</param>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteTopic(long id)
        {
            var topic = await _context.Topics.FindAsync(id);
            if (topic == null)
            {
                return NotFound();
            }

            await _context.Messages.Where(x => x.Topic == topic).ForEachAsync(x => _context.Messages.Remove(x));
            _context.Topics.Remove(topic);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        /// <summary>
        /// Lists all messages in existing topic
        /// </summary>
        /// <param name="id">Topic identifier</param>
        /// <returns>List of messages</returns>
        [HttpGet("{id}/messages")]
        [ProducesResponseType(typeof(IEnumerable<MessageReadDTO>), 200)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
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

        /// <summary>
        /// Returns subset of up to 10 messages in existing topic
        /// </summary>
        /// <param name="id">Topic identifier</param>
        /// <param name="pageNumber">Page number</param>
        /// <returns>List of messages</returns>
        [HttpGet("{id}/messages/page/{pageNumber}")]
        [ProducesResponseType(typeof(IEnumerable<MessageReadDTO>), 200)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
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
