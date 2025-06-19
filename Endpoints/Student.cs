using Microsoft.EntityFrameworkCore;
using study.db;

static class StudentEndpoints
{
  public static void Map(WebApplication app)
  {
    app.MapGet("/student/me", async (HttpContext http, AppDbContext db) =>
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
    }).RequireAuthorization();
  }
}