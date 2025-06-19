using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using study.db;
using study.models;
using study.contracts;

namespace study.endpoints;

public class AuthEndpoints
{
  public static void Map(WebApplication app)
  {
    var jwtSettings = app.Configuration.GetSection("JwtSettings");
    var secretKey = jwtSettings["Key"];
    var issuer = jwtSettings["Issuer"];
    var audience = jwtSettings["Audience"];

    app.MapPost("/auth/register", async (RegisterRequest req, AppDbContext db) =>
    {
      var existingUser = await db.Users.FirstOrDefaultAsync(u => u.Username == req.Username);
      if (existingUser is not null) return Results.BadRequest(new { error = "User already exists" });

      var (username, password, roleId) = req;

      // Find the role
      var role = await db.Roles.FindAsync(roleId);
      if (role is null) return Results.BadRequest(new { error = "Invalid role" });

      var hashedPassword = BCrypt.Net.BCrypt.HashPassword(password);
      var user = new User
      {
        Username = username,
        Password = hashedPassword,
        Role = role,
        RoleId = role.Id
      };

      db.Users.Add(user);
      await db.SaveChangesAsync();

      return Results.Ok(new { message = $"User {username} created successfully" });
    });

    app.MapPost("/auth/login", async (LoginRequest req, AppDbContext db, IConfiguration config) =>
    {
      var foundUser = await db.Users
        .Include(u => u.Role)
        .FirstOrDefaultAsync(u => u.Username == req.Username);

      if (foundUser is null) return Results.NotFound(new { error = "User not found" });

      var isPasswordValid = BCrypt.Net.BCrypt.Verify(req.Password, foundUser.Password);
      if (!isPasswordValid) return Results.Unauthorized();

      var claims = new[]
      {
        new Claim(ClaimTypes.NameIdentifier, foundUser.Id.ToString()),
        new Claim(ClaimTypes.Name, foundUser.Username),
        new Claim(ClaimTypes.Role, foundUser.Role.Name)
      };

      var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey!));
      var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

      var token = new JwtSecurityToken(
        issuer: issuer,
        audience: audience,
        claims: claims,
        expires: DateTime.UtcNow.AddHours(1),
        signingCredentials: credentials
      );

      var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

      return Results.Ok(new LoginResponse(tokenString));
    });
  }
}

