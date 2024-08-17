using Lab1.Models;

namespace Lab1.Services
{
    public class StudentRepository : IStudentRepository
    {
        private readonly List<Student> _students;

        public StudentRepository()
        {
            _students = new List<Student>
            {
                new Student { Id = 1, Firstname = "John", LastName = "Doe", FatherName = "Michael", MotherName = "Sarah", Email = "john.doe@example.com", Age = 20 },
                new Student { Id = 2, Firstname = "Jane", LastName = "Smith", FatherName = "Robert", MotherName = "Emily", Email = "jane.smith@example.com", Age = 22 },
                new Student { Id = 3, Firstname = "Alice", LastName = "Johnson", FatherName = "Peter", MotherName = "Laura", Email = "alice.johnson@example.com", Age = 21 },
                new Student { Id = 4, Firstname = "Bob", LastName = "Brown", FatherName = "William", MotherName = "Helen", Email = "bob.brown@example.com", Age = 23 },
                new Student { Id = 5, Firstname = "Charlie", LastName = "Davis", FatherName = "James", MotherName = "Mary", Email = "charlie.davis@example.com", Age = 19 },
                new Student { Id = 6, Firstname = "Diana", LastName = "Wilson", FatherName = "Edward", MotherName = "Elizabeth", Email = "diana.wilson@example.com", Age = 24 }
            };
        }

        public List<Student> GetAllStudents() => _students;

        public Student? GetStudentById(long id) => _students.FirstOrDefault(s => s.Id == id);

        public List<Student> GetStudentsByNameFilter(string nameFilter)
        {
            // Using LINQ to filter students by name
            return _students
                .Where(s => s.Firstname.Contains(nameFilter, StringComparison.OrdinalIgnoreCase) 
                         || s.LastName.Contains(nameFilter, StringComparison.OrdinalIgnoreCase))
                .ToList();
        }

        public void DeleteStudent(long id)
        {
            // Using LINQ to find and remove a student by ID
            var student = _students.FirstOrDefault(s => s.Id == id);
            if (student != null)
            {
                _students.Remove(student);
            }
        }

        public bool StudentExists(long id)
        {
            // Using LINQ to check if a student exists by ID
            return _students.Any(s => s.Id == id);
        }

        public void UpdateStudent(Student updatedStudent)
        {
            // Using LINQ to find the student and update their details
            var student = _students.FirstOrDefault(s => s.Id == updatedStudent.Id);
            if (student != null)
            {
                student.Firstname = updatedStudent.Firstname;
                student.LastName = updatedStudent.LastName;
                student.Email = updatedStudent.Email;
                student.FatherName = updatedStudent.FatherName;
                student.MotherName = updatedStudent.MotherName;
                student.Age = updatedStudent.Age;
            }
        }
    }

    public interface IStudentRepository
    {
        List<Student> GetAllStudents();
        Student? GetStudentById(long id);
        List<Student> GetStudentsByNameFilter(string nameFilter);
        void DeleteStudent(long id);
        bool StudentExists(long id);
        void UpdateStudent(Student updatedStudent);
    }
}
