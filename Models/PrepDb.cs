namespace DiscountAPI.Models;
using Microsoft.EntityFrameworkCore;

public static class PrepDb
{
  public static void PrepPopulation(IApplicationBuilder app, bool isProd)
  {
    using (var serviceScope = app.ApplicationServices.CreateScope())
    {
      SeedData(serviceScope.ServiceProvider.GetService<AppDbContext>(), isProd);
    }
  }

  private static void SeedData(AppDbContext context, bool isProd)
  {
    if (isProd)
    {
      if (!context.Database.EnsureCreated())
      {
        Console.WriteLine("----> Applying Migration...");
        try
        {
          context.Database.Migrate();
        }
        catch (System.Exception ex)
        {
          Console.WriteLine("Could not apply migration: " + ex.Message);
        }
      }
      else
      {
        Console.WriteLine("Database existed!");
      }

    }
  }
}