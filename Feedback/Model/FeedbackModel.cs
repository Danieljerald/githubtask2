using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
#nullable disable

namespace Webapi.Models;
public class FeedbackModel
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int sno{get;set;}
    [Required]
    public string username{get;set;}
    [Required]
    public string comment{get; set;}
}