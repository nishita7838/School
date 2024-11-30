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
    public class StudentController : ControllerBase
    {
        private readonly ApplicationDbConext _context;
        public StudentController(ApplicationDbConext context) {
            _context = context;
        }
        [HttpPost]
        public async Task<IActionResult> AddStudent([FromBody] Student student)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);

            }
            if(_context.Students.Any(s=>s.EmailId==student.EmailId|| s.PhoneNumber==student.PhoneNumber)) 
            {
                return Conflict("Email or phone number already exists");
                    }
            else
            {
              _context.Students.Add(student);
                await _context.SaveChangesAsync();
                return CreatedAtAction(nameof(GetStudentById), new { id = student.Id }, student);

            }
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> EditStudent(int id, [FromBody] Student student)
        {
            if (id != student.Id)
                return BadRequest("Student ID mismatch.");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var existingStudent = await _context.Students.FindAsync(id);
            if (existingStudent == null)
                return NotFound("Student not found.");

            if (_context.Students.Any(s => (s.EmailId == student.EmailId || s.PhoneNumber == student.PhoneNumber) && s.Id != id))
                return Conflict("Email or Phone number already exists.");

            existingStudent.FirstName = student.FirstName;
            existingStudent.LastName = student.LastName;
            existingStudent.PhoneNumber = student.PhoneNumber;
            existingStudent.EmailId = student.EmailId;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteStudent(int id)
        {
            var student = await _context.Students.FindAsync(id);
            if (student == null)
                return NotFound("Student not found.");

            _context.Students.Remove(student);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        
        [HttpGet("{id}")]
        public async Task<IActionResult> GetStudentById(int id)
        {
            var student = await _context.Students
                .Include(s => s.StudentClasses)
                .ThenInclude(sc => sc.Class)
                .FirstOrDefaultAsync(s => s.Id == id);

            if (student == null)
                return NotFound("Student not found.");

            return Ok(student);
        }

       
        [HttpGet]
        public IActionResult GetStudents([FromQuery] string search, [FromQuery] string sortBy = "FirstName", [FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            var query = _context.Students.AsQueryable();

            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(s => s.FirstName.Contains(search) || s.LastName.Contains(search) || s.EmailId.Contains(search));
            }

            switch (sortBy.ToLower())
            {
                case "lastname":
                    query = query.OrderBy(s => s.LastName);
                    break;
                case "email":
                    query = query.OrderBy(s => s.EmailId);
                    break;
                default:
                    query = query.OrderBy(s => s.FirstName);
                    break;
            }

            var students = query.Skip((page - 1) * pageSize).Take(pageSize).ToList();
            return Ok(students);
        }
    }

}
