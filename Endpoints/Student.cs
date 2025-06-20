using Microsoft.EntityFrameworkCore;
using study.db;
using study.models;

static class StudentEndpoints
{
  public static void Map(WebApplication app)
  {
    app.MapGet("/students/me", async (HttpContext http, AppDbContext db) =>
    {
      var username = http.User.Identity?.Name;

      if (string.IsNullOrEmpty(username)) return Results.Unauthorized();

      var user = await db.Users
        .Where(u => u.Username == username)
        .Select(u => new
        {
          u.Id,
          u.Username,
          u.Role
        })
        .FirstOrDefaultAsync();

      return user is null
      ? Results.NotFound()
      : Results.Ok(user);
    }).RequireAuthorization("AdminOnly");

    // Get all students
    app.MapGet("/students", async (HttpContext http, AppDbContext db) =>
    {
      var studentRole = new Role("Student");
      var students = await db.Users.Where(u => u.Role.Id == studentRole.Id).ToListAsync();

      return Results.Ok(students);
    }).RequireAuthorization("AdminOnly");
  }
}