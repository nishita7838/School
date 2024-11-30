using CsvHelper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Students.Models;
using System.Globalization;

namespace Students.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BulkImportController : ControllerBase
    {
        private readonly ApplicationDbConext _context;

        public BulkImportController(ApplicationDbConext context)
        {
            _context = context;
        }

        [HttpPost("bulk-import")]
        [RequestSizeLimit(5 * 1024 * 1024)] // 5 MB limit
        public IActionResult BulkImportStudents(IFormFile file)
        {
            if (file == null || !file.FileName.EndsWith(".csv"))
                return BadRequest("Invalid file format.");

            using var reader = new StreamReader(file.OpenReadStream());
            using var csv = new CsvReader(reader, new CsvHelper.Configuration.CsvConfiguration(CultureInfo.InvariantCulture));

            var students = csv.GetRecords<Student>().ToList();

            foreach (var student in students)
            {
                if (!ModelState.IsValid)
                    return BadRequest("Invalid data in CSV file.");

                _context.Students.Add(student);
            }

            _context.SaveChanges();
            return Ok("Students imported successfully.");
        }
       
    }
}
