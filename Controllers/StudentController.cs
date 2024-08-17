using Lab1.Models;
using Lab1.Services;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System.IO;
using System.Threading.Tasks;

namespace Lab1.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StudentController : ControllerBase
    {
        private readonly StudentRepository _repository = StudentRepository.Instance;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public StudentController(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
        }

        [HttpGet]
        public IActionResult GetAllStudents()
        {
            return Ok(_repository.GetAllStudents());
        }

        [HttpGet("{id:long}")]
        public IActionResult GetStudentById(long id)
        {
            var student = _repository.GetStudentById(id);
            if (student == null) return NotFound();
            return Ok(student);
        }

        [HttpGet("filter")]
        public IActionResult GetStudentsByNameFilter([FromQuery] string name)
        {
            var students = _repository.GetStudentsByNameFilter(name);
            if (!students.Any()) return NotFound();
            return Ok(students);
        }

        [HttpGet("current-date")]
        public IActionResult GetCurrentDate([FromQuery] string culture = "en-US")
        {
            // Define valid cultures
            string[] validCultures = { "en-US", "es-ES", "fr-FR" };

            // Normalize and validate the culture
            var cultureToUse = validCultures.Contains(culture.ToLowerInvariant())
                ? culture.ToLowerInvariant()
                : "en-US"; // Default to en-US if invalid

            // Set the culture explicitly
            CultureInfo currentCulture;
            try
            {
                currentCulture = new CultureInfo(cultureToUse);
            }
            catch (CultureNotFoundException)
            {
                currentCulture = new CultureInfo("en-US");
            }

            // Format the current date and time
            var currentDate = DateTime.Now.ToString("F", currentCulture);

            return Ok(currentDate);
        }

        [HttpPut("{id:long}")]
        public IActionResult UpdateStudent(long id, [FromBody] Student updatedStudent)
        {
            // Find the existing student by ID
            var student = _repository.GetStudentById(id);

            if (student == null)
            {
                return NotFound(new { Message = "Student not found." });
            }

            // Update the student's properties
            student.Firstname = updatedStudent.Firstname;
            student.LastName = updatedStudent.LastName;
            student.Email = updatedStudent.Email;
            student.FatherName = updatedStudent.FatherName;
            student.MotherName = updatedStudent.MotherName;
            student.Age = updatedStudent.Age;

            // Save the changes (in this case, since we're using an in-memory list, the changes are automatically saved)
            return Ok(student);
        }

        [HttpPost("UploadImage")]
        public async Task<IActionResult> UploadImage([FromForm] IFormFile imageFile)
        {
            if (imageFile == null || imageFile.Length == 0)
            {
                return BadRequest("No file selected.");
            }

            string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "uploads");
            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
            }

            string fileName = Guid.NewGuid().ToString() + Path.GetExtension(imageFile.FileName);
            string filePath = Path.Combine(uploadsFolder, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await imageFile.CopyToAsync(stream);
            }

            return Ok(new { FilePath = $"uploads/{fileName}" });
        }

        [HttpDelete("{id:long}")]
        public IActionResult DeleteStudent(long id)
        {
            // Find the existing student by ID
            var student = _repository.GetStudentById(id);

            if (student == null)
            {
                return NotFound(new { Message = "Student not found." });
            }

            // Remove the student from the repository
            bool removed = _repository.DeleteStudent(id);

            if (!removed)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { Message = "Failed to delete student." });
            }

            return NoContent(); // Status code 204 for successful deletion with no content
        }
    }
}
