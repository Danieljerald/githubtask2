using Microsoft.AspNetCore.Mvc;
using web.ApplicationDb;
using Webapi.Models;
#nullable disable

namespace Feedback.Controllers;

[ApiController]
[Route("api/[controller]")]
public class FeedbackController: ControllerBase
{
    private ApplicationDbContext _database;
   public FeedbackController(ApplicationDbContext database)
   {
         _database=database;
   }
   [HttpGet]
   public async Task<IEnumerable<FeedbackModel>> getFeedback()
   {
        var listoffeedback =_database.feedbacks.ToList();
        return listoffeedback;
    }
    [HttpPost]
    public async Task<bool> postFeedback(FeedbackModel feedbackModel)
    {
        _database.feedbacks.Add(feedbackModel);
        _database.SaveChanges();
        return true;
    }
    [HttpDelete]
    public async Task<bool> deleteFeedbacks()
    {
        foreach (var item in _database.feedbacks)
        {
            _database.feedbacks.Remove(item);
        }
        _database.SaveChanges();
        return true;
    }

}