using study.interfaces;

namespace study.models
{
  public class Course : ICourse
  {
    public int Id { get; set; }
    public string Title { get; set; }
    public string? Description { get; set; }
    public DateTime StartDate { get; set; }
    public Boolean IsActive { get; set; }

    public Course(int id, string title, string? description, DateTime startDate, Boolean isActive)
    {
      Id = id;
      Title = title;
      Description = description;
      StartDate = startDate;
      IsActive = isActive;
    }

    public Course(string title, string? description, DateTime startDate)
    {
      Title = title;
      Description = description;
      StartDate = startDate;
      IsActive = true;
    }
  }

}