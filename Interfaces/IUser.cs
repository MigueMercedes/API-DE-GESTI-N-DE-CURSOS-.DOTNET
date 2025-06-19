using study.models;

namespace study.interfaces
{
  public interface IUser
  {
    int Id { get; set; }
    string Username { get; set; }
    string Password { get; set; }
    Role Role { get; set; }
    int RoleId { get; set; }
  }
}