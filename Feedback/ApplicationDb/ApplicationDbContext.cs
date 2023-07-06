using Microsoft.EntityFrameworkCore;
using Webapi.Models;
#nullable disable
namespace web.ApplicationDb
{
    public class ApplicationDbContext:DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options):base (options)
        {

        }

        public DbSet<FeedbackModel>  feedbacks{get; set;}

        
    }
}
