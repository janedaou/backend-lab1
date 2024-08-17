using Lab1.Models;
using System.Collections.Generic;
using System.Linq;

namespace Lab1.Services
{
    public class StudentRepository
    {
        private static StudentRepository? _instance;
        private readonly List<Student> _students;

        private StudentRepository()
        {
            _students = new List<Student>
            {
                new Student { Id = 1, Firstname = "John", LastName = "Doe", FatherName = "Michael", MotherName = "Sarah", Email = "john.doe@example.com", Age = 20 },
                new Student { Id = 2, Firstname = "Jane", LastName = "Smith", FatherName = "Robert", MotherName = "Emily", Email = "jane.smith@example.com", Age = 22 },
                new Student { Id = 3, Firstname = "Alice", LastName = "Johnson", FatherName = "James", MotherName = "Linda", Email = "alice.johnson@example.com", Age = 19 },
                new Student { Id = 4, Firstname = "Bob", LastName = "Williams", FatherName = "David", MotherName = "Karen", Email = "bob.williams@example.com", Age = 23 },
                new Student { Id = 5, Firstname = "Charlie", LastName = "Brown", FatherName = "Thomas", MotherName = "Patricia", Email = "charlie.brown@example.com", Age = 21 },
                new Student { Id = 6, Firstname = "Diana", LastName = "Taylor", FatherName = "Paul", MotherName = "Jessica", Email = "diana.taylor@example.com", Age = 20 }
            };
        }

        public static StudentRepository Instance => _instance ??= new StudentRepository();

        public List<Student> GetAllStudents() => _students;

        public Student? GetStudentById(long id) => _students.FirstOrDefault(s => s.Id == id);

        public List<Student> GetStudentsByNameFilter(string nameFilter) =>
            _students.Where(s => s.Firstname.Contains(nameFilter, StringComparison.OrdinalIgnoreCase) || s.LastName.Contains(nameFilter, StringComparison.OrdinalIgnoreCase)).ToList();

        public bool DeleteStudent(long id)
        {
            var student = _students.FirstOrDefault(s => s.Id == id);
            if (student == null) return false;

            _students.Remove(student);
            return true;
        }
    }
}
