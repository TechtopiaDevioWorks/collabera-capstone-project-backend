using Microsoft.EntityFrameworkCore;
using WebApi.Helpers;
public static class DbInitializer
{
    public static void Initialize(DataContext context)
    {
        //create the database
        context.Database.EnsureCreated();

        // Look for any roles.
        if (context.Roles.Any())
        {
            return;   // DB has been seeded
        }

        var roles = new WebApi.Entities.Role[]
        {
        new WebApi.Entities.Role{id=1,name="Employee"},
        new WebApi.Entities.Role{id=2,name="Manager"},
        new WebApi.Entities.Role{id=3,name="HR"},
        };
        foreach (WebApi.Entities.Role s in roles)
        {
            context.Roles.Add(s);
        }
        //Add default admin
        if (context.Users.Any())
        {
            return;
        }
        context.Users.Add(new WebApi.Entities.User { id = 1, username="admin", firstname = "admin", lastname = "admin", email = "admin@admin.adm", token = "default", password = BCrypt.Net.BCrypt.HashPassword("admin"), Role=roles[2] });
        context.SaveChanges();

        //Applies any pending migrations for the context to the database.Will create the database if it does not already exist.
        //Note that this API is mutually exclusive with DbContext.Database.EnsureCreated().
        //EnsureCreated does not use migrations to create the database and therefore the
        //database that is created cannot be later updated using migrations.
        context.Database.Migrate();
    }
}