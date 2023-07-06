using Microsoft.EntityFrameworkCore;
using Digitalrestaurantorderplatform.Models;
#nullable disable
public class AppDbContext:DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options):base (options)
    {

    }
    public DbSet<SignUpModel>  customerlist{get; set;}

    public DbSet<FeedbackModel>  feedbacks{get; set;}

    
}