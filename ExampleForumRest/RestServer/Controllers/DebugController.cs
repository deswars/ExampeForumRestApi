using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestServer.DTO.DebugDTO;
using RestServer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestServer.Controllers
{
    /// <summary>
    /// Controller for debug purposes
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class DebugController : ControllerBase
    {
        private readonly ForumContext _context;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="context">Database context</param>
        public DebugController(ForumContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Generates 2 categories, 10 topics, 3 users and 100 messages
        /// </summary>
        [HttpGet("fill")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> GetTodoItems()
        {
            int categoryCount = 2;
            int topicCount = 10;
            int userCount = 3;
            int messageCount = 100;
            var rnd = new Random();

            for (int i = 0; i < categoryCount; i++)
            {
                var category = new Category()
                {
                    Name = "Category" + i,
                    Status = TopicStatuses.Created,
                };
                if (i % 2 == 0)
                {
                    category.Status = TopicStatuses.Featured;
                }
                _context.Categories.Add(category);
            }
            await _context.SaveChangesAsync();

            for (int i = 0; i < topicCount; i++)
            {
                var topic = new Topic()
                {
                    Name = "Topic" + i,
                    Status = TopicStatuses.Created,
                    CategoryId = rnd.Next(categoryCount) + 1,
                };
                if (i % 3 == 0)
                {
                    topic.Status = TopicStatuses.Featured;
                }
                _context.Topics.Add(topic);
            }
            await _context.SaveChangesAsync();

            for (int i = 0; i < userCount; i++)
            {
                var user = new User()
                {
                    Name = "User" + i,
                    Status = UserStatuses.Created,
                    PsText = "AAaa" + i,
                };
                _context.Users.Add(user);
            }
            await _context.SaveChangesAsync();

            for (int i = 0; i < messageCount; i++)
            {
                var message = new Message()
                {
                    Text = "Message" + i,
                    AuthorId = rnd.Next(userCount) + 1,
                    TopicId = rnd.Next(topicCount) + 1,
                    Created = DateTime.Now,
                    Status = MessageStatuses.Created
                };
                if (i % 11 == 0)
                {
                    message.Status = MessageStatuses.Edited;
                    message.Modified = DateTime.Now;
                }
                _context.Messages.Add(message);
            }
            await _context.SaveChangesAsync();

            return NoContent();
        }

        /// <summary>
        /// Creates a tree representation of all forum
        /// </summary>
        /// <returns>All forum data in tree form</returns>
        [HttpGet("getall")]
        [ProducesResponseType(typeof(IEnumerable<CategoryTreeDTO>), 200)]
        public async Task<ActionResult<IEnumerable<CategoryTreeDTO>>> GetAll()
        {
            return await _context.Categories.Include(category => category.Topics).ThenInclude(topic => topic.Messages).Select(x => CategoryTreeDTO.ToDTO(x)).ToListAsync();
        }
    }
}
