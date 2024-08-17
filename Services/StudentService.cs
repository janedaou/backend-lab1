using Lab1.Models;
using Lab1.Services;

namespace Lab1.Services
{
    public class StudentService : IStudentService
    {
        private readonly IStudentRepository _repository;

        public StudentService(IStudentRepository repository)
        {
            _repository = repository;
        }

        public List<Student> GetAllStudents() => _repository.GetAllStudents();

        public Student? GetStudentById(long id) => _repository.GetStudentById(id);

        public List<Student> GetStudentsByNameFilter(string nameFilter) => _repository.GetStudentsByNameFilter(nameFilter);

        public void DeleteStudent(long id)
        {
            if (!_repository.StudentExists(id))
            {
                throw new KeyNotFoundException("Student not found.");
            }
            _repository.DeleteStudent(id);
        }

        public void UpdateStudent(Student updatedStudent)
        {
            if (!_repository.StudentExists(updatedStudent.Id))
            {
                throw new KeyNotFoundException("Student not found.");
            }
            _repository.UpdateStudent(updatedStudent);
        }
    }

    public interface IStudentService
    {
        List<Student> GetAllStudents();
        Student? GetStudentById(long id);
        List<Student> GetStudentsByNameFilter(string nameFilter);
        void DeleteStudent(long id);
        void UpdateStudent(Student updatedStudent);
    }
}
