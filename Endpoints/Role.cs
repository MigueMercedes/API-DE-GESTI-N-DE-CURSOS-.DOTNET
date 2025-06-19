using Microsoft.EntityFrameworkCore;
using study.db;
using study.models;

namespace study.endpoints;

public class RoleEndpoints
{
  public static void Map(WebApplication app)
  {
    app.MapGet("/roles", async (AppDbContext db) =>
    {
      var roles = await db.Roles.ToListAsync();
      return Results.Ok(roles);
    });

    app.MapGet("/roles/{id}", async (AppDbContext db, int id) =>
    {
      var role = await db.Roles.FindAsync(id);
      if (role is null)
      {
        return Results.NotFound();
      }
      return Results.Ok(role);
    });

    app.MapPost("/roles", async (AppDbContext db, Role role) =>
    {
      db.Roles.Add(role);
      await db.SaveChangesAsync();
      return Results.Created($"/roles/{role.Id}", role);
    });

    app.MapPut("/roles/{id}", async (AppDbContext db, int id, Role role) =>
    {
      var existingRole = await db.Roles.FindAsync(id);
      if (existingRole is null)
      {
        return Results.NotFound();
      }
      existingRole.Name = role.Name;
      await db.SaveChangesAsync();
      return Results.Ok(existingRole);
    });

    app.MapDelete("/roles/{id}", async (AppDbContext db, int id) =>
    {
      var role = await db.Roles.FindAsync(id);
      if (role is null)
      {
        return Results.NotFound();
      }
      db.Roles.Remove(role);
      await db.SaveChangesAsync();
      return Results.NoContent();
    });
  }
}