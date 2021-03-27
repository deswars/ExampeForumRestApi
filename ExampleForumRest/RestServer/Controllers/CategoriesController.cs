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
    public class CategoriesController : ControllerBase
    {
        private readonly ForumContext _context;
        private const int pageSize = 10;

        public CategoriesController(ForumContext context)
        {
            _context = context;
        }

        // GET: api/Cattegories
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CategoryDTO>>> GetGategories()
        {
            return await _context.Categories.Select(x => CategoryDTO.ToDTO(x)).ToListAsync();
        }

        // GET: api/Cattegories/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<CategoryDTO>> GetCategory(long id)
        {
            var category = await _context.Categories.FindAsync(id);

            if (category == null)
            {
                return NotFound();
            }

            return CategoryDTO.ToDTO(category);
        }

        // PUT: api/Cattegories/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCategory(long id, CategoryDTO categoryDTO)
        {
            if (id != categoryDTO.Id)
            {
                return BadRequest();
            }

            var category = await _context.Categories.FindAsync(id);
            if (category == null)
            {
                return NotFound();
            }

            CategoryDTO.UpdateFromDTO(categoryDTO, category);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // POST: api/Cattegories
        [HttpPost]
        public async Task<ActionResult<CategoryDTO>> PostCategory(CategoryDTO categoryDTO)
        {
            var category = CategoryDTO.FromDTO(categoryDTO);

            _context.Categories.Add(category);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetCategory), new { id = category.Id }, CategoryDTO.ToDTO(category));
        }

        // DELETE: api/Cattegories/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategory(long id)
        {
            var cattegory = await _context.Categories.FindAsync(id);
            if (cattegory == null)
            {
                return NotFound();
            }

            //await _context.Messages.Where(x => x.Topic == topic).ForEachAsync(x => _context.Messages.Remove(x));

            var topics = _context.Topics.Where(x => x.CategoryId == id).Include(x => x.Messages);
            await topics.ForEachAsync(x => _context.Messages.RemoveRange(x.Messages));
            _context.Topics.RemoveRange(topics);
            _context.Categories.Remove(cattegory);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // GET: api/Cattegories/{id}/topics
        [HttpGet("{id}/topics")]
        public async Task<ActionResult<IEnumerable<TopicDTO>>> GetCategoryTopics(long id)
        {
            var category = await _context.Categories.FindAsync(id);

            if (category == null)
            {
                return NotFound();
            }

            return await _context.Topics
                .Where(x => id == x.CategoryId)
                .Select(x => TopicDTO.ToDTO(x))
                .ToListAsync();
        }

        // GET: api/Cattegories/{id}/topics/page/{pageNumber}
        [HttpGet("{id}/topics/page/{pageNumber}")]
        public async Task<ActionResult<IEnumerable<TopicDTO>>> GetCategoryTopicsPaged(long id, int pageNumber)
        {
            var category = await _context.Categories.FindAsync(id);

            if (category == null)
            {
                return NotFound();
            }

            return await _context.Topics
                .Where(x => id == x.CategoryId)
                .Select(x => TopicDTO.ToDTO(x))
                .Skip(pageSize*(pageNumber-1))
                .Take(pageSize)
                .ToListAsync();
        }
    }
}
