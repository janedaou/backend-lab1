using Lab1.Models;
using Lab1.Services;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;

namespace Lab1.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StudentController : ControllerBase
    {
        private readonly IStudentService _studentService;
        private readonly IWebHostEnvironment _environment;

        public StudentController(IStudentService studentService, IWebHostEnvironment environment)
        {
            _studentService = studentService;
            _environment = environment;
        }

        [HttpGet]
        public IActionResult GetAllStudents()
        {
            return Ok(_studentService.GetAllStudents());
        }

        [HttpGet("{id:long}")]
        public IActionResult GetStudentById(long id)
        {
            try
            {
                var student = _studentService.GetStudentById(id);
                if (student == null) return NotFound();
                return Ok(student);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        [HttpGet("filter")]
        public IActionResult GetStudentsByNameFilter([FromQuery] string name)
        {
            var students = _studentService.GetStudentsByNameFilter(name);
            if (!students.Any()) return NotFound();
            return Ok(students);
        }

        [HttpGet("current-date")]
        public IActionResult GetCurrentDate([FromQuery] string culture = "en-US")
        {
            try
            {
                var supportedCultures = CultureInfo.GetCultures(CultureTypes.SpecificCultures);

                if (!supportedCultures.Any(c => c.Name.Equals(culture, StringComparison.OrdinalIgnoreCase)))
                {
                    throw new ArgumentException("Unsupported culture.");
                }

                var currentCulture = new CultureInfo(culture);
                var currentDate = DateTime.Now.ToString("F", currentCulture);

                return Ok(currentDate);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        [HttpPut("{id:long}")]
        public IActionResult UpdateStudent(long id, [FromBody] Student updatedStudent)
        {
            try
            {
                if (id <= 0) throw new ArgumentException("Invalid student ID.");

                updatedStudent.Id = id;
                _studentService.UpdateStudent(updatedStudent);

                return Ok(updatedStudent);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        [HttpDelete("{id:long}")]
        public IActionResult DeleteStudent(long id)
        {
            try
            {
                if (id <= 0) throw new ArgumentException("Invalid student ID.");

                _studentService.DeleteStudent(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        [HttpPost("upload-image")]
        public IActionResult UploadImage([FromForm] IFormFile file)
        {
            try
            {
                if (file == null || file.Length == 0)
                    throw new ArgumentException("Invalid file.");

                var uploadsFolder = Path.Combine(_environment.WebRootPath, "uploads");
                Directory.CreateDirectory(uploadsFolder);

                var filePath = Path.Combine(uploadsFolder, file.FileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    file.CopyTo(stream);
                }

                return Ok(new { Message = "File uploaded successfully." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }
    }
}
