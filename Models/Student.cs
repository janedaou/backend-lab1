using System.ComponentModel.DataAnnotations;

namespace Lab1.Models
{
    public class Student
    {
        public long Id { get; set; }

        [Required]
        public string Firstname { get; set; } = string.Empty;

        [Required]
        public string LastName { get; set; } = string.Empty;

        [Required]
        public string FatherName { get; set; } = string.Empty;

        [Required]
        public string MotherName { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        public int Age { get; set; }
    }
}
