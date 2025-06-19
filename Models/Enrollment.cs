using study.interfaces;

namespace study.models
{
  public class Enrollment : IEnrollment
  {
    public int Id { get; set; }
    public string StudentId { get; set; }
    public string CourseId { get; set; }
    public DateTime EnrollmentDate { get; set; }

    public Enrollment(int id, string studentId, string courseId, DateTime enrollmentDate)
    {
      Id = id;
      StudentId = studentId;
      CourseId = courseId;
      EnrollmentDate = enrollmentDate;
    }

    public Enrollment(string studentId, string courseId, DateTime enrollmentDate)
    {
      StudentId = studentId;
      CourseId = courseId;
      EnrollmentDate = enrollmentDate;
    }
  }
}