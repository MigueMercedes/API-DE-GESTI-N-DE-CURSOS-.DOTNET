namespace study.interfaces
{
  public interface IEnrollment
  {
    int Id { get; set; }
    string StudentId { get; set; }
    string CourseId { get; set; }
    DateTime EnrollmentDate { get; set; }
  }
}
