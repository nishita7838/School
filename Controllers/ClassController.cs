using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Students.Models;

namespace Students.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ClassController : ControllerBase
    {
        private readonly ApplicationDbConext _context;

        public ClassController(ApplicationDbConext context)
        {
            _context = context;
        }

        
        [HttpPost]
        public async Task<IActionResult> AddClass([FromBody] Class @class)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            _context.Classes.Add(@class);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetClassById), new { id = @class.Id }, @class);
        }

       
        [HttpPut("{id}")]
        public async Task<IActionResult> EditClass(int id, [FromBody] Class @class)
        {
            if (id != @class.Id)
                return BadRequest("Class ID mismatch.");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var existingClass = await _context.Classes.FindAsync(id);
            if (existingClass == null)
                return NotFound("Class not found.");

            existingClass.Name = @class.Name;
            existingClass.Description = @class.Description;

            await _context.SaveChangesAsync();
            return NoContent();
        }

     
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteClass(int id)
        {
            var @class = await _context.Classes.FindAsync(id);
            if (@class == null)
                return NotFound("Class not found.");

            _context.Classes.Remove(@class);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetClassById(int id)
        {
            var @class = await _context.Classes.Include(c => c.StudentClasses)
                                                .ThenInclude(sc => sc.Student)
                                                .FirstOrDefaultAsync(c => c.Id == id);

            if (@class == null)
                return NotFound("Class not found.");

            return Ok(@class);
        }

        [HttpGet]
        public IActionResult GetClasses()
        {
            var classes = _context.Classes.ToList();
            return Ok(classes);
        }
    }
}
