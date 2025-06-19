using study.interfaces;

namespace study.models
{
  public class User : IUser
  {
    public int Id { get; set; }
    public required string Username { get; set; }
    public required string Password { get; set; }
    public required Role Role { get; set; }
    public int RoleId { get; set; }

    // Required by EF Core
    public User() { }

    public User(int id, string username, string password, Role role)
    {
      Id = id;
      Username = username;
      Password = password;
      Role = role;
      RoleId = role.Id;
    }

    public User(string username, string password, Role role)
    {
      Username = username;
      Password = password;
      Role = role;
      RoleId = role.Id;
    }
  }
}
