using study.interfaces;

namespace study.models
{
  public class Role : IRole
  {
    public int Id { get; set; }
    public string Name { get; set; }

    public Role() { }

    public Role(int id, string name)
    {
      Id = id;
      Name = name;
    }

    public Role(string name)
    {
      Name = name;
    }
  }
}