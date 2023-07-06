using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
#nullable disable
namespace Digitalrestaurantorderplatform.Models;

public class FeedbackModel
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Key]
    public int sno{get;set;}
    [Required]
    public string username{get;set;}
    [Required]
    public string comment{get; set;}
}