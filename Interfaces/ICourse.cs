namespace study.interfaces
{
  public interface ICourse
  {
    int Id { get; set; }
    string Title { get; set; }
    string? Description { get; set; }
    DateTime StartDate { get; set; }
    Boolean IsActive { get; set; }
  }
}