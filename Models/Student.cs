using study.interfaces;

namespace study.models
{
    public class Student : IStudent
    {
        public int Id { get; set; }
        public required string FullName { get; set; }
        public required string Email { get; set; }
        public int UserId { get; set; }

        public Student(int id, string fullName, string email, int userId)
        {
            Id = id;
            FullName = fullName;
            Email = email;
            UserId = userId;
        }

        public Student(string fullName, string email, int userId)
        {
            FullName = fullName;
            Email = email;
            UserId = userId;
        }
    }
}