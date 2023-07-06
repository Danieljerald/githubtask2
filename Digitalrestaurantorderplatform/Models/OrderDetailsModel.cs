using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
#nullable disable
namespace Digitalrestaurantorderplatform.Models;


public class OrderDetailsModel
{
    [Required][RegularExpression("^[a-zA-Z]{5,}$",ErrorMessage ="Name should contain letters")]
    public string name{get; set;}
    [Required]
    public string empId{get; set;}
    [Required]
    public string address{get; set;}
    [Required][RegularExpression("^[6789][0-9]{9}$",ErrorMessage ="Check Mobile number again")]
    public string mobileNo{get; set;}

    
}
    
