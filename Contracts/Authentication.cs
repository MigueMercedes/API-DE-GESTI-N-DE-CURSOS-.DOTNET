using study.models;

namespace study.contracts
{
  public record LoginRequest(string Username, string Password);
  public record LoginResponse(string Token);
  public record RegisterRequest(string Username, string Password, int RoleId);
  public record RegisterResponse(string Username);
}

