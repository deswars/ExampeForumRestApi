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
    /// Category controller
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly ForumContext _context;
        private const int pageSize = 10;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="context">Database context</param>
        public CategoriesController(ForumContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Lists all categories
        /// </summary>
        /// <returns>List of categories</returns>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<CategoryDTO>), 200)]
        public async Task<ActionResult<IEnumerable<CategoryDTO>>> GetGategories()
        {
            return await _context.Categories.Select(x => CategoryDTO.ToDTO(x)).ToListAsync();
        }

        /// <summary>
        /// Returns category
        /// </summary>
        /// <param name="id">Category identifier</param>
        /// <returns>Category data</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(CategoryDTO), 200)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<CategoryDTO>> GetCategory(long id)
        {
            var category = await _context.Categories.FindAsync(id);

            if (category == null)
            {
                return NotFound();
            }

            return CategoryDTO.ToDTO(category);
        }

        /// <summary>
        /// Updates existing category
        /// </summary>
        /// <param name="id">Category identifier</param>
        /// <param name="categoryDTO">Updated category data</param>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
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

        /// <summary>
        /// Creates new category
        /// </summary>
        /// <param name="categoryDTO">New category data</param>
        /// <returns>Category data</returns>
        [HttpPost]
        [ProducesResponseType(typeof(CategoryDTO), 200)]
        public async Task<ActionResult<CategoryDTO>> PostCategory(CategoryDTO categoryDTO)
        {
            var category = CategoryDTO.FromDTO(categoryDTO);

            _context.Categories.Add(category);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetCategory), new { id = category.Id }, CategoryDTO.ToDTO(category));
        }

        /// <summary>
        /// Deletes existing category, including topics and messages
        /// </summary>
        /// <param name="id">Category identifier</param>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteCategory(long id)
        {
            var cattegory = await _context.Categories.FindAsync(id);
            if (cattegory == null)
            {
                return NotFound();
            }

            var topics = _context.Topics.Where(x => x.CategoryId == id).Include(x => x.Messages);
            await topics.ForEachAsync(x => _context.Messages.RemoveRange(x.Messages));
            _context.Topics.RemoveRange(topics);
            _context.Categories.Remove(cattegory);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        /// <summary>
        /// Lists all topics in existing category
        /// </summary>
        /// <param name="id">Category identifier</param>
        /// <returns>List of topics</returns>
        [HttpGet("{id}/topics")]
        [ProducesResponseType(typeof(IEnumerable<TopicDTO>), 200)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
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

        /// <summary>
        /// Returns subset of up to 10 topics in existing category
        /// </summary>
        /// <param name="id">Category identifier</param>
        /// <param name="pageNumber">Page number</param>
        /// <returns>List of topics</returns>
        [HttpGet("{id}/topics/page/{pageNumber}")]
        [ProducesResponseType(typeof(IEnumerable<TopicDTO>), 200)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
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
