using System.ComponentModel.DataAnnotations;

namespace Digitalrestaurantorderplatform.Models
#nullable disable
{
    public class LoginModel
    {
    private string Name;
    private string Password;

        [Required]
        public string name{get{return Name;}set{Name=value;}}
        [Required]
        public string password {get{return Password;}set{Password=value;}}
        
    }
}