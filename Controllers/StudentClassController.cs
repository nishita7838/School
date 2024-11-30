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
    public class StudentClassController : ControllerBase
    {
        private readonly ApplicationDbConext _context;

        public StudentClassController(ApplicationDbConext context)
        {
            _context = context;
        }

        [HttpGet("class/{classId}/students")]
        public IActionResult GetStudentsByClass(int classId)
        {
            var students = _context.StudentClasses
                .Where(sc => sc.ClassId == classId)
                .Select(sc => sc.Student)
                .ToList();

            if (!students.Any())
                return NotFound("No students found for this class.");

            return Ok(students);
        }

        
        [HttpPost("{studentId}/assign/{classId}")]
        public async Task<IActionResult> AssignStudentToClass(int studentId, int classId)
        {
            var student = await _context.Students.FindAsync(studentId);
            var @class = await _context.Classes.FindAsync(classId);

            if (student == null || @class == null)
                return NotFound("Student or Class not found.");

            if (_context.StudentClasses.Any(sc => sc.StudentId == studentId && sc.ClassId == classId))
                return Conflict("Student is already assigned to this class.");

            _context.StudentClasses.Add(new StudentClass { StudentId = studentId, ClassId = classId });
            await _context.SaveChangesAsync();
            return Ok("Student assigned to class.");
        }

        [HttpDelete("{studentId}/remove/{classId}")]
        public async Task<IActionResult> RemoveStudentFromClass(int studentId, int classId)
        {
            var studentClass = await _context.StudentClasses
                .FirstOrDefaultAsync(sc => sc.StudentId == studentId && sc.ClassId == classId);

            if (studentClass == null)
                return NotFound("Student is not assigned to this class.");

            _context.StudentClasses.Remove(studentClass);
            await _context.SaveChangesAsync();
            return Ok("Student removed from class.");
        }
 
    }
}
