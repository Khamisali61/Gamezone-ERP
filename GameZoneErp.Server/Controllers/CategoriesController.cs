using GameZoneErp.Server.Data;
using GameZoneErp.Shared.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GameZoneErp.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly GameZoneDbContext _context;

        public CategoriesController(GameZoneDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<List<Category>>> GetCategories()
        {
            // Fetch all categories
            var categories = await _context.Categories.ToListAsync();

            // Reconstruct tree on client or server?
            // The entity has `Children` list. If I include it, EF might handle it or I might need to build it.
            // But EF Core loading self-referencing data can be tricky with serialization cycles.
            // I'll return the flat list and let the client build the tree, OR I'll return root nodes.
            // Let's return the flat list for now to avoid cycle issues in JSON unless I configure ReferenceHandler.Preserve.
            // Actually, returning flat list is safer for Blazor WASM to reconstruct.
            return categories;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Category>> GetCategory(int id)
        {
            var category = await _context.Categories.FindAsync(id);

            if (category == null)
            {
                return NotFound();
            }

            return category;
        }

        [HttpPost]
        public async Task<ActionResult<Category>> PostCategory(Category category)
        {
            _context.Categories.Add(category);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCategory", new { id = category.Id }, category);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutCategory(int id, Category category)
        {
            if (id != category.Id)
            {
                return BadRequest();
            }

            _context.Entry(category).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CategoryExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category == null)
            {
                return NotFound();
            }

            // Check for children or products
            if (await _context.Categories.AnyAsync(c => c.ParentId == id) ||
                await _context.Products.AnyAsync(p => p.CategoryId == id))
            {
                 return BadRequest("Cannot delete category with children or products.");
            }

            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool CategoryExists(int id)
        {
            return _context.Categories.Any(e => e.Id == id);
        }
    }
}
